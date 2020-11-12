﻿//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System;
using System.Collections;
using FPSBuilder.Core.Animation;
using FPSBuilder.Core.Input;
using FPSBuilder.Core.Managers;
using FPSBuilder.Core.Player;
using FPSBuilder.Interfaces;
using UnityEngine;

namespace FPSBuilder.Core.Weapons
{
    public class Arms : MonoBehaviour, IWeapon
    {
        /// <summary>
        /// Defines the reference to the character’s Main Camera transform.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("Defines the reference to the character’s Main Camera transform.")]
        protected Transform m_CameraTransformReference;

        /// <summary>
        /// Defines the reference to the First Person Character Controller.
        /// </summary>
        [SerializeField]
        [Required]
        [Tooltip("Defines the reference to the First Person Character Controller.")]
        protected FirstPersonCharacterController m_FPController;

        /// <summary>
        /// Defines the maximum range of attacks.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the maximum range of attacks.")]
        protected float m_Range = 1.5f;

        /// <summary>
        /// Defines the frequency of attacks.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the frequency of attacks.")]
        protected float m_AttackRate = 0.35f;

        /// <summary>
        /// Defines the force applied by each attack.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines the force applied by each attack.")]
        protected float m_Force = 20;

        /// <summary>
        /// Defines the minimum and maximum damage value inflicted by each attack.
        /// </summary>
        [SerializeField]
        [MinMaxSlider(0, 100, "Defines the minimum and maximum damage value inflicted by each attack.")]
        protected Vector2 m_Damage = new Vector2(15, 30);

        /// <summary>
        /// The Layers affected by a attack.
        /// </summary>
        [SerializeField]
        [Tooltip("The Layers affected by a attack.")]
        private LayerMask m_AffectedLayers = 1;

        /// <summary>
        /// The WeaponSwing component.
        /// </summary>
        [SerializeField]
        protected WeaponSwing m_WeaponSwing = new WeaponSwing();

        /// <summary>
        /// The MotionAnimation component.
        /// </summary>
        [SerializeField]
        protected MotionAnimation m_MotionAnimation = new MotionAnimation();

        /// <summary>
        /// The ArmsAnimator component.
        /// </summary>
        [SerializeField]
        protected ArmsAnimator m_ArmsAnimator = new ArmsAnimator();

        protected Camera m_Camera;
        protected bool m_ArmsActive;
        protected float m_NexAttackTime;
        protected float m_NextInteractTime;

        protected Button m_FireButton;
        protected Button m_AimButton;

        #region PROPERTIES

        /// <summary>
        /// Returns the unique identifier of this weapon.
        /// </summary>
        public int Identifier => GetInstanceID();

        /// <summary>
        /// Returns the viewmodel (GameObject) of this weapon.
        /// </summary>
        public GameObject Viewmodel => gameObject;

        /// <summary>
        /// Returns true if this weapon is ready to be replaced, false otherwise.
        /// </summary>
        public virtual bool CanSwitch => m_ArmsActive && m_FPController.State != MotionState.Running && m_NexAttackTime < Time.time;

        /// <summary>
        /// Returns true if the character is not performing an action that prevents him from using items, false otherwise.
        /// </summary>
        public virtual bool CanUseItems => m_ArmsActive && m_FPController.State != MotionState.Running && m_NexAttackTime < Time.time;

        /// <summary>
        /// Returns true if the character is not performing an action that prevents him from vaulting, false otherwise.
        /// </summary>
        public virtual bool CanVault => m_ArmsActive && m_NexAttackTime < Time.time;

        /// <summary>
        /// Returns the size of the arms.
        /// </summary>
        public float Size => m_Range;

        /// <summary>
        /// The duration in seconds of the hide animation.
        /// </summary>
        public float HideAnimationLength => m_ArmsAnimator.HideAnimationLength;

        /// <summary>
        /// The duration in seconds of the interact animation.
        /// </summary>
        public float InteractAnimationLength => m_ArmsAnimator.InteractAnimationLength;

        /// <summary>
        /// The time required to send the activation signal to the object the character is interacting with.
        /// </summary>
        public float InteractDelay => m_ArmsAnimator.InteractDelay;

        /// <summary>
        /// Returns true if an attack is being performed, false otherwise.
        /// </summary>
        public bool MeleeAttacking => m_NexAttackTime > Time.time;

        /// <summary>
        /// Returns true if the character is interacting with some object, false otherwise.
        /// </summary>
        public bool Interacting => m_NextInteractTime > Time.time;

        /// <summary>
        /// Returns true if the player is not performing any action, false otherwise.
        /// </summary>
        public bool Idle => !MeleeAttacking && !MeleeAttacking && !Interacting;

        #endregion

        /// <summary>
        /// Validates the references before starting the script.
        /// </summary>
        protected virtual void Awake()
        {
            if (!m_CameraTransformReference)
            {
                throw new Exception("Camera Transform Reference was not assigned");
            }

            if (!m_FPController)
            {
                throw new Exception("FPController was not assigned");
            }
        }

        protected virtual void Start()
        {
            m_Camera = m_CameraTransformReference.GetComponent<Camera>();

            InitSwing(transform);
            DisableShadowCasting();

            // Buttons
            m_FireButton = InputManager.FindButton("Fire");
            m_AimButton = InputManager.FindButton("Aim");

            // Events callbacks
            m_FPController.PreJumpEvent += WeaponJump;
            m_FPController.LandingEvent += WeaponLanding;
            m_FPController.VaultEvent += Vault;
            m_FPController.GettingUpEvent += Vault;
        }

        protected virtual void Update()
        {
            if (m_ArmsActive)
            {
                if (m_FPController.Controllable)
                    HandleInput();

                m_FPController.ReadyToVault = CanVault;
                m_ArmsAnimator.SetSpeed(m_FPController.State == MotionState.Running);
            }

            m_WeaponSwing.Swing(transform.parent, m_FPController);
            m_MotionAnimation.MovementAnimation(m_FPController);
            m_MotionAnimation.BreathingAnimation(m_FPController.IsAiming ? 0 : 1);
            m_Camera.fieldOfView = Mathf.Lerp(m_Camera.fieldOfView, GameplayManager.Instance.FieldOfView, Time.deltaTime * 10);
        }

        /// <summary>
        /// Draw the weapon.
        /// </summary>
        public virtual void Select()
        {
            // Animator
            if (!m_ArmsAnimator.Initialized)
                m_ArmsAnimator.Init(m_FPController);

            m_ArmsAnimator.Draw();
            StartCoroutine(Draw());
        }

        /// <summary>
        /// Deselect the weapon.
        /// </summary>
        public virtual void Deselect()
        {
            m_ArmsActive = false;
            m_FPController.ReadyToVault = false;
            m_ArmsAnimator.Hide();
        }

        /// <summary>
        /// Method used to verify the actions the player wants to execute.
        /// </summary>
        protected virtual void HandleInput()
        {
            bool canAttack = m_FPController.State != MotionState.Running && m_NexAttackTime < Time.time && m_NextInteractTime < Time.time;

            if (canAttack)
            {
                if (InputManager.GetButtonDown(m_FireButton))
                {
                    m_ArmsAnimator.LeftAttack();
                    StartCoroutine(Attack());
                }
                else if (InputManager.GetButtonDown(m_AimButton))
                {
                    m_ArmsAnimator.RightAttack();
                    StartCoroutine(Attack());
                }
            }
        }

        /// <summary>
        /// Applies force and damage to objects hit by attacks.
        /// </summary>
        protected virtual IEnumerator Attack()
        {
            m_NexAttackTime = Time.time + m_AttackRate;

            // Wait 0.1 seconds before applying damage/force.
            yield return new WaitForSeconds(0.1f);

            Vector3 direction = m_CameraTransformReference.TransformDirection(Vector3.forward);
            Vector3 origin = m_CameraTransformReference.transform.position;

            Ray ray = new Ray(origin, direction);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, m_Range, m_AffectedLayers, QueryTriggerInteraction.Collide))
            {
                m_ArmsAnimator.Hit(hitInfo.point);

                // If hit a rigidbody applies force to push.
                Rigidbody rigidBody = hitInfo.collider.GetComponent<Rigidbody>();
                if (rigidBody)
                {
                    rigidBody.AddForce(direction * m_Force, ForceMode.Impulse);
                }

                if (hitInfo.transform.root != transform.root)
                {
                    IProjectileDamageable damageableTarget = hitInfo.collider.GetComponent<IProjectileDamageable>();
                    damageableTarget?.ProjectileDamage(UnityEngine.Random.Range(m_Damage.x, m_Damage.y), transform.root.position, hitInfo.point, 0);
                }
            }
        }

        /// <summary>
        /// Wait until the weapon is selected and then enable its features.
        /// </summary>
        protected virtual IEnumerator Draw()
        {
            yield return new WaitForSeconds(m_ArmsAnimator.DrawAnimationLength);
            m_ArmsActive = true;
        }

        #region ANIMATIONS

        /// <summary>
        /// Start the WeaponSwing component and move the weapon to the transform that will be animated.
        /// </summary>
        /// <param name="weaponSwing">The weapon transform.</param>
        protected void InitSwing(Transform weaponSwing)
        {
            if (!weaponSwing.parent.name.Equals("WeaponSwing"))
            {
                Transform parent = weaponSwing.parent.Find("WeaponSwing");

                if (parent != null)
                {
                    weaponSwing.parent = parent;
                }
                else
                {
                    GameObject weaponController = new GameObject("WeaponSwing");
                    weaponController.transform.SetParent(weaponSwing.parent, false);
                    weaponSwing.parent = weaponController.transform;
                }
            }
            m_WeaponSwing.Init(weaponSwing.parent, m_MotionAnimation.ScaleFactor);
        }

        /// <summary>
        /// Event method that simulates the effect of jump on the motion animation component.
        /// </summary>
        protected void WeaponJump()
        {
            if (m_ArmsActive)
                StartCoroutine(m_MotionAnimation.JumpAnimation.Play());
        }

        /// <summary>
        /// Event method that simulates the effect of landing on the motion animation component.
        /// </summary>
        protected void WeaponLanding(float fallDamage)
        {
            if (m_ArmsActive)
                StartCoroutine(m_MotionAnimation.LandingAnimation.Play());
        }

        #endregion

        /// <summary>
        /// Deactivates the shadows created by the weapon.
        /// </summary>
        public void DisableShadowCasting()
        {
            // For each object that has a renderer inside the weapon gameObject
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
            {
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
        }

        /// <summary>
        /// Plays the interaction animation.
        /// </summary>
        public virtual void Interact()
        {
            m_NextInteractTime = Time.time + Mathf.Max(InteractAnimationLength, InteractDelay);
            m_ArmsAnimator.Interact();
        }

        /// <summary>
        /// Event method that simulates the effect of vaulting on the motion animation component.
        /// </summary>
        protected virtual void Vault()
        {
            if (m_ArmsActive)
                m_ArmsAnimator.Vault();
        }
    }
}

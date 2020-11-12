﻿//=========== Copyright (c) GameBuilders, All rights reserved. ================//

using System.Collections;
using UnityEngine;

namespace FPSBuilder.Core.Animation
{
    /// <summary>
    /// The Lerp Animation class is used to simulates interpolation animations that move a transform to a destination
    /// and retreat to their original position.
    /// </summary>
    [System.Serializable]
    public class LerpAnimation
    {
        /// <summary>
        /// Defines the animation’s target (destination) position.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the animation’s target (destination) position.")]
        private Vector3 m_TargetPosition;

        /// <summary>
        /// Defines the animation’s target rotation.
        /// </summary>
        [SerializeField]
        [Tooltip("Defines the animation’s target rotation.")]
        private Vector3 m_TargetRotation;

        /// <summary>
        /// Defines how long the animation will take to move from the origin to the destination.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how long the animation will take to move from the origin to the destination.")]
        private float m_Duration;

        /// <summary>
        /// Defines how long the animation will take to retreat from the destination to the original position.
        /// </summary>
        [SerializeField]
        [MinMax(0, Mathf.Infinity)]
        [Tooltip("Defines how long the animation will take to retreat from the destination to the original position.")]
        private float m_ReturnDuration;

        /// <summary>
        /// The current position of the transform.
        /// </summary>
        public Vector3 Position
        {
            get;
            set;
        }

        /// <summary>
        /// The current rotation of the transform.
        /// </summary>
        public Vector3 Rotation
        {
            get;
            set;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <param name="targetRotation"></param>
        /// <param name="duration"></param>
        /// <param name="returnDuration"></param>
        public LerpAnimation(Vector3 targetPosition, Vector3 targetRotation, float duration = 0.25f, float returnDuration = 0.25f)
        {
            m_TargetPosition = targetPosition;
            m_TargetRotation = targetRotation;
            m_Duration = duration;
            m_ReturnDuration = returnDuration;
        }

        /// <summary>
        /// Define the target position and rotation.
        /// </summary>
        public void SetTargets(Vector3 targetPosition, Vector3 targetRotation)
        {
            m_TargetPosition = targetPosition;
            m_TargetRotation = targetRotation;
        }

        /// <summary>
        /// Interpolates the position and rotation and retreat to the original position.
        /// </summary>
        public IEnumerator Play()
        {
            Vector3 initialPos = Position;
            Vector3 initialRot = Rotation;

            // Make the GameObject move to target slightly.
            for (float t = 0; t <= m_Duration; t += Time.deltaTime)
            {
                Position = Vector3.Lerp(initialPos, m_TargetPosition, t / m_Duration);
                Rotation = Vector3.Lerp(initialRot, m_TargetRotation, t / m_Duration);
                yield return new WaitForFixedUpdate();
            }

            // Make it move back to its origin.
            for (float t = 0; t <= m_ReturnDuration; t += Time.deltaTime)
            {
                Position = Vector3.Lerp(m_TargetPosition, Vector3.zero, t / m_ReturnDuration);
                Rotation = Vector3.Lerp(m_TargetRotation, Vector3.zero, t / m_ReturnDuration);
                yield return new WaitForFixedUpdate();
            }

            Position = Vector3.zero;
            Rotation = Vector3.zero;
        }

        /// <summary>
        /// Immediately interpolates the position and rotation to the to the original position.
        /// </summary>
        public IEnumerator Stop()
        {
            // Make it move back to its origin.
            for (float t = 0; t <= m_ReturnDuration; t += Time.deltaTime)
            {
                Position = Vector3.Lerp(m_TargetPosition, Vector3.zero, t / m_ReturnDuration);
                Rotation = Vector3.Lerp(m_TargetRotation, Vector3.zero, t / m_ReturnDuration);
                yield return new WaitForFixedUpdate();
            }

            Position = Vector3.zero;
            Rotation = Vector3.zero;
        }
    }
}

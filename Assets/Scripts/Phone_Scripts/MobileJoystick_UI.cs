using UnityEngine;
using UnityEngine.UI;

public class MobileJoystick_UI : MonoBehaviour
{
    public Image backgroundCircle;
    public Image mainButton;

    public float maxRadius = 150f;
    public float deadZone = 19f;

    //Use this in your movement script for the input control
    public Vector3 moveDirection;

    private Rect defaultArea;
    private Vector2 touchOffset;
    private Vector2 currentTouchPos;
    private int touchID;
    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        RectTransform defaultRect = mainButton.rectTransform;

        Debug.Log($"Pos: {defaultRect.position}");
        Debug.Log($"Size: {defaultRect.sizeDelta}");

        //Save the default location of the joystick button to be used later for input detection
        defaultArea = new Rect(defaultRect.position.x, defaultRect.position.y, defaultRect.sizeDelta.x, defaultRect.sizeDelta.y);

        currentTouchPos = defaultArea.position;

/*#if (UNITY_ANDROID || UNITY_IOS || UNITY_WP8 || UNITY_WP8_1) && !UNITY_EDITOR
        gameObject.SetActive(true);
#else
        gameObject.SetActive(false);
#endif*/
    }

    // Update is called once per frame
    void Update()
    {
        //Handle joystick movement
#if (UNITY_ANDROID || UNITY_IOS || UNITY_WP8 || UNITY_WP8_1) && !UNITY_EDITOR
        //Mobile touch input
        for (var i = 0; i < Input.touchCount; ++i)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                MobileButtonsCheck(new Vector2(touch.position.x, Screen.height - touch.position.y), touch.fingerId);
            }

            if (touch.phase == TouchPhase.Moved )
            {
                if(isActive && touchID == touch.fingerId)
                {
                    currentTouchPos = touch.position;
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                MobileButtonStop(touch.fingerId);
            }
        }
#else
        //Desktop mouse input for editor testing
        if (Input.GetMouseButtonDown(0))
        {
            MobileButtonsCheck(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y), -1);
        }

        if (Input.GetMouseButtonUp(0))
        {
            MobileButtonStop(-1);
        }

        currentTouchPos = Input.mousePosition;
#endif

        //Moving
        if (isActive)
        {
            moveDirection = new Vector3(currentTouchPos.x - touchOffset.x - defaultArea.x, currentTouchPos.y - touchOffset.y - defaultArea.y);

            if(moveDirection.magnitude > maxRadius)
            {
                moveDirection = moveDirection.normalized * maxRadius;
            }
            else if(moveDirection.magnitude < deadZone)
            {
                moveDirection = Vector3.zero;
            }

            mainButton.rectTransform.position = new Vector3(defaultArea.x, defaultArea.y) + moveDirection;

            if (Mathf.Abs(moveDirection.x) < deadZone)
            {
                moveDirection.x = 0;
            }
            else
            {
                moveDirection.x = Mathf.Clamp(moveDirection.x / maxRadius, -1.000f, 1.000f);
            }

            if (Mathf.Abs(moveDirection.y) < deadZone)
            {
                moveDirection.y = 0;
            }
            else
            {
                moveDirection.y = Mathf.Clamp(moveDirection.y / maxRadius, -1.000f, 1.000f);
            }

            Debug.Log($"Move Direction: {moveDirection}");
        }
        else
        {
            mainButton.rectTransform.position = new Vector3(defaultArea.x, defaultArea.y);
            moveDirection = Vector2.zero;
        }
    }

    //Here we check if the clicked/tapped position is inside the joystick button
    void MobileButtonsCheck(Vector2 touchPos, int tID)
    {
        Debug.Log(defaultArea);
        //Move controller
        if (defaultArea.Contains(new Vector2(touchPos.x + mainButton.rectTransform.sizeDelta.x / 2, Screen.height - touchPos.y + mainButton.rectTransform.sizeDelta.y / 2)) && !isActive)
        {
            isActive = true;
            touchOffset = new Vector2(touchPos.x - defaultArea.x, Screen.height - touchPos.y - defaultArea.y);
            currentTouchPos = new Vector2(touchPos.x, Screen.height - touchPos.y);
            touchID = tID;
        }
    }

    //Here we release the previously active joystick if we release the mouse button/finger from the screen
    void MobileButtonStop(int tID)
    {
        if (isActive && touchID == tID)
        {
            isActive = false;
            touchOffset = Vector2.zero;
            touchID = -1;
        }
    }
}

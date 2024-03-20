using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputManager : MonoBehaviour
{
    public static MobileInputManager Instance;

    RefactoredCharacterController _chController;

    public List<touchLocation> touches = new List<touchLocation>();

    [SerializeField]
    //private Joystick _joystick;
    private float _joystickDeadAngle = 0;

    [SerializeField]
    float touchMaxTime = 0.2f;
    [SerializeField]
    float slideMaxTime = 0.5f;

    public int xInput { get { return _xInput; } }
    private int _xInput;

    public Vector2 _fingerMovement;

    private float minFingerDisplacement = 60f;

    void Start()
    {
        _chController = FindObjectOfType<RefactoredCharacterController>();
        if (Instance == null)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //GetJoystickInput();

        GetTouchesOnScreen(new Vector2 (Screen.width/2, 0), new Vector2(Screen.width, Screen.height), _fingerMovement);
        if (RefactoredCharacterController._isGrounded)
        {
            if (_fingerMovement == Vector2.up)
            {
                _chController.JumpDown();
                //Cambia la fuerza del salto segun la distancia recorrida por el dedo
            }
            else if (Mathf.Abs(_fingerMovement.x) == 1)
            {
                _chController.SlideDown();
            }
        }
        else
        {
            if (_fingerMovement == Vector2.down)
            {
                _chController.SlideDown();
            }else if (Mathf.Abs(_fingerMovement.x) == 1)
            {
                _chController.JumpDown();
            }
            /* DEjar pulsado un dedo en pantalla para hacer WR
            else if ()
            {

            }
            */
        }

        
    }

    /*
    void GetJoystickInput()
    {
        if (_joystick.Horizontal < -_joystickDeadAngle)
            _xInput = -1;
        else if (_joystick.Horizontal > _joystickDeadAngle)
            _xInput = 1;
        else
            _xInput = 0;
    }
    */

    void GetTouchesOnScreen(Vector2 screenZone0, Vector2 screenZone1, Vector2 returnMove)
    {
        int i = 0;
        while (i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            
            if (t.phase == TouchPhase.Began)
            {
                if (t.position.x < screenZone0.x || t.position.x > screenZone1.x || t.position.y < screenZone0.y || t.position.y > screenZone1.y) return;
                touches.Add(new touchLocation(t.fingerId, t.position));

            }
            else if (t.phase == TouchPhase.Ended)
            {
                touchLocation thisTouch = touches.Find(touchLocation => touchLocation.touchId == t.fingerId);
                if (thisTouch != null)
                {
                    if (LookForFingerMovement(thisTouch.startPosition, t.position, thisTouch.startTime, ref returnMove) < touchMaxTime && RefactoredCharacterController._isGrounded)
                    {
                        _chController.JumpDown();
                    }

                    touches.RemoveAt(touches.IndexOf(thisTouch));
                }
            }
            i++;
        }
    }

    float LookForFingerMovement(Vector2 startPos, Vector2 finishPos, float startTime, ref Vector2 returnMove)
    {
        float totalTime = Time.time - startTime;

        if (totalTime > slideMaxTime) return totalTime;

        Vector2 movement = finishPos - startPos;
        if(movement.x > minFingerDisplacement)
        {
            movement.x = 1;
        }else if(movement.x < -minFingerDisplacement)
        {
            movement.x = -1;
        }
        else
        {
            movement.x = 0;
        }

        if (movement.y > minFingerDisplacement)
        {
            movement.y = 1;
        }
        else if (movement.y < -minFingerDisplacement)
        {
            movement.y = -1;
        }
        else
        {
            movement.y = 0;
        }

        //Debug.Log(movement);
        returnMove = movement;
        return totalTime;
    }

}
    /*
    void DetectSwipeOnly1Finger()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButtonUp(0))
        {
            secondPressPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            currentSwipe = new Vector2(secondPressPosition.x - firstPressPosition.x, secondPressPosition.y - firstPressPosition.y);

            if (currentSwipe.y > 120f)
            {
                print("SWIPE UP!");
            }
            if (currentSwipe.y < -120f)
            {
                print("SWIPE DOWN!");
            }
            if (currentSwipe.x < -120f)
            {
                print("SWIPE LEFT!");
            }
            if (currentSwipe.x > 120f)
            {
                print("SWIPE RIGHT!");
            }
        }
    }



            int i = 0;
        while(i < Input.touchCount)
        {
            Touch t = Input.GetTouch(i);
            if(t.phase == TouchPhase.Began)
            {
                Debug.Log("TOUCH BEGAN");
            }else if(t.phase == TouchPhase.Ended)
            {
                Debug.Log("TOUCH ENDED");
            }else if(t.phase == TouchPhase.Moved)
            {
                Debug.Log("TOUCH IS MOVING");
            }
            ++i;
        }
    */


public class touchLocation
{
    public int touchId;
    public Vector2 startPosition;
    public float startTime;
    public touchLocation(int newTouchId, Vector2 newStartPosition)
    {
        touchId = newTouchId;
        startPosition = newStartPosition;
        startTime = Time.time;
    }
}


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class DesktopMovementControls : MovementControlsBase
{
    [SerializeField] float _deadZoneRadius = 0.1f;
    float _rollAmount = 0;

    // Speed of the simulated mouse movement
    public float horizontalSpeed = 1f;
    public float verticalSpeed = 1f;

    // Position trackers for simulated mouse movement
    private float simulatedMouseX;
    private float simulatedMouseY;

    // Direction flags to control linear movement
    private bool movingRight = true;
    private bool movingUp = true;
    private bool timeerRunning = false;
    Vector2 ScreenCenter => new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

    void Start()
    {
        // Initialize simulated mouse positions at the center of the screen
        simulatedMouseX = ScreenCenter.x;
        simulatedMouseY = ScreenCenter.y;
    }

     IEnumerator DelayInControl(bool value)
    {
        float timetoDelay = UnityEngine.Random.Range(0.5f, 3f);
        yield return new WaitForSeconds(timetoDelay);
        movingRight = !value;
        timeerRunning = value;
    }
    public override float YawAmount
    {
        get
        {
            // Move left or right based on the direction flag
            if (movingRight)
            {
                simulatedMouseX += horizontalSpeed * Time.deltaTime * Screen.width;
                if (simulatedMouseX >= Screen.width)
                {
                    simulatedMouseX = Screen.width;
                    if (!timeerRunning)
                    {
                        StartCoroutine(DelayInControl(true));
                    }
                     // Switch direction to left
                }
            }
            else
            {
                simulatedMouseX -= horizontalSpeed * Time.deltaTime * Screen.width;
                if (simulatedMouseX <= 0)
                {
                    simulatedMouseX = 0;
                    movingRight = true; // Switch direction to right
                    if (timeerRunning)
                    {
                        StartCoroutine(DelayInControl(false));
                    }
                }
            }

            // Calculate yaw based on the simulated mouse position
            float yaw = (simulatedMouseX - ScreenCenter.x) / ScreenCenter.x;
            return Mathf.Abs(yaw) > _deadZoneRadius ? yaw : 0f;
        }
    }

    public override float PitchAmount
    {
        get
        {
            // Move up or down based on the direction flag
            if (movingUp)
            {
                simulatedMouseY += verticalSpeed * Time.deltaTime * Screen.height;
                if (simulatedMouseY >= Screen.height)
                {
                    simulatedMouseY = Screen.height;
                    movingUp = false; // Switch direction to down
                }
            }
            else
            {
                simulatedMouseY -= verticalSpeed * Time.deltaTime * Screen.height;
                if (simulatedMouseY <= 0)
                {
                    simulatedMouseY = 0;
                    movingUp = true; // Switch direction to up
                }
            }

            // Calculate pitch based on the simulated mouse position
            float pitch = (simulatedMouseY - ScreenCenter.y) / ScreenCenter.y;
            return Mathf.Abs(pitch) > _deadZoneRadius ? pitch * -1 : 0f;
        }
    }

    public override float RollAmount
    {
        get
        {
            float roll;
            if (Keyboard.current.qKey.isPressed)
            {
                roll = 1f;
            }
            else
            {
                roll = Keyboard.current.eKey.isPressed ? -1f : 0f;
            }

            _rollAmount = Mathf.Lerp(_rollAmount, roll, Time.deltaTime * 3f);
            return _rollAmount;
        }
    }

    public override float ThrustAmount =>
        Keyboard.current.wKey.isPressed ? 1f : (Keyboard.current.sKey.isPressed ? -1f : 0f);
}
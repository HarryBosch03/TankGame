using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[SelectionBase]
[DisallowMultipleComponent]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(TankMovement))]
public class PlayerController : MonoBehaviour
{
    public float angleScale;
    public bool useThumbstickAim;

    [Space]
    public UnityEvent<float>[] shootEvents;

    PlayerInput inputComponent;
    TankMovement movement;
    
    Vector2 shootPoint;
    bool screenSpaceShootPoint;

    UnityEngine.InputSystem.Controls.TouchControl aimTouch;

    public float Throttle { get => movement.ThrottleInput; set => movement.ThrottleInput = value; }
    public float Steering { get => movement.TurnInput; set => movement.TurnInput = value; }
    
    private void Awake()
    {
        movement = GetComponent<TankMovement>();
    }

    private void OnEnable()
    {
        if (TryGetComponent(out inputComponent))
        {
            inputComponent.onActionTriggered += OnActionTriggered;
        }
    }

    private void OnDisable()
    {
        if (inputComponent)
        {
            inputComponent.onActionTriggered -= OnActionTriggered;
        }

        using (var save = Save.Get())
        {
            save.lastMaxScore = save.maxScore;
            save.maxScore = Mathf.Max(save.maxScore, (int)Stats.Main.score.Value);
            save.maxTanksDead = Mathf.Max(save.maxTanksDead, (int)Stats.Main.tanksDestroyed.Value);
            save.maxTimeAlive = Mathf.Max(save.maxTimeAlive, Stats.Main.timeAlive.Value);
        }
    }

    private void Update()
    {
        if (screenSpaceShootPoint) movement.ShootPoint = Camera.main.ScreenToWorldPoint(shootPoint);
        else movement.ShootPoint = shootPoint;
        ProcessTouch();
    }

    private void ProcessTouch()
    {
        if (useThumbstickAim) return;

        if (aimTouch == null)
        {
            if (Touchscreen.current == null) return;

            foreach (var touch in Touchscreen.current.touches)
            {
                Vector2 pos = touch.position.ReadValue();

                if (!touch.press.wasPressedThisFrame) continue;
                if (pos.x < 650.0f && pos.y < 650.0f) continue;

                ShootPress();
                aimTouch = touch;
                break;
            }
        }

        if (aimTouch != null)
        {
            if (!aimTouch.press.isPressed)
            {
                aimTouch = null;
                ShootRelease();
                return;
            }

            shootPoint = aimTouch.position.ReadValue();
            screenSpaceShootPoint = true;
        }
    }

    private void OnActionTriggered(InputAction.CallbackContext callbackContext)
    {
        if (Application.isMobilePlatform)
        {
            Debug.Log("uh oh");
            return;
        }

        switch (callbackContext.action.name)
        {
            case "Throttle":
                Throttle = callbackContext.ReadValue<float>();
                break;
            case "Turning":
                Steering = callbackContext.ReadValue<float>();
                break;
            case "Shoot Point":
                shootPoint = callbackContext.ReadValue<Vector2>();
                screenSpaceShootPoint = true;
                break;
            case "Shoot 1":
                if (shootEvents.Length > 0) shootEvents[0].Invoke(callbackContext.ReadValue<float>());
                break;
            case "Shoot 2":
                if (shootEvents.Length > 1) shootEvents[1].Invoke(callbackContext.ReadValue<float>());
                break;
            default:
                break;
        }
    }

    public void SetMoveDirection (Vector2 vector)
    {
        vector = Vector2.ClampMagnitude(vector, 1.0f);

        float targetAngle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        float currentAngle = movement.transform.eulerAngles.z;
        float deltaAngle = Mathf.DeltaAngle(targetAngle, currentAngle);

        movement.ThrottleInput = Vector2.Dot(vector, transform.right);
        movement.TurnInput = Mathf.Clamp(deltaAngle * angleScale, -1.0f, 1.0f) * vector.magnitude;
    }

    public void SetShootDirection (Vector2 direction)
    {
        useThumbstickAim = false;

        shootPoint = (Vector2)transform.position + direction;
        screenSpaceShootPoint = false;
    }

    public void SetShootPoint (Vector2 screenPoint)
    {
        shootPoint = screenPoint;
        screenSpaceShootPoint = true;
    }

    public void ShootPress ()
    {
        if (useThumbstickAim) return;

        shootEvents[1].Invoke(1.0f);
    }

    public void ShootRelease()
    {
        if (useThumbstickAim) return;

        shootEvents[1].Invoke(0.0f);
        StartCoroutine(ShootCannonMobile());
    }

    private IEnumerator ShootCannonMobile()
    {
        shootEvents[0].Invoke(1.0f);
        yield return null;
        yield return null;
        shootEvents[0].Invoke(0.0f);
    }

    [ContextMenu("Give 1000 Points")]
    public void Give1000Points()
    {
        ScoreCalculator.AwardCheatPoints(1000);
    }
}


using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform aimPivot;
    [SerializeField] private Camera mainCamera;

    [Header("Aim Settings")]
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private float deadZone = 0.2f;
    [SerializeField] private float mouseMovementThreshold = 0.5f;

    private Vector2 gamepadAim;
    private Vector2 lastMousePosition;

    private bool usingGamepad = false;
    private float targetAngle;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (aimPivot != null)
            targetAngle = aimPivot.eulerAngles.z;

        if (Mouse.current != null)
            lastMousePosition =
                Mouse.current.position.ReadValue();
    }

    private void Update()
    {
        if (aimPivot == null)
            return;

        // Detecta movimento real do mouse.
        if (Mouse.current != null)
        {
            Vector2 mousePosition =
                Mouse.current.position.ReadValue();

            if (Vector2.Distance(
                mousePosition,
                lastMousePosition
            ) > mouseMovementThreshold)
            {
                usingGamepad = false;
            }

            lastMousePosition = mousePosition;
        }

        // Calcula a direção da mira.
        if (usingGamepad)
        {
            if (gamepadAim.magnitude > deadZone)
            {
                UpdateTargetAngle(gamepadAim);
            }
        }
        else if (Mouse.current != null &&
                 mainCamera != null)
        {
            Vector2 mousePosition =
                Mouse.current.position.ReadValue();

            Vector3 mouseWorldPosition =
                mainCamera.ScreenToWorldPoint(
                    new Vector3(
                        mousePosition.x,
                        mousePosition.y,
                        -mainCamera.transform.position.z
                    )
                );

            Vector2 direction =
                (Vector2)(mouseWorldPosition -
                aimPivot.position);

            if (direction.sqrMagnitude > 0.001f)
                UpdateTargetAngle(direction);
        }

        // Rotaciona suavemente até o ângulo desejado.
        float currentAngle = aimPivot.eulerAngles.z;

        float newAngle = Mathf.MoveTowardsAngle(
            currentAngle,
            targetAngle,
            rotationSpeed * Time.deltaTime
        );

        aimPivot.rotation =
            Quaternion.Euler(0f, 0f, newAngle);

        // Espelha verticalmente a arma ao mirar
        // para a esquerda, sem inverter o Player.
        Vector3 scale = aimPivot.localScale;
        scale.y = Mathf.Cos(newAngle * Mathf.Deg2Rad) < 0f
            ? -1f
            : 1f;

        aimPivot.localScale = scale;
    }

    private void UpdateTargetAngle(Vector2 direction)
    {
        targetAngle = Mathf.Atan2(
            direction.y,
            direction.x
        ) * Mathf.Rad2Deg;
    }

    // Ação Aim do New Input System.
    public void Aim(InputAction.CallbackContext context)
    {
        gamepadAim = context.ReadValue<Vector2>();

        if (gamepadAim.magnitude > deadZone)
        {
            usingGamepad = true;
        }
    }
}

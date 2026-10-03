using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSO stats;
    [SerializeField] private Transform toolOrigin;
    [SerializeField] private ThermometerUI thermometer;

    private const float size = 2.5f;

    private GameInputs input;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 facing = Vector2.down;
    private bool heating;
    private bool cooling;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new GameInputs();
    }

    private void OnEnable()
    {
        if (input == null)
            input = new GameInputs();

        input.Player.Enable();
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;
        input.Player.Heat.performed += OnHeat;
        input.Player.Heat.canceled += OnHeat;
        input.Player.Cool.performed += OnCool;
        input.Player.Cool.canceled += OnCool;
    }

    private void OnDisable()
    {
        if (input == null) return;

        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMove;
        input.Player.Heat.performed -= OnHeat;
        input.Player.Heat.canceled -= OnHeat;
        input.Player.Cool.performed -= OnCool;
        input.Player.Cool.canceled -= OnCool;
        input.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput.sqrMagnitude > 0.01f)
            facing = moveInput.normalized;
    }

    private void OnHeat(InputAction.CallbackContext context)
    {
        heating = context.ReadValueAsButton();
    }

    private void OnCool(InputAction.CallbackContext context)
    {
        cooling = context.ReadValueAsButton();
    }

    private void Update()
    {
        if (moveInput.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(moveInput.x) * size, size, 1f);
    }

    private void FixedUpdate()
    {
        if (toolOrigin != null)
            toolOrigin.position = rb.position + facing * 1.2f;

        float speed = stats != null ? stats.speed * stats.speedMultiplier : 4f;
        rb.linearVelocity = moveInput * speed;

        ApplyTemperature(Time.fixedDeltaTime);
    }

    private void ApplyTemperature(float dt)
    {
        if (stats == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            toolOrigin != null ? (Vector2)toolOrigin.position : rb.position,
            0.8f,
            stats.thermalMask
        );

        ThermalObject target = null;

        for (int i = 0; i < hits.Length; i++)
        {
            ThermalObject thermal = hits[i].GetComponent<ThermalObject>();
            if (thermal == null)
                thermal = hits[i].GetComponentInParent<ThermalObject>();

            if (thermal != null)
            {
                target = thermal;
                break;
            }
        }

        if (thermometer != null)
            thermometer.SetTarget(target);

        if (target == null) return;

        if (heating)
            target.AddTemperature(stats.heatRate * dt);
        else if (cooling)
            target.AddTemperature(-stats.coolRate * dt);
    }
}
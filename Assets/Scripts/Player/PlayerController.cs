using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerSO stats;
    [SerializeField] private Transform toolOrigin;

    private GameInputs input;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 facing = Vector2.right;
    private bool heating;
    private bool cooling;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new GameInputs();

        if (toolOrigin == null)
            toolOrigin = transform.Find("ToolOrigin");
    }

    private void OnEnable()
    {
        input.Player.Enable();
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;
        input.Player.Heat.started += _ => heating = true;
        input.Player.Heat.canceled += _ => heating = false;
        input.Player.Cool.started += _ => cooling = true;
        input.Player.Cool.canceled += _ => cooling = false;
    }

    private void OnDisable()
    {
        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMove;
        input.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();

        if (moveInput.sqrMagnitude > 0.01f)
            facing = moveInput.normalized;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * (stats.speed * stats.speedMultiplier);

        if (toolOrigin != null)
            toolOrigin.localPosition = facing * 0.55f;

        ApplyTemperature(Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (Mathf.Abs(facing.x) > 0.1f)
            transform.localScale = new Vector3(Mathf.Sign(facing.x), 1f, 1f);
    }

    private void ApplyTemperature(float dt)
    {
        if (heating == cooling) return;
        if (stats == null) return;

        Vector2 point = (Vector2)transform.position + facing * stats.toolRange;
        Collider2D hit = Physics2D.OverlapCircle(point, stats.toolRadius, stats.thermalMask);

        if (hit == null) return;
        if (!hit.TryGetComponent(out ThermalObject thermal))
            thermal = hit.GetComponentInParent<ThermalObject>();
        if (thermal == null) return;

        float delta = heating ? stats.heatRate * dt : -stats.coolRate * dt;
        thermal.AddTemperature(delta);
    }

    private void OnDrawGizmosSelected()
    {
        if (stats == null) return;

        Vector2 dir = Application.isPlaying ? facing : Vector2.right;
        Vector2 point = (Vector2)transform.position + dir * stats.toolRange;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(point, stats.toolRadius);
    }
}
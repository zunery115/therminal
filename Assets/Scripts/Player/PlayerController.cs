using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ThermometerUI thermometer;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float reach = 1.2f;
    [SerializeField] private float toolRadius = 0.45f;
    [SerializeField] private float power = 25f;

    private Vector2 facing = Vector2.down;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (thermometer == null)
            thermometer = FindFirstObjectByType<ThermometerUI>();
    }

    private void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        Vector2 move = Vector2.zero;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) move.x -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) move.x += 1f;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) move.y -= 1f;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) move.y += 1f;

        if (move.sqrMagnitude > 1f)
            move.Normalize();

        if (move.sqrMagnitude > 0.01f)
            facing = move.normalized;

        rb.linearVelocity = move * moveSpeed;

        Vector2 origin = rb.position + facing * reach;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, toolRadius);

        ThermalObject target = null;
        float best = float.MaxValue;

        foreach (Collider2D hit in hits)
        {
            if (hit == null || hit.transform == transform) continue;

            ThermalObject thermal = hit.GetComponent<ThermalObject>();
            if (thermal == null) thermal = hit.GetComponentInParent<ThermalObject>();
            if (thermal == null) continue;

            float distance = Vector2.Distance(origin, hit.ClosestPoint(origin));
            if (distance < best)
            {
                best = distance;
                target = thermal;
            }
        }

        if (thermometer != null)
            thermometer.SetTarget(target);

        if (target == null) return;

        if (keyboard.qKey.isPressed)
            target.AddTemperature(-power * Time.deltaTime);

        if (keyboard.eKey.isPressed)
            target.AddTemperature(power * Time.deltaTime);
    }
}
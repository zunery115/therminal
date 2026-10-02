using UnityEngine;

[RequireComponent(typeof(ThermalObject))]
public class WaterPit : MonoBehaviour
{
    [SerializeField] private ThermalObject thermal;
    [SerializeField] private Collider2D pit;
    [SerializeField] private Transform respawn;

    private void Awake()
    {
        if (thermal == null)
            thermal = GetComponent<ThermalObject>();

        if (pit == null)
            pit = GetComponent<Collider2D>();
    }

    private void LateUpdate()
    {
        if (pit == null) return;

        pit.enabled = true;
        pit.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (thermal == null || thermal.State == ThermalState.Frozen) return;
        if (!other.CompareTag("Player")) return;
        if (respawn == null) return;

        other.transform.position = respawn.position;

        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
    }
}
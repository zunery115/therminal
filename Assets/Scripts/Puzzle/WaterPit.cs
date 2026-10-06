using UnityEngine;

public class WaterPit : MonoBehaviour
{
    [SerializeField] private ThermalObject thermal;
    [SerializeField] private Collider2D pit;
    [SerializeField] private Transform respawn;

    private void Awake()
    {
        if (thermal == null) thermal = GetComponent<ThermalObject>();
        if (pit == null) pit = GetComponent<Collider2D>();
    }

    private void LateUpdate()
    {
        if (pit == null) return;

        // Siempre trigger. Si se apaga, el hielo se vuelve un bloque y no se puede cruzar.
        pit.isTrigger = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Congelado se puede caminar. Solo el agua derretida regresa al respawn.
        if (thermal != null && thermal.State == ThermalState.Frozen) return;
        if (respawn == null) return;

        other.transform.position = respawn.position;

        Rigidbody2D body = other.attachedRigidbody;
        if (body != null) body.linearVelocity = Vector2.zero;
    }
}
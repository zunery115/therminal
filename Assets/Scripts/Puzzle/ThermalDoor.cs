using UnityEngine;

public class ThermalDoor : MonoBehaviour
{
    [SerializeField] private ThermalObject source;
    [SerializeField] private ThermalState openWhen = ThermalState.Expanded;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private SpriteRenderer view;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;

    private void Awake()
    {
        if (view == null) view = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (source != null)
            source.onStateChanged.AddListener(ApplyState);
    }

    private void Start()
    {
        if (source != null)
            ApplyState(source.State);
    }

    private void OnDisable()
    {
        if (source != null)
            source.onStateChanged.RemoveListener(ApplyState);
    }

    public void ApplyState(ThermalState state)
    {
        bool open = state == openWhen;

        // Abierta: se apaga el collider para poder pasar.
        if (doorCollider != null)
            doorCollider.enabled = !open;

        if (view == null) return;

        if (open && openSprite != null)
            view.sprite = openSprite;
        else if (!open && closedSprite != null)
            view.sprite = closedSprite;

        view.color = open ? new Color(1f, 1f, 1f, 0.35f) : Color.white;
    }
}
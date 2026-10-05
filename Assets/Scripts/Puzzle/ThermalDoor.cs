using UnityEngine;

public class ThermalDoor : MonoBehaviour
{
    [SerializeField] private ThermalObject source;
    [SerializeField] private ThermalState openWhen = ThermalState.Expanded;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private SpriteRenderer view;
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;

    private bool isOpen;

    private void Awake()
    {
        if (view == null) view = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (source == null) return;
        ApplyState(source.State);
    }

    public void ApplyState(ThermalState state)
    {
        bool open = state == openWhen;
        if (open == isOpen && view != null && view.sprite == (open ? openSprite : closedSprite))
            return;

        isOpen = open;

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
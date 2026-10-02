using UnityEngine;

public class ThermalDoor : MonoBehaviour
{
    [SerializeField] private ThermalObject source;
    [SerializeField] private ThermalState openWhen = ThermalState.Contracted;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private SpriteRenderer view;
    [SerializeField] private Color closedColor = new Color(0.35f, 0.2f, 0.2f, 1f);
    [SerializeField] private Color openColor = new Color(0.35f, 0.2f, 0.2f, 0.25f);

    private bool isOpen;

    private void Awake()
    {
        if (doorCollider == null)
            doorCollider = GetComponent<Collider2D>();

        if (view == null)
            view = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (source == null) return;

        bool open = source.State == openWhen;
        if (open == isOpen) return;

        isOpen = open;
        Apply(open);
    }

    private void Apply(bool open)
    {
        if (doorCollider != null)
        {
            doorCollider.enabled = true;
            doorCollider.isTrigger = open;
        }

        if (view != null)
            view.color = open ? openColor : closedColor;
    }
}
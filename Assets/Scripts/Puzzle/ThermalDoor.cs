using UnityEngine;

public class ThermalDoor : MonoBehaviour
{
    [SerializeField] private ThermalObject source;
    [SerializeField] private ThermalState openWhen = ThermalState.Contracted;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private SpriteRenderer view;

    [Header("Sprites")]
    [SerializeField] private Sprite closedSprite;
    [SerializeField] private Sprite openSprite;

    private bool isOpen;

    private void Awake()
    {
        if (doorCollider == null)
            doorCollider = GetComponent<Collider2D>();

        if (view == null)
            view = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        if (source == null) return;
        Apply(source.State == openWhen);
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

        if (view == null) return;

        view.color = Color.white;
        view.sprite = open ? openSprite : closedSprite;
    }
}
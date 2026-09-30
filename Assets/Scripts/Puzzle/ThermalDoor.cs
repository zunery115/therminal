using UnityEngine;

public class ThermalDoor : MonoBehaviour
{
    [SerializeField] private ThermalObject source;
    [SerializeField] private ThermalState openWhen = ThermalState.Contracted;
    [SerializeField] private Collider2D doorCollider;
    [SerializeField] private SpriteRenderer view;
    [SerializeField] private Color closedColor = new Color(0.35f, 0.2f, 0.2f);
    [SerializeField] private Color openColor = new Color(0.35f, 0.2f, 0.2f, 0.25f);

    private void Awake()
    {
        if (doorCollider == null)
            doorCollider = GetComponent<Collider2D>();

        if (view == null)
            view = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (source == null) return;
        source.onStateChanged.AddListener(OnStateChanged);
        Refresh(source.State);
    }

    private void OnDisable()
    {
        if (source == null) return;
        source.onStateChanged.RemoveListener(OnStateChanged);
    }

    private void OnStateChanged(ThermalState state)
    {
        Refresh(state);
    }

    private void Refresh(ThermalState state)
    {
        bool open = state == openWhen;

        if (doorCollider != null)
        {
            doorCollider.enabled = true;
            doorCollider.isTrigger = open;
        }

        if (view != null)
            view.color = open ? openColor : closedColor;
    }
}
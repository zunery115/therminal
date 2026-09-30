using UnityEngine;

[RequireComponent(typeof(ThermalObject))]
public class ThermalMetal : MonoBehaviour
{
    [SerializeField] private ThermalObject thermal;
    [SerializeField] private SpriteRenderer view;

    [SerializeField] private Vector3 contractedScale = new Vector3(0.45f, 1f, 1f);
    [SerializeField] private Vector3 normalScale = Vector3.one;
    [SerializeField] private Vector3 expandedScale = new Vector3(1.85f, 1f, 1f);

    [SerializeField] private Color contractedColor = new Color(0.55f, 0.6f, 0.65f);
    [SerializeField] private Color normalColor = new Color(0.62f, 0.62f, 0.66f);
    [SerializeField] private Color expandedColor = new Color(0.78f, 0.42f, 0.32f);

    private void Awake()
    {
        if (thermal == null)
            thermal = GetComponent<ThermalObject>();

        if (view == null)
            view = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (thermal == null) return;
        thermal.onStateChanged.AddListener(Apply);
        Apply(thermal.State);
    }

    private void OnDisable()
    {
        if (thermal == null) return;
        thermal.onStateChanged.RemoveListener(Apply);
    }

    private void Apply(ThermalState state)
    {
        switch (state)
        {
            case ThermalState.Contracted:
                transform.localScale = contractedScale;
                if (view != null) view.color = contractedColor;
                break;

            case ThermalState.Expanded:
                transform.localScale = expandedScale;
                if (view != null) view.color = expandedColor;
                break;

            default:
                transform.localScale = normalScale;
                if (view != null) view.color = normalColor;
                break;
        }
    }
}
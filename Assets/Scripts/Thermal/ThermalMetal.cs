using UnityEngine;

[RequireComponent(typeof(ThermalObject))]
public class ThermalMetal : MonoBehaviour
{
    [SerializeField] private ThermalObject thermal;
    [SerializeField] private SpriteRenderer view;

    [Header("Sprites")]
    [SerializeField] private Sprite contractedSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite expandedSprite;

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

    private void Start()
    {
        if (thermal != null)
            Apply(thermal.State);
    }

    private void Apply(ThermalState state)
    {
        if (view == null) return;

        view.color = Color.white;

        switch (state)
        {
            case ThermalState.Contracted:
                if (contractedSprite != null)
                    view.sprite = contractedSprite;
                break;

            case ThermalState.Expanded:
                if (expandedSprite != null)
                    view.sprite = expandedSprite;
                break;

            default:
                if (normalSprite != null)
                    view.sprite = normalSprite;
                break;
        }
    }
}
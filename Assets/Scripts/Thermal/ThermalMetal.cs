using UnityEngine;

public class ThermalMetal : MonoBehaviour
{
    [SerializeField] private ThermalObject thermal;
    [SerializeField] private SpriteRenderer view;
    [SerializeField] private Sprite contractedSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite expandedSprite;

    private void Awake()
    {
        if (thermal == null) thermal = GetComponent<ThermalObject>();
        if (view == null) view = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (thermal != null)
            thermal.onStateChanged.AddListener(ApplyState);
    }

    private void Start()
    {
        if (thermal != null)
            ApplyState(thermal.State);
    }

    private void OnDisable()
    {
        if (thermal != null)
            thermal.onStateChanged.RemoveListener(ApplyState);
    }

    public void ApplyState(ThermalState state)
    {
        if (view == null) return;

        if (state == ThermalState.Contracted)
        {
            view.color = new Color(0.55f, 0.78f, 1f);
            if (contractedSprite != null) view.sprite = contractedSprite;
        }
        else if (state == ThermalState.Expanded)
        {
            view.color = new Color(1f, 0.45f, 0.3f);
            if (expandedSprite != null) view.sprite = expandedSprite;
        }
        else
        {
            view.color = Color.white;
            if (normalSprite != null) view.sprite = normalSprite;
        }
    }
}
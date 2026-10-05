using UnityEngine;
using UnityEngine.Events;

public enum ThermalState
{
    Contracted,
    Frozen,
    Normal,
    Heated,
    Expanded
}

public class ThermalObject : MonoBehaviour
{
    [SerializeField] private ThermalProfileSO profile;
    [SerializeField] private SpriteRenderer view;
    [SerializeField] private Collider2D solidCollider;
    [SerializeField] private Sprite iceSprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite contractedSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite expandedSprite;

    [SerializeField] private float currentTemperature;
    [SerializeField] private ThermalState currentState;

    public UnityEvent<ThermalState> onStateChanged;

    public ThermalState State => currentState;
    public float Temperature => currentTemperature;

    private void Start()
    {
        EvaluateState(true);
    }

    private void Update()
    {
        if (profile == null || profile.returnRate <= 0f) return;

        currentTemperature = Mathf.MoveTowards(
            currentTemperature,
            profile.ambientTemperature,
            profile.returnRate * Time.deltaTime);

        EvaluateState(false);
    }

    public void AddTemperature(float amount)
    {
        if (profile == null) return;

        currentTemperature += amount;
        currentTemperature = Mathf.Clamp(
            currentTemperature,
            profile.minTemperature,
            profile.maxTemperature);

        EvaluateState(false);
    }

    private void EvaluateState(bool force)
    {
        if (profile == null) return;

        ThermalState next = ThermalState.Normal;

        if (currentTemperature <= profile.freezeTemperature)
            next = profile.isMetal ? ThermalState.Contracted : ThermalState.Frozen;
        else if (currentTemperature >= profile.heatTemperature)
            next = profile.isMetal ? ThermalState.Expanded : ThermalState.Heated;

        if (!force && next == currentState) return;

        currentState = next;
        ApplyView();
        onStateChanged?.Invoke(currentState);
    }

    private void ApplyView()
    {
        if (view == null) return;

        if (profile != null && profile.isMetal)
        {
            if (currentState == ThermalState.Contracted)
            {
                view.color = new Color(0.55f, 0.78f, 1f);
                if (contractedSprite != null) view.sprite = contractedSprite;
            }
            else if (currentState == ThermalState.Expanded)
            {
                view.color = new Color(1f, 0.45f, 0.3f);
                if (expandedSprite != null) view.sprite = expandedSprite;
            }
            else
            {
                view.color = Color.white;
                if (normalSprite != null) view.sprite = normalSprite;
            }
            return;
        }

        view.color = Color.white;
        if (currentState == ThermalState.Frozen && iceSprite != null)
            view.sprite = iceSprite;
        else if (waterSprite != null)
            view.sprite = waterSprite;
    }

    public string GetDisplayName()
    {
        if (currentState == ThermalState.Frozen) return "Ice";
        if (currentState == ThermalState.Contracted) return "Compressed";
        if (currentState == ThermalState.Expanded) return "Expanded";
        if (currentState == ThermalState.Heated) return "Hot";
        return profile != null && profile.isMetal ? "Metal" : "Water";
    }
}
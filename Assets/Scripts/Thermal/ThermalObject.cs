using UnityEngine;
using UnityEngine.Events;

public class ThermalObject : MonoBehaviour
{
    [SerializeField] private ThermalProfileSO profile;
    [SerializeField] private SpriteRenderer view;
    [SerializeField] private Collider2D solidCollider;

    [SerializeField] private float currentTemperature;
    [SerializeField] private ThermalState currentState;

    public UnityEvent<ThermalState> onStateChanged;

    public float Temperature => currentTemperature;
    public ThermalState State => currentState;

    public string GetDisplayName()
    {
        return currentState switch
        {
            ThermalState.Frozen => "Ice",
            ThermalState.Solid => "Ice",
            ThermalState.Liquid => "Water",
            ThermalState.Vapor => "Vapor",
            ThermalState.Expanded => "Expanded Metal",
            ThermalState.Contracted => "Contracted Metal",
            _ => name
        };
    }

    private void Awake()
    {
        if (profile == null) return;

        currentTemperature = profile.startTemperature;
        currentState = EvaluateState(currentTemperature);
        ApplyState(currentState);
    }

    private void Update()
    {
        if (profile == null || profile.returnRate <= 0f) return;

        float target = profile.startTemperature;
        if (Mathf.Approximately(currentTemperature, target)) return;

        float step = profile.returnRate * Time.deltaTime;
        float next = Mathf.MoveTowards(currentTemperature, target, step);
        AddTemperature(next - currentTemperature);
    }

    public void AddTemperature(float delta)
    {
        if (profile == null) return;

        currentTemperature = Mathf.Clamp(
            currentTemperature + delta,
            profile.minTemperature,
            profile.maxTemperature
        );

        ThermalState next = EvaluateState(currentTemperature);
        if (next == currentState) return;

        currentState = next;
        ApplyState(currentState);
        onStateChanged?.Invoke(currentState);
    }

    private ThermalState EvaluateState(float temp)
    {
        ThermalState state = profile.thresholds != null && profile.thresholds.Length > 0
            ? profile.thresholds[0].state
            : ThermalState.Solid;

        for (int i = 0; i < profile.thresholds.Length; i++)
        {
            if (temp >= profile.thresholds[i].celsius)
                state = profile.thresholds[i].state;
        }

        return state;
    }

    private void ApplyState(ThermalState state)
    {
        bool walkable = state == ThermalState.Frozen
                        || state == ThermalState.Solid
                        || state == ThermalState.Expanded
                        || state == ThermalState.Contracted;

        if (solidCollider != null)
        {
            solidCollider.enabled = true;
            solidCollider.isTrigger = !walkable;
        }

        if (view == null) return;

        switch (state)
        {
            case ThermalState.Frozen:
            case ThermalState.Solid:
                view.color = new Color(0.6f, 0.85f, 1f, 1f);
                break;

            case ThermalState.Liquid:
                view.color = new Color(0.2f, 0.45f, 0.95f, 0.45f);
                break;

            case ThermalState.Vapor:
                view.color = new Color(1f, 1f, 1f, 0.15f);
                break;

            default:
                view.color = Color.white;
                break;
        }
    }
}
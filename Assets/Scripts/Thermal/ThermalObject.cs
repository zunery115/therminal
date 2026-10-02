using UnityEngine;
using UnityEngine.Events;

public class ThermalObject : MonoBehaviour
{
    [SerializeField] private ThermalProfileSO profile;
    [SerializeField] private SpriteRenderer view;
    [SerializeField] private Collider2D solidCollider;

    [Header("Sprites")]
    [SerializeField] private Sprite iceSprite;
    [SerializeField] private Sprite waterSprite;

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
            ThermalState.Solid => "Metal",
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
        onStateChanged?.Invoke(currentState);
    }

    private void Update()
    {
        SyncCollider();

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
        SyncCollider();

        if (view == null) return;

        view.color = Color.white;

        switch (state)
        {
            case ThermalState.Frozen:
                if (iceSprite != null)
                    view.sprite = iceSprite;
                break;

            case ThermalState.Liquid:
                if (waterSprite != null)
                    view.sprite = waterSprite;
                break;
        }
    }

    private void SyncCollider()
    {
        if (solidCollider == null) return;

        bool walkable = currentState == ThermalState.Frozen
                        || currentState == ThermalState.Solid
                        || currentState == ThermalState.Expanded
                        || currentState == ThermalState.Contracted;

        solidCollider.enabled = true;
        solidCollider.isTrigger = !walkable;
    }
}
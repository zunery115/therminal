using UnityEngine;

public enum ThermalState
{
    Frozen,
    Solid,
    Liquid,
    Vapor,
    Expanded,
    Contracted
}

[System.Serializable]
public struct ThermalThreshold
{
    public float celsius;
    public ThermalState state;
}

[CreateAssetMenu(fileName = "ThermalProfile", menuName = "Termia/Thermal Profile")]
public class ThermalProfileSO : ScriptableObject
{
    public float startTemperature = -10f;
    public float minTemperature = -50f;
    public float maxTemperature = 120f;

    [Tooltip("Grados por segundo hacia la temperatura inicial")]
    public float returnRate = 8f;

    [Tooltip("De menor a mayor temperatura")]
    public ThermalThreshold[] thresholds;
}
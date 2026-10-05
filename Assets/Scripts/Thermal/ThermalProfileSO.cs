using UnityEngine;

[CreateAssetMenu(fileName = "ThermalProfile", menuName = "Termia/Thermal Profile")]
public class ThermalProfileSO : ScriptableObject
{
    public float minTemperature = -20f;
    public float maxTemperature = 80f;
    public float ambientTemperature = 20f;
    public float returnRate = 0f;
    public float freezeTemperature = 0f;
    public float heatTemperature = 40f;
    public bool isMetal;
}
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProperties", menuName = "Termia/Player Properties")]
public class PlayerSO : ScriptableObject
{
    [Header("Movement")]
    public float speed = 5f;
    public float speedMultiplier = 1f;

    [Header("Thermal Tool")]
    public float heatRate = 25f;
    public float coolRate = 25f;
    public float toolRange = 1.4f;
    public float toolRadius = 0.25f;
    public LayerMask thermalMask;
}
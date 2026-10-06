using UnityEngine;
using UnityEngine.UIElements;

public class ThermometerUI : MonoBehaviour
{
    [SerializeField] private UIDocument document;

    private VisualElement card;
    private Label nameLabel;
    private Label tempLabel;
    private Label stateLabel;

    private void Awake()
    {
        // Si no lo asignaste, usa el UIDocument de este mismo objeto.
        if (document == null) document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        if (document == null) return;

        // Estos nombres tienen que coincidir con HUD.uxml.
        VisualElement root = document.rootVisualElement;
        card = root.Q<VisualElement>("thermo-card");
        nameLabel = root.Q<Label>("thermo-name");
        tempLabel = root.Q<Label>("thermo-temp");
        stateLabel = root.Q<Label>("thermo-state");

        Hide();
    }

    public void SetTarget(ThermalObject target)
    {
        if (card == null) return;

        // Sin objeto enfrente, el panel se esconde.
        if (target == null)
        {
            Hide();
            return;
        }

        card.RemoveFromClassList("thermo-hidden");

        // cold y hot cambian el color de los grados en HUD.uss.
        card.EnableInClassList("cold", target.Temperature < 0f);
        card.EnableInClassList("hot", target.Temperature > 40f);

        if (nameLabel != null) nameLabel.text = target.GetDisplayName();
        if (tempLabel != null) tempLabel.text = Mathf.RoundToInt(target.Temperature) + " °C";
        if (stateLabel != null) stateLabel.text = target.State.ToString();
    }

    private void Hide()
    {
        if (card == null) return;
        card.AddToClassList("thermo-hidden");
    }
}
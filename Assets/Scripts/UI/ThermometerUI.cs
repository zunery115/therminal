using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class ThermometerUI : MonoBehaviour
{
    private VisualElement card;
    private Label nameLabel;
    private Label tempLabel;
    private Label stateLabel;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        card = root.Q<VisualElement>("thermo-card");
        nameLabel = root.Q<Label>("thermo-name");
        tempLabel = root.Q<Label>("thermo-temp");
        stateLabel = root.Q<Label>("thermo-state");
        Hide();
    }

    public void Show(ThermalObject target)
    {
        if (card == null || target == null)
        {
            Hide();
            return;
        }

        card.RemoveFromClassList("thermo-hidden");
        card.RemoveFromClassList("hot");
        card.RemoveFromClassList("cold");

        if (target.Temperature >= 20f) card.AddToClassList("hot");
        else if (target.Temperature <= 0f) card.AddToClassList("cold");

        nameLabel.text = target.GetDisplayName();
        tempLabel.text = $"{target.Temperature:0} °C";
        stateLabel.text = target.State.ToString();
    }

    public void Hide()
    {
        card?.AddToClassList("thermo-hidden");
    }
}
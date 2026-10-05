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
        if (document == null) document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        if (document == null) return;

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

        if (target == null)
        {
            Hide();
            return;
        }

        card.RemoveFromClassList("thermo-hidden");
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
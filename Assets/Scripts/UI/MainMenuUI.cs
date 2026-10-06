using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private string gameScene = "01_level";

    private VisualElement mainPanel;
    private VisualElement controlsPanel;
    private VisualElement creditsPanel;

    private void Awake()
    {
        if (document == null) document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = document.rootVisualElement;

        mainPanel = root.Q<VisualElement>("main-panel");
        controlsPanel = root.Q<VisualElement>("controls-panel");
        creditsPanel = root.Q<VisualElement>("credits-panel");

        // Cada boton del UXML llama a su metodo.
        root.Q<Button>("play-button").clicked += Play;
        root.Q<Button>("controls-button").clicked += ShowControls;
        root.Q<Button>("credits-button").clicked += ShowCredits;
        root.Q<Button>("quit-button").clicked += Quit;
        root.Q<Button>("controls-back").clicked += ShowMain;
        root.Q<Button>("credits-back").clicked += ShowMain;

        ShowMain();
    }

    private void ShowMain()
    {
        mainPanel.RemoveFromClassList("panel-hidden");
        controlsPanel.AddToClassList("panel-hidden");
        creditsPanel.AddToClassList("panel-hidden");
    }

    private void ShowControls()
    {
        mainPanel.AddToClassList("panel-hidden");
        controlsPanel.RemoveFromClassList("panel-hidden");
        creditsPanel.AddToClassList("panel-hidden");
    }

    private void ShowCredits()
    {
        mainPanel.AddToClassList("panel-hidden");
        controlsPanel.AddToClassList("panel-hidden");
        creditsPanel.RemoveFromClassList("panel-hidden");
    }

    private void Play()
    {
        SceneManager.LoadScene(gameScene);
    }

    private void Quit()
    {
        // En el editor detiene Play. En el build cierra el juego.
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
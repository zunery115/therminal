using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private string levelScene = "01_sandbox";

    private VisualElement credits;

    private void OnEnable()
    {
        if (document == null)
            document = GetComponent<UIDocument>();

        VisualElement root = document.rootVisualElement;

        root.Q<Button>("play").clicked += Play;
        root.Q<Button>("credits").clicked += ShowCredits;
        root.Q<Button>("quit").clicked += Quit;
        root.Q<Button>("back").clicked += HideCredits;

        credits = root.Q<VisualElement>("credits-panel");
    }

    private void Play()
    {
        SceneManager.LoadScene(levelScene);
    }

    private void ShowCredits()
    {
        credits.RemoveFromClassList("hidden");
    }

    private void HideCredits()
    {
        credits.AddToClassList("hidden");
    }

    private void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
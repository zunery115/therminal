using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private string menuScene = "MainMenu";

    private VisualElement root;
    private bool paused;

    private void OnEnable()
    {
        if (document == null)
            document = GetComponent<UIDocument>();

        root = document.rootVisualElement.Q<VisualElement>("root");
        root.Q<Button>("resume").clicked += Resume;
        root.Q<Button>("menu").clicked += GoToMenu;

        root.AddToClassList("hidden");
        paused = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (Keyboard.current == null) return;
        if (!Keyboard.current.uKey.wasPressedThisFrame) return;

        if (paused)
            Resume();
        else
            Pause();
    }

    private void Pause()
    {
        paused = true;
        Time.timeScale = 0f;
        root.RemoveFromClassList("hidden");
    }

    private void Resume()
    {
        paused = false;
        Time.timeScale = 1f;
        root.AddToClassList("hidden");
    }

    private void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuScene);
    }
}
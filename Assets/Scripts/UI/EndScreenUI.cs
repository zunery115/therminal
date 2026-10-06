using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] private UIDocument document;
    [SerializeField] private string menuScene = "MainMenu";

    private void Awake()
    {
        if (document == null) document = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = document.rootVisualElement;
        root.Q<Button>("menu-button").clicked += GoMenu;
        root.Q<Button>("quit-button").clicked += Quit;
    }

    private void GoMenu()
    {
        SceneManager.LoadScene(menuScene);
    }

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
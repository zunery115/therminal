using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    // En 01_level pon 02_level. En 02_level pon End.
    [SerializeField] private string nextScene = "02_level";

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Solo el jugador, y el collider de esta salida tiene que ser trigger.
        if (!other.CompareTag("Player")) return;
        SceneManager.LoadScene(nextScene);
    }
}
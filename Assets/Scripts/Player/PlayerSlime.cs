using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSlime : MonoBehaviour
{
    [SerializeField] private SpriteRenderer view;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Sprite[] greenFrames;
    [SerializeField] private Sprite[] redFrames;
    [SerializeField] private Sprite[] blueFrames;
    [SerializeField] private float frameRate = 8f;

    private Sprite[] current;
    private float timer;
    private int frame;

    private void Awake()
    {
        if (view == null)
            view = GetComponent<SpriteRenderer>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        current = greenFrames;
    }

    private void Update()
    {
        Sprite[] next = greenFrames;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.eKey.isPressed && redFrames.Length > 0)
                next = redFrames;
            else if (Keyboard.current.qKey.isPressed && blueFrames.Length > 0)
                next = blueFrames;
        }

        if (next != current)
        {
            current = next;
            frame = 0;
            timer = 0f;
        }

        if (current == null || current.Length == 0 || view == null)
            return;

        bool moving = rb != null && rb.linearVelocity.sqrMagnitude > 0.05f;
        if (!moving)
        {
            frame = 0;
            view.sprite = current[0];
            return;
        }

        timer += Time.deltaTime;
        if (timer >= 1f / frameRate)
        {
            timer = 0f;
            frame = (frame + 1) % current.Length;
        }

        view.sprite = current[frame];
    }
}
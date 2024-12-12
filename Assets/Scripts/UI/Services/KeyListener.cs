using UnityEngine;
using UnityEngine.Events;

public class KeyListener : MonoBehaviour
{
    [Header("Key Settings")]
    [Tooltip("Клавиша, которую нужно отслеживать.")]
    public KeyCode keyToListen = KeyCode.Space;

    [Header("Action Settings")]
    [Tooltip("Действие, выполняемое при нажатии клавиши.")]
    public UnityEvent onKeyPressed;

    private void Update()
    {
        // Проверяем, нажата ли клавиша
        if (Input.GetKeyDown(keyToListen))
        {
            onKeyPressed?.Invoke();
        }
    }
}

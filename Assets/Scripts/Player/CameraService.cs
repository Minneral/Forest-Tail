using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraService : MonoBehaviour
{
    public CinemachineFreeLook freeLook; // Предполагаем, что вы установите это через инспектор
    public KeyCode[] LockKeyCodes; // Массив клавиш для блокировки
    public bool IsLocked { get; private set; }
    private bool previousState;
    public static CameraService Instance;

    private float default_axis_X;
    private float default_axis_Y;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject); // Уничтожаем объект
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Инициализируем состояние камеры
        previousState = IsLocked;
        default_axis_X = freeLook.m_XAxis.m_MaxSpeed;
        default_axis_Y = freeLook.m_YAxis.m_MaxSpeed;

        UpdateCameraLockState();
    }

    private void Update()
    {
        bool shouldLock = InventoryUI.Instance.isActive || DialogueManager.Instance.dialogueBox.activeSelf;
        if (shouldLock != previousState)
        {
            IsLocked = shouldLock; // Устанавливаем состояние блокировки
            UpdateCameraLockState();
            previousState = shouldLock;
        }
    }

    private void UpdateCameraLockState()
    {
        if (IsLocked)
        {
            Lock();
        }
        else
        {
            UnLock();
        }
    }

    public void Lock()
    {
        freeLook.m_YAxis.m_MaxSpeed = 0;
        freeLook.m_XAxis.m_MaxSpeed = 0;
    }

    public void UnLock()
    {
        freeLook.m_YAxis.m_MaxSpeed = default_axis_Y;
        freeLook.m_XAxis.m_MaxSpeed = default_axis_X;
    }
}

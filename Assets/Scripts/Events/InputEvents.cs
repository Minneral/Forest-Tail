using UnityEngine;
using System;
using System.Security.Cryptography;

public class InputEvents
{
    public event Action<Vector2> onMovePressed;
    public void MovePressed(Vector2 moveDir)
    {
        if (onMovePressed != null)
        {
            onMovePressed(moveDir);
        }
    }

    public event Action onJumpPressed;
    public void JumpPressed()
    {
        if (onJumpPressed != null)
        {
            if (GameEventsManager.instance.IsAnyUIVisible())
                return;

            onJumpPressed();
        }
    }

    public event Action onInteractPressed;
    public void InteractPressed()
    {
        if (onInteractPressed != null)
        {
            if (GameEventsManager.instance.IsAnyUIVisible())
                return;

            onInteractPressed();
        }
    }

    public event Action onSubmitPressed;
    public void SubmitPressed()
    {
        if (Memories.instance.isActive)
            return;

        if (!GameEventsManager.instance.IsAnyUIVisible())
            return;

        if (onSubmitPressed != null)
        {
            onSubmitPressed();
        }
    }

    public event Action onClosePressed;
    public void ClosePressed()
    {
        if (onClosePressed != null)
        {
            onClosePressed();
        }
    }

    public event Action onInventoryPressed;
    public void InventoryPressed()
    {
        if (onInventoryPressed != null)
        {
            onInventoryPressed();
        }
    }

    public event Action onAttackPressed;
    public void AttackPressed()
    {
        if (onAttackPressed != null)
        {
            if (GameEventsManager.instance.IsAnyUIVisible())
                return;

            onAttackPressed();
        }
    }

    public event Action onDodgePressed;
    public void DodgePressed()
    {
        if (onDodgePressed != null)
        {
            if (GameEventsManager.instance.IsAnyUIVisible())
                return;

            onDodgePressed();
        }
    }

    public event Action onQuestMenuPressed;
    public void QuestMenuPressed()
    {
        if (onQuestMenuPressed != null)
        {
            onQuestMenuPressed();
        }
    }
}
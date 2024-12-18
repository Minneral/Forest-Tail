using UnityEngine;
using System;

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
            onJumpPressed();
        }
    }

    public event Action onInteractPressed;
    public void InteractPressed()
    {
        if (onInteractPressed != null)
        {
            onInteractPressed();
        }
    }

    public event Action onSubmitPressed;
    public void SubmitPressed()
    {
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
            onAttackPressed();
        }
    }

    public event Action onDodgePressed;
    public void DodgePressed()
    {
        if (onDodgePressed != null)
        {
            onDodgePressed();
        }
    }
}
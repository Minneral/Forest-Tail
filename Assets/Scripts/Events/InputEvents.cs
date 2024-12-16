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

    public event Action onQuestLogTogglePressed;
    public void QuestLogTogglePressed()
    {
        if (onQuestLogTogglePressed != null)
        {
            onQuestLogTogglePressed();
        }
    }
}
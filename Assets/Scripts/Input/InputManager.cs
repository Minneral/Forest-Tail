using UnityEngine;
using UnityEngine.InputSystem;

// This script acts as a proxy for the PlayerInput component
// such that the input events the game needs to proces will 
// be sent through the GameEventManager. This lets any other
// script in the project easily subscribe to an input action
// without having to deal with the PlayerInput component directly.

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed || context.canceled)
        {
            GameEventsManager.instance.inputEvents.MovePressed(context.ReadValue<Vector2>());
        }
    }

    public void JumpPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.instance.inputEvents.JumpPressed();
        }
    }

    public void InteractPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.instance.inputEvents.InteractPressed();
        }
    }

    public void SubmitPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.instance.inputEvents.SubmitPressed();
        }
    }

    public void InventoryPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.instance.inputEvents.InventoryPressed();
        }
    }

    public void ClosePressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.instance.inputEvents.ClosePressed();
        }
    }

    public void AttackPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.instance.inputEvents.AttackPressed();
        }
    }

    public void DodgePressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.instance.inputEvents.DodgePressed();
        }
    }

    public void QuestMenuPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventsManager.instance.inputEvents.QuestMenuPressed();
        }
    }
}
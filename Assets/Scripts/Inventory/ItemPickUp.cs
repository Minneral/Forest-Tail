using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour, IInteractable
{
    public Item item;  // Предмет, с которым взаимодействует игрок
    private Inventory _inventory;

    public Transform GetTransform()
    {
        return transform;
    }

    public void Interact()
    {
        PickUp();
    }

    public void OnDefocused()
    {
        HintManager.instance.HideHint();
    }

    public void OnFocused()
    {
        HintManager.instance.ShowHint("Нажмите E чтобы подобрать " + item.itemName);
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player == null)
                throw new MissingComponentException(nameof(Inventory), gameObject.name, GetType().Name, "You need to assign tag 'Player'");

            _inventory = player.GetComponent<Inventory>();

            if (_inventory == null)
                throw new MissingComponentException(nameof(Inventory), gameObject.name, GetType().Name, "You need to append 'Inventory' script to player");

        }
        catch (MissingComponentException ex)
        {
            Debug.LogError(ex.Message);
            enabled = false;
            return;
        }

    }

    void PickUp()
    {
        if(item.ItemId == "Mushroom")
            GameEventsManager.instance.miscEvents.MushroomCollected();
            
        if (_inventory.AddItem(item))
        {
            Destroy(gameObject);
        }
    }
}

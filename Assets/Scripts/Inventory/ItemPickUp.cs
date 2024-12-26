using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour, IInteractable
{
    string itemPickUpId;
    public Item item;  // Предмет, с которым взаимодействует игрок
    public AudioClip pickupClip;
    private Inventory _inventory;

    private void Awake()
    {
        itemPickUpId = name + item.type + item.itemName;

        if (GameManager.instance.NotRestoringCollectedItem.Contains(itemPickUpId))
            Destroy(gameObject);
    }

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
        MasterVolume.instance.audioSource.PlayOneShot(pickupClip);
        if (item.ItemId == "Mushroom")
        {
            GameEventsManager.instance.miscEvents.MushroomCollected();
            GameManager.instance.ItemCollected(itemPickUpId);
        }

        if (_inventory.AddItem(item))
        {
            Destroy(gameObject);
        }
    }
}

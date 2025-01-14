using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int rows = 4;
    public int columns = 7;
    public float MaxWeight = 100;
    public bool IsOverLoaded = false;
    private int totalSlots;
    public List<InventorySlot> slots;
    public static Inventory instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        totalSlots = rows * columns;
        slots = new List<InventorySlot>();

        for (int i = 0; i < totalSlots; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public bool AddItem(Item newItem)
    {
        foreach (var slot in slots)
        {
            if (slot.item != null && slot.item.ItemId == newItem.ItemId)
            {
                if (newItem.stackAmount > 1 && slot.amount < newItem.stackAmount)
                {
                    slot.amount++;
                    return true;
                }
            }
        }

        foreach (var slot in slots)
        {
            if (slot.item == null)
            {
                slot.item = newItem;
                slot.amount = 1;
                return true;
            }
        }

        return false; // Inventory is full
    }

    public bool RemoveItem(Item itemToRemove)
    {
        foreach (var slot in slots)
        {
            if (slot.item != null && slot.item.type == itemToRemove.type && slot.item.name == itemToRemove.name)
            {
                slot.amount--;
                if (slot.amount <= 0)
                {
                    slot.item = null;
                    slot.amount = 0;
                }
                return true;
            }
        }
        return false; // Item not found
    }

    public bool RemoveItem(string itemId)
    {
        foreach (var slot in slots)
        {
            if (slot.item != null && slot.item.ItemId == itemId)
            {
                slot.amount--;
                if (slot.amount <= 0)
                {
                    slot.item = null;
                    slot.amount = 0;
                }
                return true;
            }
        }
        return false; // Item not found
    }

    public void DisplayInventory()
    {
        foreach (var slot in slots)
        {
            if (slot.item != null)
            {
                Debug.Log($"Item: {slot.item.itemName}, Amount: {slot.amount}, Type: {slot.item.type}");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DisplayInventory();
        }

        IsOverLoaded = GetWeight() > MaxWeight;
    }

    public float GetWeight()
    {
        return slots.Sum(t => t.amount * (t.item == null ? 0 : t.item.itemWeight));
    }

    public bool SwapItems(InventorySlot slot1, InventorySlot slot2)
    {
        // Check if both slots are not null
        if (slot1 == null || slot2 == null)
        {
            Debug.LogError("One or both of the slots are null."); return false;
        }
        // Swap the items
        Item tempItem = slot1.item;
        int tempAmount = slot1.amount;

        slot1.item = slot2.item;
        slot1.amount = slot2.amount;

        slot2.item = tempItem;
        slot2.amount = tempAmount;
        return true;
    }

    public bool SwapItems(int id1, int id2)
    {
        InventorySlot slot1 = GetSlotById(id1);
        InventorySlot slot2 = GetSlotById(id2);

        return SwapItems(slot1, slot2);
    }

    private InventorySlot GetSlotById(int id)
    {
        if (id < 0 || id >= slots.Count)
        {
            return null;
        }
        return slots[id];
    }

    public List<InventorySlot> GetSlotsByItemId(string itemId)
    {
        return slots.Where(item => item.item != null && item.item.ItemId.Equals(itemId)).ToList();
    }
}

public class InventorySlot
{
    public Item item;
    public int amount;

    public InventorySlot()
    {
        item = null;
        amount = 0;
    }
}

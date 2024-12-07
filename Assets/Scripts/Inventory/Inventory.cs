using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int rows = 3;
    public int columns = 7;
    public float MaxWeight = 100;
    public bool IsOverLoaded = false;
    private int totalSlots;

    public List<InventorySlot> slots;

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
            if (slot.item != null && slot.item.type == newItem.type && slot.item.name == newItem.name)
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int row = 3;
    public int column = 9;
    private int totalSlots;

    public List<InventorySlot> slots;

    // Метод для добавления предметов в инвентарь
    public bool AddItem(Item newItem)
    {
        // Проверяем, можно ли застакать предмет с существующими
        foreach (var slot in slots)
        {
            if (slot.item != null && slot.item.type == newItem.type && slot.item.name == newItem.name)
            {
                // Проверка, если предмет стакается
                if (newItem.stackAmount > 1) // Предметы с возможностью стака добавляем
                {
                    int currentAmount = slot.amount;
                    int stackSize = newItem.stackAmount;

                    if (currentAmount < stackSize)
                    {
                        // Застакаем предметы, увеличив количество в текущем слоте
                        slot.amount += 1;
                        return true;
                    }
                    else
                    {
                        // Слот полностью заполнен, предмет не вмещается
                        continue;
                    }
                }
            }
        }

        // Если предмет не удалось застакать, находим пустой слот
        foreach (var slot in slots)
        {
            if (slot.item == null)
            {
                slot.item = newItem;
                slot.amount = 1; // Первоначально добавляем 1 предмет

                return true;
            }
        }

        // Если инвентарь полный
        return false;
    }

    // Метод для вывода всех предметов в инвентаре
    public void DisplayInventory()
    {
        foreach (var slot in slots)
        {
            if (slot.item != null)
            {
                if (slot.item.type == ItemType.Food)
                {
                    Debug.Log($"Item: {slot.item.itemName}, Amount: {slot.amount}, Type: {slot.item.type}, Nutrition: {slot.item.nutritionValue}");
                }

                if (slot.item.type == ItemType.Resource)
                {
                    Debug.Log($"Item: {slot.item.itemName}, Amount: {slot.amount}, Type: {slot.item.type}");
                }
            }
        }
    }

    private void Start()
    {
        totalSlots = row * column;
        slots = new List<InventorySlot>();

        for (int i = 0; i < totalSlots; i++)
        {
            slots.Add(new InventorySlot());
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            DisplayInventory();
        }
    }
}

public class InventorySlot
{
    public Item item;
    public int amount;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 0)]
public class Item : ScriptableObject
{
    public string itemName = "Item";
    public string itemDescription = "Description";
    public float itemWeight = 1;
    public Sprite icon = null;

    public virtual int stackAmount
    {
        get { return 1; } // По умолчанию предметы не стакаются
    }
}


// Класс для ресурса
[CreateAssetMenu(fileName = "ResourceItem", menuName = "Inventory/Resource Item", order = 1)]
public class ResourceItem : Item
{

    public override int stackAmount
    {
        get { return 999; } // Ресурсы можно стакавать до 999
    }
}

// Класс для еды
[CreateAssetMenu(fileName = "FoodItem", menuName = "Inventory/Food Item", order = 2)]
public class FoodItem : Item
{
    public int nutritionValue; // Восстанавливаемое здоровье или энергия

    public override int stackAmount
    {
        get { return 10; } // Еда стакается до 10
    }
}

// Класс для оружия
[CreateAssetMenu(fileName = "WeaponItem", menuName = "Inventory/Weapon Item", order = 3)]
public class WeaponItem : Item
{
    public int damage; // Урон
    public float attackSpeed; // Скорость атаки

    public override int stackAmount
    {
        get { return 1; } // Оружие не стакается
    }
}

// Класс для брони
[CreateAssetMenu(fileName = "ArmorItem", menuName = "Inventory/Armor Item", order = 4)]
public class ArmorItem : Item
{
    public int defenseValue; // Защита

    public override int stackAmount
    {
        get { return 1; } // Броня не стакается
    }
}

// Класс для прочих предметов
[CreateAssetMenu(fileName = "MiscItem", menuName = "Inventory/Misc Item", order = 5)]
public class MiscItem : Item
{
    public string miscPurpose; // Назначение предмета (например, квестовый или декор)

    public override int stackAmount
    {
        get { return 50; } // Прочие предметы стакаются до 50
    }
}

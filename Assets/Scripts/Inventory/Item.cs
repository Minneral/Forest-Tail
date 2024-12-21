using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Inventory/Item", order = 0)]
public class Item : ScriptableObject
{
    [SerializeField] private string itemId;
    public string ItemId => itemId;

    [Header("General Settings")]
    public ItemType type;
    public string itemName = "Item";
    public string itemDescription = "Description";
    public float itemWeight = 1;
    public Sprite icon = null;
    public int stackAmount;

    [Header("Specified Settings")]
    public int nutritionValue; // Восстанавливаемое здоровье или энергия
    public int damage; // Урон
    public float attackSpeed; // Скорость атаки
    public int defenseValue; // Защита
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!string.IsNullOrEmpty(this.name))
        {
            itemId = this.name;
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }

}

public enum ItemType
{
    Resource, Food, Weapon, Armor, Misc
}

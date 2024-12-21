#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Item assign = (Item)target;

        // General Settings Header
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("General Settings", EditorStyles.boldLabel);

        // General Settings Fields
        assign.itemName = EditorGUILayout.TextField("Id", assign.ItemId);
        assign.itemName = EditorGUILayout.TextField("Name", assign.itemName);
        assign.itemDescription = EditorGUILayout.TextField("Description", assign.itemDescription);
        assign.itemWeight = EditorGUILayout.FloatField("Weight", assign.itemWeight);
        assign.icon = (Sprite)EditorGUILayout.ObjectField("Icon", assign.icon, typeof(Sprite), false);

        assign.stackAmount = Mathf.Max(1, EditorGUILayout.IntField("Stack Amount", assign.stackAmount));  // Prevent negative or zero stack amounts


        EditorGUILayout.Space();

        // Specified Settings Header
        EditorGUILayout.LabelField("Specified Settings", EditorStyles.boldLabel);

        // Add the enum dropdown
        assign.type = (ItemType)EditorGUILayout.EnumPopup("Select Type", assign.type);

        // Conditionally show fields based on the selected type
        if (assign.type == ItemType.Food)
        {
            // Show nutritionValue field for food
            assign.nutritionValue = EditorGUILayout.IntField("Nutrition", assign.nutritionValue);
        }
        else if (assign.type == ItemType.Weapon)
        {
            // Show damage and attackSpeed fields for weapon
            assign.damage = EditorGUILayout.IntField("Damage", assign.damage);
            assign.attackSpeed = EditorGUILayout.FloatField("Attack Speed", assign.attackSpeed);
        }
        else if (assign.type == ItemType.Armor)
        {
            // Show defenseValue field for armor
            assign.defenseValue = EditorGUILayout.IntField("Defense", assign.defenseValue);
        }

        // Mark the object as dirty if any change was made
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}
#endif

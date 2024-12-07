using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image BackgroundImage;
    private Image _itemIcon;
    private TextMeshProUGUI _itemAmount;

    void Start()
    {
        try
        {
            _itemIcon = transform.Find("SlotItemIcon")?.GetComponent<Image>();
            if (_itemIcon == null)
                throw new MissingComponentException($"Slot '{gameObject.name}' is missing a child Image component named 'SlotItemIcon'.");

            _itemAmount = transform.Find("SlotItemAmount")?.GetComponent<TextMeshProUGUI>();
            if (_itemAmount == null)
                throw new MissingComponentException($"Slot '{gameObject.name}' is missing a child TextMeshProUGUI component named 'SlotItemAmount'.");

            _itemIcon.enabled = false;
            _itemAmount.enabled = false;
        }
        catch (MissingComponentException ex)
        {
            Debug.LogError(ex.Message);
            enabled = false;
            return;
        }
    }

    public void AssignItem(InventorySlot slot)
    {
        if (slot == null || slot.item == null)
        {
            _itemIcon.sprite = null;
            _itemIcon.enabled = false;
            _itemAmount.text = string.Empty;
            _itemAmount.enabled = false;
            return;
        }

        _itemIcon.sprite = slot.item.icon;
        _itemIcon.enabled = slot.item.icon != null;

        _itemAmount.text = slot.amount > 1 ? slot.amount.ToString() : string.Empty;
        _itemAmount.enabled = slot.amount > 1;
    }
}

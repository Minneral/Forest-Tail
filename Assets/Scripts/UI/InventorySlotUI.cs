using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IDropHandler
{
    public Image BackgroundImage;
    private GameObject _player;

    private Inventory _inventory;

    private Transform _item;
    private Image _itemIcon;
    private TextMeshProUGUI _itemAmount;
    public int id;

    void Start()
    {
        try
        {
            _item = transform.Find("item");

            if (_item == null)
                throw new MissingComponentException($"Slot '{gameObject.name}' is missing a child wrapper component named 'item'.");

            _itemIcon = _item.Find("SlotItemIcon")?.GetComponent<Image>();
            if (_itemIcon == null)
                throw new MissingComponentException($"Slot '{gameObject.name}' is missing a child Image component named 'SlotItemIcon'.");

            _itemAmount = _item.Find("SlotItemAmount")?.GetComponent<TextMeshProUGUI>();
            if (_itemAmount == null)
                throw new MissingComponentException($"Slot '{gameObject.name}' is missing a child TextMeshProUGUI component named 'SlotItemAmount'.");

            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null)
                throw new MissingComponentException(nameof(GameObject), gameObject.name, GetType().Name, $"Tag 'Player' is missing. Assign the 'Player' tag to the player GameObject.");

            _inventory = _player.GetComponent<Inventory>();
            if (_inventory == null)
                throw new MissingComponentException(nameof(Inventory), gameObject.name, GetType().Name);

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

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        DraggableUI draggableUI = dropped.GetComponent<DraggableUI>();
        InventorySlotUI droppedSlotUI = draggableUI.parentTransform.GetComponent<InventorySlotUI>();

        if (droppedSlotUI == null)
        {
            Debug.LogError("Dropped slot UI is null.");
            return;
        }

        Debug.Log("Current slot: " + this.id);
        Debug.Log("Dropped slot: " + droppedSlotUI.id);

        _inventory.SwapItems(this.id, droppedSlotUI.id);
    }

}

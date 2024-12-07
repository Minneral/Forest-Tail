using System.Linq;
using UnityEngine;

public class ActiveSlotsBarUI : MonoBehaviour
{
    private GameObject _player;
    private Inventory _inventory;
    private InventorySlotUI[] _slotsUI;
    public int ActiveSlotIndex = 0;

    private readonly Vector3 _normalScale = Vector3.one;
    private readonly Vector3 _activeScale = Vector3.one * 1.2f;

    void Start()
    {
        try
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null)
                throw new MissingComponentException($"Tag 'Player' is missing. Assign the 'Player' tag to the player GameObject.");

            _inventory = _player.GetComponent<Inventory>();
            if (_inventory == null)
                throw new MissingComponentException($"Inventory component is missing on GameObject '{_player.name}'.");

            _slotsUI = GetComponentsInChildren<InventorySlotUI>();
            if (_slotsUI.Length == 0)
                throw new MissingComponentException($"No active inventory slots found in children of '{gameObject.name}'.");

            UpdateSlotScales();
        }
        catch (MissingComponentException ex)
        {
            Debug.LogError(ex.Message);
            enabled = false;
        }
    }

    void Update()
    {
        UpdateUI();
        HandleSlotSelection();
    }

    private void HandleSlotSelection()
    {
        for (int i = 0; i < _slotsUI.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                if (ActiveSlotIndex == i + 1)
                {
                    ActiveSlotIndex = 0; // Reset if the same slot is selected twice
                }
                else
                {
                    ActiveSlotIndex = i + 1;
                }
                UpdateSlotScales();
                break;
            }
        }
    }

    private void UpdateUI()
    {
        if (_inventory == null || _slotsUI == null) return;

        var slots = _inventory.slots.Take(_slotsUI.Length).ToArray();

        for (int i = 0; i < slots.Length; i++)
        {
            _slotsUI[i].AssignItem(slots[i]);
        }

        for (int i = slots.Length; i < _slotsUI.Length; i++)
        {
            _slotsUI[i].AssignItem(null);
        }
    }

    private void UpdateSlotScales()
    {
        for (int i = 0; i < _slotsUI.Length; i++)
        {
            _slotsUI[i].transform.localScale = (i == ActiveSlotIndex - 1) ? _activeScale : _normalScale;
        }
    }
}

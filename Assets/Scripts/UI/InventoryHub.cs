using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryHub : MonoBehaviour
{
    public GameObject inventoryItems;
    private GameObject _player;
    private Inventory _inventory;
    private InventorySlotUI[] _slotsUI;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            if (_player == null)
                throw new MissingComponentException(nameof(GameObject), gameObject.name, GetType().Name, $"Tag 'Player' is missing. Assign the 'Player' tag to the player GameObject.");

            _inventory = _player.GetComponent<Inventory>();
            if (_inventory == null)
                throw new MissingComponentException(nameof(Inventory), gameObject.name, GetType().Name);

            _slotsUI = GetComponentsInChildren<InventorySlotUI>().ToArray();
            if (_slotsUI == null)
                throw new MissingComponentException(nameof(InventorySlotUI), gameObject.name, GetType().Name);

            for (int i = 0; i < _slotsUI.Length; i++)
            {
                _slotsUI[i].id = i;
            }
        }
        catch (MissingComponentException ex)
        {
            Debug.LogError(ex.Message);
            enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
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
}

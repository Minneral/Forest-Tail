using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    GameObject _background;
    GameObject _inventory;
    public bool isActive { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            isActive = false;

            _background = GameObject.Find("InventoryBackGround");
            if (_background == null)
                throw new MissingComponentException(nameof(GameObject), gameObject.name, GetType().Name, "Child object 'InventoryBackGround' is missing!");

            _inventory = GameObject.Find("InventoryHub");
            if (_inventory == null)
                throw new MissingComponentException(nameof(GameObject), gameObject.name, GetType().Name, "Child object 'InventoryHub' is missing!");
        }
        catch (MissingComponentException ex)
        {
            Debug.LogError(ex.Message);
            enabled = false;
            return;
        }

        _background.SetActive(false);
        _inventory.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isActive = !isActive;

        _background.SetActive(isActive);
        _inventory.SetActive(isActive);
    }
}

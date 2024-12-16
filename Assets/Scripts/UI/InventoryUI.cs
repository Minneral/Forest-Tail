using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    GameObject _background;
    GameObject _inventory;
    public bool isActive { get; private set; }
    public static InventoryUI Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        GameEventsManager.instance.inputEvents.onInventoryPressed += InventoryPressed;
        GameEventsManager.instance.inputEvents.onClosePressed += CloseInventory;

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

    private void OnDestroy() {
        GameEventsManager.instance.inputEvents.onInventoryPressed -= InventoryPressed;
        GameEventsManager.instance.inputEvents.onClosePressed -= CloseInventory;
    }

    void InventoryPressed()
    {
        ToggleInventory();
    }

    void CloseInventory()
    {
        ToggleInventory(false);
    }

    void ToggleInventory(bool? state = null)
    {
        if (DialogueManager.instance.dialoguePanel.activeSelf)
        {
            isActive = false;
        }
        else
        {
            if (state == null)
                isActive = !isActive;
            else
                isActive = Convert.ToBoolean(state);
        }

        _background.SetActive(isActive);
        _inventory.SetActive(isActive);
    }
}

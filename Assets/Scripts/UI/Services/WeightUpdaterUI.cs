using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class WeightUpdaterUI : MonoBehaviour
{
    private GameObject _player;
    private Inventory _inventory;
    private TextMeshProUGUI _title;

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

            _title = GetComponent<TextMeshProUGUI>();
            if (_title == null)
                throw new MissingComponentException(nameof(TextMeshProUGUI), gameObject.name, GetType().Name);
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
        UpdateWeight();
    }

    void UpdateWeight()
    {
        string pattern = @"\d+/\d+";

        _title.text = Regex.Replace(_title.text, pattern, match =>
        {
            string[] parts = match.Value.Split('/');
            parts[0] = _inventory.GetWeight().ToString();
            parts[1] = _inventory.MaxWeight.ToString();
            return $"{parts[0]}/{parts[1]}";
        });
    }
}

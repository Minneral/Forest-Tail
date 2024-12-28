using UnityEngine;
using UnityEngine.UIElements;

public class DontDestroy : MonoBehaviour
{
    [HideInInspector]
    public string objectID;

    private void Awake()
    {
        if (string.IsNullOrEmpty(objectID))
        {
            objectID = name + transform.position.ToString() + transform.eulerAngles.ToString();
        }
    }

    void Start()
    {
        var foundObjects = FindObjectsOfType<DontDestroy>();
        foreach (var obj in foundObjects)
        {
            if (obj != this && obj.objectID == objectID)
            {
                Destroy(gameObject);
                return;
            }
        }

        DontDestroyOnLoad(gameObject);
    }
}

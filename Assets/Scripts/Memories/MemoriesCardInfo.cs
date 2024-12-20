using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MemoriesCardInfo", menuName = "Memories/CardInfo", order = 1)]
public class MemoriesCardInfo : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }
    public string cardName;
    public Sprite cardImage;

    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    OwnedItems,
    CluesObject,
    Default,
}
public abstract class ItemObject : ScriptableObject
{
    public int Id;
    public GameObject prefab;
    public ItemType type;
    [TextArea(15,20)]
    public string description;
}

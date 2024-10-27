using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Clue Object", menuName = "Inventory System/Item/Clue")]
public class CluesObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.CluesObject;
    }
}

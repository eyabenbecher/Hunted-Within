using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Owned Object", menuName = "Inventory System/Item/Owned")]
public class OwnedObject : ItemObject
{
    public void Awake()
    {
        type = ItemType.OwnedItems; 
    }
}

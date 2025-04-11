using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath = "inventorySave.json";
    private ItemDataBaseObject database;
    public List<InventorySlot> container = new List<InventorySlot>();

    private void OnEnable()
    {
# if UNITY_EDITOR
        database = (ItemDataBaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/database.asset", typeof(ItemDataBaseObject));
#else
    database= Resources.Load<ItemDataBaseObject>("database");
#endif
    }

    public void AddItem(ItemObject _item, int _amount)
    {
        for (int i = 0; i < container.Count; i++)
        {
            if (container[i].item == _item)
            {
                container[i].AddAmount(_amount);
                return;
            }
        }
        container.Add(new InventorySlot(database.GetID[_item], _item, _amount));
    }

    public void Save()
    {

        string fullPath = Path.Combine(Application.persistentDataPath, savePath);
        Debug.Log("Saving data to: " + fullPath);

        string saveData = JsonUtility.ToJson(this, true);
        File.WriteAllText(fullPath, saveData);
        Debug.Log("Data saved successfully.");
    }

    public void Load()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, savePath);
        Debug.Log("Loading data from: " + fullPath);

        if (File.Exists(fullPath))
        {
            string jsonData = File.ReadAllText(fullPath);
            JsonUtility.FromJsonOverwrite(jsonData, this);
            Debug.Log("Data loaded successfully.");
        }
        else
        {
            Debug.LogWarning("Save file not found at: " + fullPath);
        }
    }


    public void OnAfterDeserialize()
    {
        for (int i = 0; i < container.Count; i++)
        {
            container[i].item = database.GetItem[container[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {

    }
}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public ItemObject item;
    public int amount;

    public InventorySlot(int _id, ItemObject _item, int _amount)
    {
        ID = _id;
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}

using Assets.Scripts.Managers;
using Assets.Scripts.Old;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public List<Item> m_ItemList = new List<Item>();

    public Menu m_InventoryMenu;

    void Awake() {

        for(int i = 0; i < GetComponentsInChildren<Item>().Length; i++) {
            m_ItemList.Add(GetComponentsInChildren<Item>()[i]);
        }

        m_InventoryMenu.RebuildMenuOptionsFromChildren<InventoryObject>();
        m_InventoryMenu.AddCoordinatesToMenuOptions<InventoryObject>();
    }

    public void AddItemToMenu(Item item) {

        foreach(InventoryObject _InventoryObject in m_InventoryMenu.GetComponentsInChildren<InventoryObject>(true)) {
            if(_InventoryObject.name == item.name) {
                _InventoryObject.ShowInventoryObject();
                _InventoryObject.m_AssociatedItem = item;
                m_InventoryMenu.RebuildMenuOptionsFromChildren<InventoryObject>();
                return;
            }
        }

        if(GameManager.Instance.DebugMode) { Debug.Log("No Match Found for: " + item.name); }
    }

    public void AddContents(Item item, int quantity = 1) {

        if(item.m_Consumable) {
            foreach(Item Item in m_ItemList) {
                if(Item.name == item.name) {
                    Item.m_Quantity += item.m_Quantity;
                        
                    if(GameManager.Instance.DebugMode) { Debug.Log(quantity + " " + item.name + " added to inventory."); }
                        
                    break;
                }
            }
        }

        m_ItemList.Add(item);
        item.transform.SetParent(transform);

        AddItemToMenu(item);
    }

    public bool CheckForDuplicate(Item item) {
        
        foreach(Item Item in m_ItemList) {
            if(Item.name == item.name) { return true; }
        }
        return false;
    }
}

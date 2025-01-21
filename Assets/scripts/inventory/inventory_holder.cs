using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class inventory_holder : MonoBehaviour
{
    // inventory for the player
    [SerializeField] private int inventorySize ;
    [SerializeField] protected inventory_sys inventorySys;

    public inventory_sys InventorySys => inventorySys;
    public static UnityAction<inventory_sys> OnDynamicInventoryDisplayRequest;

    private void Awake()
    {
        inventorySys = new inventory_sys(inventorySize);
    }

    
}

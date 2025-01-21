using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class staticInventoryDisplay : inventory_display
{
    [SerializeField] private inventory_holder inventoryHolder;  
    [SerializeField] private inventorySlots_UI[] slots;
    protected override void Start()
    {
        base.Start();

        if(inventoryHolder != null)
        {
            inventorySys = inventoryHolder.InventorySys;
            inventorySys.OnInventoryChanged += UpdateSlot;
        }

        AssignedSlot(inventorySys);
    }

    public override void AssignedSlot(inventory_sys invDisplay)
    {
        slotDictionary = new Dictionary<inventorySlots_UI, inventory_player>();

        if(slots.Length != inventorySys.inventorySize)
        {
            Debug.LogWarning("Slot length does not match inventory length");
            return;
        }

        for (int i = 0; i < inventorySys.inventorySize; i++)
        {
            slotDictionary.Add(slots[i], inventorySys.InventorySlots[i]);
            slots[i].Init(inventorySys.InventorySlots[i]);
        }
    }
}

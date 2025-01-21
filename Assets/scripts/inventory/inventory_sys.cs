using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;


[System.Serializable]

public class inventory_sys
{   
    // inventory system 
    [SerializeField] private List<inventory_player> inventorySlots ;
    public List<inventory_player> InventorySlots => inventorySlots;
    public int inventorySize => inventorySlots.Count;
    public UnityAction<inventory_player> OnInventoryChanged;

    public inventory_sys(int size)
    {
        inventorySlots = new List<inventory_player>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new inventory_player());
        }
    }

    // add item to the inventory
    public bool AddToInventory(inventory_item item , int amountAdd)
    {
        if(ContainsItem(item, out List<inventory_player> invslot))
        {
            foreach(var slot in invslot) 
            {
                if(slot.IsPossibleToAdd(amountAdd))
                {
                    slot.addAmount(amountAdd);
                    OnInventoryChanged?.Invoke(slot);
                    return true;
                }
                else
                {
                    int amountLeft;
                    if(slot.IsPossibleToAdd(amountAdd, out amountLeft))
                    {
                        slot.addAmount(amountLeft);
                        amountAdd -= amountLeft;
                    }
                }
            }
        }
        
        if(HasFreeSlot(out inventory_player freeslot))
        {
            freeslot.UpdateSlot(item, amountAdd);
            OnInventoryChanged?.Invoke(freeslot);
            return true;
        }

        return false;
    }

    // check if item is in the inventory
    public bool ContainsItem(inventory_item item , out List<inventory_player> invslot)
    {
        invslot = inventorySlots.Where(x => x.Item == item).ToList();
        
        return invslot ==  null ? false : true;
    }

    // check if there is a free slot in the inventory
    public bool HasFreeSlot(out inventory_player freeslot)
    {
        freeslot = inventorySlots.FirstOrDefault(x => x.Item == null);
        
        return freeslot ==  null ? false : true;
    }

    // remove item from the inventory
    public void ConsumeOneItem(inventory_player slot)
    {
        slot.removeAmount(1);
        
        if(slot.Amount == 0)
        {
            slot.clearSlot();
        }
        
        OnInventoryChanged?.Invoke(slot);
    }

    // clear slot
    public void Clearslot(inventory_player slot)
    {
        slot.clearSlot();
        OnInventoryChanged?.Invoke(slot);
    }
    
    // remove item from the inventory
    public void RemoveItem(inventory_item item, int amountToRemove)
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.Item == item)
            {
                if (slot.Amount >= amountToRemove)
                {
                    slot.removeAmount(amountToRemove);
                    OnInventoryChanged?.Invoke(slot);
                    return;
                }
                else
                {
                    amountToRemove -= slot.Amount;
                    slot.clearSlot();
                    OnInventoryChanged?.Invoke(slot);
                }
            }
        }
    }

    // check if there are enough items in the inventory
    public bool HasEnoughItems(List<inventory_item> items, List<int> amounts)
    {
        if (items.Count != amounts.Count) return false;

        for (int i = 0; i < items.Count; i++)
        {
            inventory_item item = items[i];
            int amount = amounts[i];
            if (!ContainsItem(item, out List<inventory_player> slots)) return false;

            int totalAmount = 0;
            foreach (inventory_player slot in slots)
            {
                totalAmount += slot.Amount;
                if (totalAmount >= amount) break;
            }

            if (totalAmount < amount) return false;
        }

        return true;
    }

    // consume items from the inventory
    public void ConsumeItems(List<inventory_item> items, List<int> amounts)
    {
        if (items.Count != amounts.Count) return;

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            var amount = amounts[i];

            if (ContainsItem(item, out List<inventory_player> slots))
            {
                foreach (var slot in slots)
                {
                    if (slot.Amount > amount)
                    {
                        slot.removeAmount(amount);
                        OnInventoryChanged?.Invoke(slot);
                        break;
                    }
                    else
                    {
                        amount -= slot.Amount;
                        slot.clearSlot();
                        OnInventoryChanged?.Invoke(slot);
                    }
                }
            }
        }
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class inventory_player 
{
    [SerializeField] private inventory_item item;
    [SerializeField] public int amount;

    public inventory_item Item => item;
    public int Amount => amount;

    // slots for the player inventory and basic functionalities
    public inventory_player(inventory_item source, int val)
    {
        item = source;
        amount = val;
    }

    public inventory_player() 
    {
        item = null;
        amount = -1;
    }

    public void clearSlot()
    {
        item = null;
        amount = -1;
    }

    public bool IsPossibleToAdd(int val)
    {
        return amount + val <= item.maxValue;
    }

    public bool IsPossibleToAdd(int val , out int amountLeft)
    {
        amountLeft = item.maxValue - amount;
        return amount + val <= item.maxValue;
    }

    public void addAmount(int val)
    {
        amount += val;
    }

    public void removeAmount(int val)
    {
        amount -= val;
    }

    public void UpdateSlot(inventory_item source , int val)
    {
        item = source;
        amount = val;
    }

    public void AssignItem(inventory_player invSlot)
    {
        if(item == invSlot.Item)
        {
            addAmount(invSlot.Amount);
        }
        else
        {
            item = invSlot.Item;
            amount = 0;
            addAmount(invSlot.Amount);
        }
    }

    public bool SplitStack(out inventory_player newSlot)
    {
        if(amount > 1)
        {   
            int halfstack = Mathf.RoundToInt(amount / 2);
            removeAmount(halfstack);
            newSlot = new inventory_player(item, halfstack);
            
            return true;
        }
        else
        {
            newSlot = null;
            return false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public abstract class inventory_display : MonoBehaviour
{
    // refrences to the inventory UI
    [SerializeField] inventory_UI mouseInventoryUI;
    protected inventory_sys inventorySys;
    protected Dictionary<inventorySlots_UI,inventory_player> slotDictionary ;
    public inventory_sys InventorySys => inventorySys;
    public Dictionary<inventorySlots_UI,inventory_player> SlotDictionary => slotDictionary;
    protected virtual void Start()
    {

    }

    public abstract void AssignedSlot(inventory_sys invDisplay) ;

    protected virtual void UpdateSlot(inventory_player updatedSlot)
    {
        foreach (var slot in slotDictionary)
        {
            if(slot.Value == updatedSlot)
            {
                slot.Key.UpdateSlotUI(updatedSlot);
            }
        }
    }

    // logic for slot click
    public void SlotClicked(inventorySlots_UI clickedslot)
    {   
        bool isShiftPressed = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;

        if(clickedslot.AssignedSlot.Item != null && mouseInventoryUI.inventorySlot.Item == null) 
        {   
            // splitting stack
            if(isShiftPressed && clickedslot.AssignedSlot.SplitStack(out inventory_player newSlot))
            {
                mouseInventoryUI.UpdateMouseSlot(newSlot);
                clickedslot.UpdateSlotUI();
            }
            else {
                mouseInventoryUI.UpdateMouseSlot(clickedslot.AssignedSlot);
                clickedslot.clearSlot();
            }

            return ;
        }

        if(clickedslot.AssignedSlot.Item == null && mouseInventoryUI.inventorySlot.Item != null)
        {
            // assign item to slot
            clickedslot.AssignedSlot.AssignItem(mouseInventoryUI.inventorySlot);
            clickedslot.UpdateSlotUI();
            mouseInventoryUI.ClearSlot();
        }

        if(clickedslot.AssignedSlot.Item != null && mouseInventoryUI.inventorySlot.Item != null)
        {   
            bool isSameItem = clickedslot.AssignedSlot.Item == mouseInventoryUI.inventorySlot.Item;

            if(isSameItem) // if same item then add to stack
            {   
                int amountLeft ;
                if(clickedslot.AssignedSlot.IsPossibleToAdd(mouseInventoryUI.inventorySlot.Amount , out amountLeft))
                {
                    clickedslot.AssignedSlot.AssignItem(mouseInventoryUI.inventorySlot);
                    clickedslot.UpdateSlotUI();

                    mouseInventoryUI.ClearSlot();
                }
                else 
                {   
                    Debug.Log(mouseInventoryUI.inventorySlot.Amount);
                    Debug.Log(clickedslot.AssignedSlot.Amount);
                    Debug.Log(amountLeft);

                    if(amountLeft < 1 || mouseInventoryUI.inventorySlot.Amount >= clickedslot.AssignedSlot.Amount) 
                    {
                        SwapSlots(clickedslot);
                    }
                    else 
                    {   
                        int remainingAmount = mouseInventoryUI.inventorySlot.Amount - amountLeft;
                        clickedslot.AssignedSlot.addAmount(amountLeft);
                        clickedslot.UpdateSlotUI();

                        var newItem = new inventory_player(mouseInventoryUI.inventorySlot.Item, remainingAmount);
                        mouseInventoryUI.ClearSlot();
                        mouseInventoryUI.UpdateMouseSlot(newItem);
                    }
                        
                }
                
            }
            else 
            {
                SwapSlots(clickedslot);
            }
        }
    }

    // logic for swapping slots
    private void SwapSlots(inventorySlots_UI clickedslot)
    {
        var clonedSlot = new inventory_player(mouseInventoryUI.inventorySlot.Item, mouseInventoryUI.inventorySlot.Amount);
        mouseInventoryUI.ClearSlot();
        
        mouseInventoryUI.UpdateMouseSlot(clickedslot.AssignedSlot);
        clickedslot.clearSlot();
        
        clickedslot.AssignedSlot.AssignItem(clonedSlot);
        clickedslot.UpdateSlotUI();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;  
using UnityEngine.EventSystems;

public class inventory_UI : MonoBehaviour
{
    public Image sprite;
    public TextMeshProUGUI itemCount;
    public inventory_player inventorySlot;
    
    private void Awake()
    {
        sprite.color = Color.clear;
        itemCount.text = "";
    }

    // mouse slot related functionalities
    public void UpdateMouseSlot(inventory_player invslot)
    {
        inventorySlot.AssignItem(invslot);
        sprite.sprite = invslot.Item.Icon;
        itemCount.text = invslot.Amount.ToString();
        sprite.color = Color.white;
    }

    private void Update()
    {
        if(inventorySlot.Item != null) 
        {
            transform.position = Input.mousePosition; 

            if(Mouse.current.leftButton.wasPressedThisFrame && !IsPointerOverUIObject())
            {
                ClearSlot();
            }
        }
    }

    public void ClearSlot()
    {
        inventorySlot.clearSlot();
        sprite.color = Color.clear;
        sprite.sprite = null;
        itemCount.text = "";
    }

    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Input.mousePosition; 
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}

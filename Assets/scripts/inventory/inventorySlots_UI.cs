using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class inventorySlots_UI : MonoBehaviour
{
    [SerializeField] private Image sprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private inventory_player assigned_slot;

    private Button button; 

    public inventory_player AssignedSlot => assigned_slot;
    public inventory_display ParentDisplay { get; private set; }

    // slots for the player inventory UI and basic functionalities
    private void Awake()
    {   
        clearSlot();

        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick);

        ParentDisplay = transform.parent.GetComponent<inventory_display>();
    }

    public void Init(inventory_player slot)
    {
        assigned_slot = slot;
        UpdateSlotUI(slot);
    }

    // update slot UI
    public void UpdateSlotUI(inventory_player slot)
    {   
        if(slot.Item != null) 
        {
            sprite.sprite = slot.Item.Icon;
            sprite.color = Color.white;
            itemCount.text = slot.Amount.ToString();

            if(slot.Amount > 1) 
            {
                itemCount.text = slot.Amount.ToString();
            }
            else 
            {
                itemCount.text = "";
            }

        }
        else 
        {
            clearSlot();
        }
    }

    public void UpdateSlotUI()
    {
        if(assigned_slot != null) UpdateSlotUI(assigned_slot);
    }

    public void clearSlot()
    {
        assigned_slot?.clearSlot();
        sprite.sprite = null;
        sprite.color = Color.clear;
        itemCount.text = "";
    }

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }
}

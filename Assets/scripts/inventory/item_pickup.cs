using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider2D))]
public class item_pickup : MonoBehaviour
{
    // pick the item from the scene
    public float PickupRadius = 1.0f;
    public inventory_item item;
    private CapsuleCollider2D pickupCollider;
    
    private void Awake()
    {
        pickupCollider = GetComponent<CapsuleCollider2D>();
        pickupCollider.isTrigger = true;
        pickupCollider.size = new Vector2(PickupRadius, PickupRadius / 2);
        pickupCollider.direction = CapsuleDirection2D.Vertical;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        var inventory = collision.transform.GetComponent<inventory_holder>();
        
        if (!inventory) return ;

        if(inventory.InventorySys.AddToInventory(item,1)) 
        {
            Destroy(this.gameObject);
        }
    }
}

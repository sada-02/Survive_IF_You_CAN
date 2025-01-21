using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lootbag : MonoBehaviour
{
    public GameObject droppedItem;
    public List<loot> lootlist = new List<loot>();

    // choosing a random item from the loot list
    loot GetDroppedItem()
    {
        int randomNumber = Random.Range(0, 101);
        List<loot> possibleItems = new List<loot>();

        foreach (loot item in lootlist)
        {
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }

        if(possibleItems.Count > 0)
        {
            int randomIndex = Random.Range(0, possibleItems.Count);
            return possibleItems[randomIndex];
        }

        return null;
    }

    public void instantiateItem()
    {
        loot item = GetDroppedItem();
        if (item != null)
        {
            GameObject dropped = Instantiate(droppedItem, transform.position, Quaternion.identity);
            dropped.GetComponentInChildren<SpriteRenderer>().sprite = item.lootSprite;
            dropped.GetComponentInChildren<item_pickup>().item = item.item;
        }
    }
}
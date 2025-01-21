using System.Collections.Generic;
using UnityEngine;

public class StatsImprovement : MonoBehaviour
{
    public player player;

    // Upgrade levels
    public int healthLevel = 1;
    public int staminaLevel = 1;
    public int attackLevel = 1;

    // Base upgrade costs
    public int baseHealthCost = 5;
    public int baseStaminaCost = 5;
    public int baseAttackCost = 5;

    public int healthImp = 20;
    public int staminaImp = 10;
    public int attackImp = 3;

    // Inventory system reference
    [SerializeField] public inventory_holder playerInventory;

    // Items required for upgrades
    [SerializeField] public List<inventory_item> healthItems;
    [SerializeField] public List<inventory_item> staminaItems;
    [SerializeField] public List<inventory_item> attackItems;

    // Amount of each item required for upgrades
    [SerializeField] public List<int> healthItemAmounts;
    [SerializeField] public List<int> staminaItemAmounts;
    [SerializeField] public List<int> attackItemAmounts;

    public void UpgradeStat(string stat)
    {
        List<inventory_item> requiredItems = GetRequiredItems(stat);
        List<int> requiredAmounts = GetRequiredAmounts(stat);

        if (requiredItems == null || requiredAmounts == null || requiredItems.Count != requiredAmounts.Count)
        {
            return;
        }

        // Check if player has enough items
        if (playerInventory.InventorySys.HasEnoughItems(requiredItems, requiredAmounts))
        {
            playerInventory.InventorySys.ConsumeItems(requiredItems, requiredAmounts);

            switch (stat)
            {
                case "Health":
                    healthLevel++;
                    player.maxHealth += healthImp;
                    player.currentHealth = player.maxHealth;
                    player.healthBar.SetMaxHealth(player.maxHealth);
                    Debug.Log($"Health upgraded! New health: {player.maxHealth}, Level: {healthLevel}");
                    break;

                case "Stamina":
                    staminaLevel++;
                    player.maxStamina += staminaImp;
                    player.currentStamina = player.maxStamina;
                    player.staminaBar.SetMaxStamina(player.maxStamina);
                    break;

                case "Attack":
                    attackLevel++;
                    player.attackDamage += attackImp;
                    break;
            }
        }
    }

    // script to determine which items are required for each upgrade
    public List<inventory_item> GetRequiredItems(string stat)
    {
        switch (stat)
        {
            case "Health": return healthItems;
            case "Stamina": return staminaItems;
            case "Attack": return attackItems;
            default: return null;
        }
    }

    // script to determine how much materials are required for each upgrade
    public List<int> GetRequiredAmounts(string stat)
    {
        switch (stat)
        {
            case "Health": return healthItemAmounts;
            case "Stamina": return staminaItemAmounts;
            case "Attack": return attackItemAmounts;
            default: return null;
        }
    }
}
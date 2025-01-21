using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class StatsImprovementUI : MonoBehaviour
{
    public StatsImprovement statsImprovement; 

    // representing the text showing level and current value of each stat
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI attackText;

    // representing the text showing required items and their amounts for each stat upgrade
    public TextMeshProUGUI healthUpgradeText;
    public TextMeshProUGUI staminaUpgradeText;
    public TextMeshProUGUI attackUpgradeText;
    
    // representing the buttons for each stat upgrade
    public Button upgradeHealthButton;
    public Button upgradeStaminaButton;
    public Button upgradeAttackButton;

    private void Start()
    {
        // Show current values and levels in the UI
        healthText.text = $"Health: {statsImprovement.player.maxHealth} (Level: {statsImprovement.healthLevel})";
        staminaText.text = $"Stamina: {statsImprovement.player.maxStamina} (Level: {statsImprovement.staminaLevel})";
        attackText.text = $"Attack: {statsImprovement.player.attackDamage} (Level: {statsImprovement.attackLevel})";

        // Show required items and their amounts for each stat upgrade
        healthUpgradeText.text = GetUpgradeText("Health");
        staminaUpgradeText.text = GetUpgradeText("Stamina");
        attackUpgradeText.text = GetUpgradeText("Attack");
        
        // Button listeners
        upgradeHealthButton.onClick.AddListener(OnUpgradeHealthClicked);
        upgradeStaminaButton.onClick.AddListener(OnUpgradeStaminaClicked);
        upgradeAttackButton.onClick.AddListener(OnUpgradeAttackClicked);
    }

    private void UpdateUI()
    {
        // Show current values and levels in the UI
        healthText.text = $"Health: {statsImprovement.player.maxHealth} (Level: {statsImprovement.healthLevel})";
        staminaText.text = $"Stamina: {statsImprovement.player.maxStamina} (Level: {statsImprovement.staminaLevel})";
        attackText.text = $"Attack: {statsImprovement.player.attackDamage} (Level: {statsImprovement.attackLevel})";

        // Show required items and their amounts for each stat upgrade
        healthUpgradeText.text = GetUpgradeText("Health");
        staminaUpgradeText.text = GetUpgradeText("Stamina");
        attackUpgradeText.text = GetUpgradeText("Attack");
    }

    private string GetUpgradeText(string stat)
    {
        List<inventory_item> requiredItems = statsImprovement.GetRequiredItems(stat);
        List<int> requiredAmounts = statsImprovement.GetRequiredAmounts(stat);

        string upgradeText = "";

        if (requiredItems != null && requiredAmounts != null)
        {
            for (int i = 0; i < requiredItems.Count; i++)
            {
                upgradeText += $"{requiredItems[i].Name}: {requiredAmounts[i]}\n";
            }
        }
        else
        {
            upgradeText = "No items required.";
        }

        return upgradeText;
    }
    
    private void OnUpgradeHealthClicked()
    {
        List<inventory_item> requiredItems = statsImprovement.GetRequiredItems("Health");
        List<int> requiredAmounts = statsImprovement.GetRequiredAmounts("Health");

        if (requiredItems != null && requiredAmounts != null)
        {
            if (statsImprovement.playerInventory.InventorySys.HasEnoughItems(requiredItems, requiredAmounts))
            {
                statsImprovement.UpgradeStat("Health");
                UpdateUI(); // Refresh the UI after upgrade
            }
            else
            {
                Debug.Log("Not enough items to upgrade Health.");
            }
        }
    }

    private void OnUpgradeStaminaClicked()
    {
        List<inventory_item> requiredItems = statsImprovement.GetRequiredItems("Stamina");
        List<int> requiredAmounts = statsImprovement.GetRequiredAmounts("Stamina");

        if (requiredItems != null && requiredAmounts != null)
        {
            if (statsImprovement.playerInventory.InventorySys.HasEnoughItems(requiredItems, requiredAmounts))
            {
                statsImprovement.UpgradeStat("Stamina");
                UpdateUI();
            }
            else
            {
                Debug.Log("Not enough items to upgrade Stamina.");
            }
        }
    }

    private void OnUpgradeAttackClicked()
    {
        List<inventory_item> requiredItems = statsImprovement.GetRequiredItems("Attack");
        List<int> requiredAmounts = statsImprovement.GetRequiredAmounts("Attack");

        if (requiredItems != null && requiredAmounts != null)
        {
            if (statsImprovement.playerInventory.InventorySys.HasEnoughItems(requiredItems, requiredAmounts))
            {
                statsImprovement.UpgradeStat("Attack");
                UpdateUI();
            }
            else
            {
                Debug.Log("Not enough items to upgrade Attack.");
            }
        }
    }
}
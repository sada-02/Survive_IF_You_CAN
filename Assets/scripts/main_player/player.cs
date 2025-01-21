using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class player : MonoBehaviour
{
    // refrences required
    public health_sys healthBar;
    public stamina_sys staminaBar;
    public player_movement playerMovement;
    public hunger_sys hungerBar;
    public Rigidbody2D rb;
    public inventory_holder inventoryHolder;
    public Transform Aim;
    public GameObject bullet;
    public Animator animator;
    public GameObject pauseMenuUI;
    public LayerMask enemyLayer; // Layers for enemies
    public CircleCollider2D attackCollider;
    public GameObject statsImprovementUI;
    public GameObject gameOverUI;

    // range attacks
    public float bulletSpeed = 15f;
    float shootCooldown = 2f;
    float shootTimer = 0f;

    // Stats related variables
    public float maxHealth = 50f , maxStamina = 30f , regStamina = 2f ;
    public float currentHealth , currentStamina ;
    private int healthIncrease = 30;
    private int hungerIncrease = 25;
    public static bool ispaused = false;

    public float runCost = 4f , attackCost = 2f;
    public float hungerCost = 3f , maxHunger = 100f , currentHunger = 0f;

    public float attackCooldown = 3f; // Cooldown time between attacks
    private float lastAttackTime = 0f; // Tracks when the last attack occurred
    public float attackRange = 1f; // Attack range
    public int attackDamage = 2; // Damage dealt per attack
    

    void Start()
    {   
        
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentHunger = maxHunger;
        Time.timeScale = 1f;
        ispaused = false;

        healthBar.SetMaxHealth(maxHealth);
        staminaBar.SetMaxStamina(maxStamina);
        hungerBar.SetMaxHunger(maxHunger);

        attackCollider.enabled = false;

    }

    void Update()
    {

        shootTimer += Time.deltaTime;

        // open stats menu
        if(Input.GetKeyDown(KeyCode.U))
        {   
            if(statsImprovementUI.activeSelf)
            {
                statsImprovementUI.SetActive(false);
                ispaused = false;
                Time.timeScale = 1f;
            }
            else
            {
                statsImprovementUI.SetActive(true);
                ispaused = true;
                Time.timeScale = 0f;
            }
        }

        // open pause menu
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(ispaused)
            {
                if(statsImprovementUI.activeSelf)
                {
                    statsImprovementUI.SetActive(false);
                }

                Resume();
            }
            else 
            {
                Pause();
            }
        }

        // attacks
        if (Input.GetKeyDown(KeyCode.Z)) 
        {
            Attack();
        }
        else if (Input.GetKeyDown(KeyCode.X)) 
        {
            OnShoot();
        }

        // Stamina consumption 
        if(playerMovement.isRunning)  
        {   
            if(currentStamina < runCost * Time.deltaTime) 
            {
                playerMovement.isRunning = false;
            }
            else
            {
                currentStamina -= runCost * Time.deltaTime;
                staminaBar.Setstamina(currentStamina);
            }
        }
        else
        {   
            if(maxStamina - currentStamina < regStamina * Time.deltaTime)
            {
                currentStamina = maxStamina;
            }
            else if(currentStamina < maxStamina)
            {
                currentStamina += regStamina * Time.deltaTime;
                staminaBar.Setstamina(currentStamina);
            }
        }

        // accessing inventory
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ClearSlot(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) ClearSlot(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) ClearSlot(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4)) ClearSlot(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5)) ClearSlot(4);
            else if (Input.GetKeyDown(KeyCode.Alpha6)) ClearSlot(5);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ConsumeSlot(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) ConsumeSlot(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) ConsumeSlot(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4)) ConsumeSlot(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5)) ConsumeSlot(4);
            else if (Input.GetKeyDown(KeyCode.Alpha6)) ConsumeSlot(5);
        }
                
        // Hunger consumption
        UpdateHunger();

    }

    // shooting bullets logic
    void OnShoot()
    {
        if (shootTimer > shootCooldown && currentStamina >= attackCost)
        {
            GameObject bulletObj = Instantiate(bullet, Aim.position, Aim.rotation);

            Rigidbody2D bulletRb = bulletObj.GetComponent<Rigidbody2D>();
            bulletRb.AddForce(Aim.up * bulletSpeed, ForceMode2D.Impulse);

            currentStamina -= attackCost;
            staminaBar.Setstamina(currentStamina);

            shootTimer = 0f;
            Destroy(bulletObj, 5f);
        }
    }

    // pause menu logic
    public void Pause()
    {   
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        ispaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        ispaused = false;
    }

    // hunger deduction logic
    void UpdateHunger()
    {
        if(currentHunger < hungerCost * Time.deltaTime * (playerMovement.isRunning ? 2f : 0.66f))
        {
            currentHunger = 0f;
            TakeDamage(2 * Time.deltaTime);
        }
        else
        {   
            if(playerMovement.isMoving)
            {
                currentHunger -= hungerCost * Time.deltaTime * (playerMovement.isRunning ? 2f : 0.66f);
                hungerBar.Sethunger(currentHunger);
            }
            else 
            {
                currentHunger -= hungerCost * Time.deltaTime * 1e-2f;
                hungerBar.Sethunger(currentHunger);
            }
        }
    }

    // health deduction logic 
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(0f,currentHealth);
        healthBar.SetHealth(currentHealth);

        animator.SetTrigger("damageTaken");

        if(currentHealth <= 0f)
        {
            StartCoroutine(Die());
        }
    }

    // attack logic
    public void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown || currentStamina < attackCost)
        {
            return;
        }

        currentStamina -= attackCost;
        staminaBar.Setstamina(currentStamina);

        if(currentStamina >= 0)
        {
            animator.SetTrigger("attack");
            StartCoroutine(EnableAttackCollider());
        }

        lastAttackTime = Time.time;
    }

    private IEnumerator EnableAttackCollider()
    {
        attackCollider.enabled = true; 
        yield return new WaitForSeconds(0.2f); 
        attackCollider.enabled = false; 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (attackCollider.enabled)
        {
            EnemyBase enemy = other.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage); // Call the TakeDamage method
            }
        }
    }

    // death logic
    private IEnumerator Die()
    {   
        animator.SetTrigger("isdead");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        ispaused = true;
    }

    // consumtion of items logic
    public void ConsumeSlot(int slotIdx)
    {   
        inventory_item item = inventoryHolder.InventorySys.InventorySlots[slotIdx].Item;

        if(item != null)
        {   
            if(item.itemType == "health")
            {
                currentHealth += healthIncrease;
                currentHealth = Mathf.Min(currentHealth,maxHealth);
                healthBar.SetMaxHealth(currentHealth);
            }
            else if(item.itemType == "hunger")
            {
                currentHunger += hungerIncrease;
                currentHunger = Mathf.Min(currentHunger,maxHunger);
                hungerBar.Sethunger(currentHunger);
            }
            else if(item.itemType == "health_hunger")
            {
                currentHealth += healthIncrease;
                currentHealth = Mathf.Min(currentHealth,maxHealth);
                healthBar.SetHealth(currentHealth);

                currentHunger += hungerIncrease;
                currentHunger = Mathf.Min(currentHunger,maxHunger);
                hungerBar.Sethunger(currentHunger);
            }
            else
            {
                return;
            }

            inventoryHolder.InventorySys.ConsumeOneItem(inventoryHolder.InventorySys.InventorySlots[slotIdx]);
        }
    }
    public void ClearSlot(int slotIdx)
    {
        inventory_player item = inventoryHolder.InventorySys.InventorySlots[slotIdx];
        inventoryHolder.InventorySys.Clearslot(item);
    }

}

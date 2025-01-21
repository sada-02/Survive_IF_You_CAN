using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    // movement variables
    public float movespeed = 4f , runSpeed = 8f , currentSpeed = 0f;

    // refrences needed
    public Rigidbody2D rb ;
    public Animator animator ;
    public player player;
    public Transform Aim ;

    Vector2 movement , lastMovement;
    public bool isRunning = false , isMoving = false;
 
    // Update is called once per frame
    void Update()
    {
        
        HandleMovement();

        animator.SetFloat("horizontal",movement.x);
        animator.SetFloat("vertical",movement.y);
        animator.SetFloat("speed",currentSpeed);
        animator.SetBool("isRunning",isRunning);
        animator.SetBool("isMoving",isMoving);
        
    }

    void FixedUpdate()
    {   
        bool keyPressed = movement.sqrMagnitude > 0;
        isMoving = keyPressed; // key pressed movement
        
        currentSpeed = isRunning ? runSpeed : movespeed;
        currentSpeed = isMoving ? currentSpeed : 0f;

        // stop running when stamina is low
        if(player.currentStamina <= player.runCost && isRunning) 
        {
            isRunning = false;

            currentSpeed = isMoving ? movespeed : 0f;
        }

        // reducing speed to half when hunger drops a certain value
        if(player.currentHunger <= 0.3f * player.maxHunger) 
        {
            isRunning = false;
            currentSpeed = isMoving ? movespeed * 0.5f : 0f;
        }

        rb.MovePosition(rb.position + movement.normalized * currentSpeed * Time.fixedDeltaTime);

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = new Vector2(
            mousePosition.x - transform.position.x,
            mousePosition.y - transform.position.y
        );

        Aim.rotation = Quaternion.LookRotation(Vector3.forward, direction);

    }

    void HandleMovement() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bool shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        isRunning = shiftPressed; // shift running movement
    
    }

    // Detect the key pressed
    private KeyCode DetectMovementKey()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) return KeyCode.W;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) return KeyCode.S;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) return KeyCode.A;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) return KeyCode.D;
        return KeyCode.None;
    }

    
}

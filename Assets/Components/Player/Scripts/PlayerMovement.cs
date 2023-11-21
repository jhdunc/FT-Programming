using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // set default speed variable
    public float moveSpeed = 3f;
    
    // bool to yes/no allow player movement
    public bool playerCanMove;

    // bool to enable sprint
    // bool to check if player is already sprinting
    public bool sprint;
    private bool isSprinting;

    // set variable for components in Player object
    public Rigidbody2D rb;
    public Animator anim;

    // store the movement input
    Vector2 movement;

    private void Start()
    {
        playerCanMove = true;
        isSprinting = false;
    }
    void Update()
    {
        #region Input

        // Store a value between -1  and 1  based on movement input (in game settings), with 0 being no input

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // If player is moving, set animator's Parameter to match movement input
        if (movement != Vector2.zero)
        {
            anim.SetFloat("moveX", movement.x);
            anim.SetFloat("moveY", movement.y);
        }

        // set animator's speed Parameter to match player speed
        anim.SetFloat("Speed", movement.sqrMagnitude);
        
        // If Sprint bool is true, player toggles SPRINTING with Left Shift. 
        // Change the control by changing the KeyCode in the first if statement.
        if (Input.GetKeyDown(KeyCode.LeftShift) && sprint == true)
        {
            if (isSprinting == false)
            {
                isSprinting = true;
                moveSpeed += 2f;
            }
            else
            {
                isSprinting = false;
                moveSpeed -= 2f;
            }
        }
        #endregion
    }
    void UpdateAnimation()
    {
        if (movement != Vector2.zero)
        {
            anim.Play("Movement");
        }
        else
        { 
            anim.Play("Player_Idle");
        }

    }


    private void FixedUpdate()
    {
        #region Movement
        if (playerCanMove == true)
        {
            // Move position of rigidbody (player): start at current position,
            // add movement based on Vector2 variable changes from input (will be 0 if no change)
            // moveSpeed affects how fast the player moves, and Time.fixedDeltaTime means it's a constant movement speed

            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
        UpdateAnimation();

        #endregion
    }
}
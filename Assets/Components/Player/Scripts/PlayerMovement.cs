using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    // set default speed variable
    [Tooltip("Set base speed for the player here.")]
    public float moveSpeed;
    // bool to enable sprint
    [Tooltip("Enable or Disable Sprint in game.")]
    public bool sprint;
    [Tooltip("Set the maximum value for stamina.")]
    public float sprintStamina;

    [Header("Game Settings")]
    // bool to yes/no allow player movement
    private bool playerCanMove;
        
    // bool to check if player is already sprinting
    private bool isSprinting;

    // set variable for components in Player object
    private Rigidbody2D rb;
    private Animator anim;

    // store the movement input
    Vector2 movement;

    private GameObject timerUI;
    private GameObject slider;

    private float timeRemaining;

    private void Start()
    {
        playerCanMove = true;
        isSprinting = false;

        rb = GetComponentInChildren<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

    }
    private void Awake()
    {
        // if no max stamina has been set by client, set to default.
        if (sprintStamina == 0)
        { sprintStamina = 4; }

        if(moveSpeed ==0)
        { moveSpeed = 2; }
        // Setup stamina variables in code to make client-side easier
        timerUI = transform.Find("PlayerObject/StaminaUI").gameObject;
        slider = transform.Find("PlayerObject/StaminaUI/Slider").gameObject;


        timeRemaining = sprintStamina; // current no time has elapsed at start
        timerUI.SetActive(false); // sprint UI does not appear until sprinting
        slider.GetComponent<Slider>().maxValue = sprintStamina; // set max value of slider to sprint stamina max
        slider.GetComponent<Slider>().value = sprintStamina; // set value at start to max stamina
    }
    #region Sprint Methods
    void DecayStamina()
    {
        if (timeRemaining <= sprintStamina && isSprinting)
        {
            if (movement != Vector2.zero)
            {
                timeRemaining -= Time.deltaTime;
                slider.GetComponent<Slider>().value = timeRemaining;
            }
        }
    }
    void RechargeStamina()
    {
        if((!isSprinting && timeRemaining < sprintStamina) || movement == Vector2.zero)
            {
                timeRemaining += Time.deltaTime;
                slider.GetComponent<Slider>().value = timeRemaining;
                if (timeRemaining > sprintStamina)
                { timeRemaining = sprintStamina; }
         
        }
    }
    void Sprint()
    {
        // If Sprint bool is true, player toggles SPRINTING with Left Shift. 
        // Change the control by changing the KeyCode in the first if statement.
        if (Input.GetKeyDown(KeyCode.LeftShift) && sprint == true)
        {
            if (isSprinting == false && timeRemaining > 0)
            {
                isSprinting = true;
                moveSpeed *= 1.5f;
            }
            else
            {
                EndSprint();
            }
        }
        if (timeRemaining <= 0)
        {
            EndSprint();
        }
    }
    void EndSprint()
    {
        isSprinting = false;
        moveSpeed /= 1.5f;
    }
    void SprintUI()
    {
        if (sprintStamina >= timeRemaining)
        {
            if (isSprinting || timeRemaining < sprintStamina)
            { timerUI.SetActive(true); }
            else
            { timerUI.SetActive(false); }
        }
    }

    #endregion
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

        Sprint();

        DecayStamina();
        RechargeStamina();
        SprintUI();
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    float horizontalMove = 0f;

    bool jump = false;

    float actionCooldown = 0.75f;
    float timeSinceAction = 0f;

    bool crouch = false;

    // Update is called once per frame
    void Update()
    {
        timeSinceAction += Time.deltaTime;
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            if (timeSinceAction > actionCooldown)
            {
                timeSinceAction = 0f;
                jump = true;
                animator.SetBool("IsJumping", true);
            }
             
        }
        
       /* if (Input.GetButtonDown("Crouch")) // crouching not needed
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        }*/
    }

    public void Stop()
    {
        GetComponent<PlayerMovement>().runSpeed = 0f;
    }

    public void Go()
    {
        GetComponent<PlayerMovement>().runSpeed = 25f;
    }

    public void OnLanding ()
    {
        animator.SetBool("IsJumping", false);
    }

    private void FixedUpdate()
    {
        //Moving our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 direction;
    public float speed = 8;

    public float jumpForce = 10;
    public float gravity = -20;
    public float checkRadius = .15f;
    public Transform groundCheck;
    public Transform topCheck;
    public Transform leftCheck;
    public Transform rightCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;

    public bool isGrounded;
    public bool isUnderPlatform;
    public bool isOnWall_Left;
    public bool isOnWall_Right;

    public bool ableToMakeADoubleJump = true;

    void Update()
    {
        //Take the horizontal input to move the player
        float hInput = Input.GetAxis("Horizontal");
        direction.x = hInput * speed;

        //Take the vertical input for movement along walls
        float vInput = Input.GetAxis("Vertical");

        //Check if the player is on the ground or stuck under platform
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRadius, groundLayer);
        isUnderPlatform = Physics.CheckSphere(topCheck.position, checkRadius, groundLayer);
        isOnWall_Left = Physics.CheckSphere(leftCheck.position, checkRadius, wallLayer);
        isOnWall_Right = Physics.CheckSphere(rightCheck.position, checkRadius, wallLayer);

        if (isGrounded && !isUnderPlatform) //Grounded Jump
        {
            ableToMakeADoubleJump = true;
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else if (isUnderPlatform && !isGrounded) //Platform/ceiling release jump
        {
            ableToMakeADoubleJump = true;
            if (Input.GetButtonDown("Jump"))
            {
                jumpForce *= -1;
                Jump();
                jumpForce *= -1;
            }
        }
        else if (isOnWall_Left || isOnWall_Right) //Wall Movement and Jump
        {
            ableToMakeADoubleJump = true;
            direction.y = vInput * speed;

            if (Input.GetButtonDown("Jump"))
            {
                if (isOnWall_Right)
                {
                    jumpForce *= -1;
                    WallJump();
                    jumpForce *= -1;
                    Jump();
                }
                else
                {
                    WallJump();
                    Jump();
                }
            }
        }
        else
        {
            direction.y += gravity * Time.deltaTime;//Add Gravity
            if (ableToMakeADoubleJump && Input.GetButtonDown("Jump"))
            {
                if (jumpForce < 0) //Check for negative jumpForce
                {
                    jumpForce *= -1;
                }

                DoubleJump();
            }
        }

        //Move the player using the character controller
        controller.Move(direction * Time.deltaTime);

        //Reset Z Position
        if (transform.position.z != 0)
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);

    }

    private void DoubleJump()
    {
        //Double Jump
        direction.y = jumpForce;
        ableToMakeADoubleJump = false;
    }
    private void Jump()
    {
        direction.y = jumpForce;
    }

    private void WallJump()
    {
        direction.x = jumpForce * 3;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
        Gizmos.DrawWireSphere(topCheck.position, checkRadius);
        Gizmos.DrawWireSphere(leftCheck.position, checkRadius);
        Gizmos.DrawWireSphere(rightCheck.position, checkRadius);
    }
}

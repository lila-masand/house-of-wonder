using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Scripted by Owen Ludlam
enum AnimStates
{
    Walk = 0, Run = 1, Idle = 2, Jump = 3, Loose = 4
}

public class PlayerMovement : MonoBehaviour
{
    // Physics variables
    public float speed;
    public float jump_force;
    public float sensitivity;
    public float gravity = -9.81f;

    private Vector3 velocity;
    private float xRot;
    private Vector2 player_mouse_input;
    private Vector3 player_movement_input;

    // Object variables
    private Animator animator;
    private CharacterController character_controller;
    public Transform camera_transform;

    private float yaw = 0;
    private float pitch = 0;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        character_controller = gameObject.GetComponent<CharacterController>();
        //platformScript = platform.GetComponent<MovingPlatform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the player mouse inputs. Prevent backwards walking
        player_movement_input = new Vector3(0f, 0f, Input.GetAxis("Vertical"));
        player_mouse_input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //Debug.Log(player_mouse_input.x.ToString() + " : " + player_mouse_input.y.ToString());

        // Complete player and camera movement
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 move_vector = transform.TransformDirection(player_movement_input);

        animator.SetBool("Grounded", character_controller.isGrounded);
        if (character_controller.isGrounded)
        {
            // Treat the character as grounded
            velocity.y = -1f;

            // Play the jumping animation and apply a vertical jump force
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetTrigger("Jump");
                velocity.y = jump_force;

                // Also move in direction you're facing (LM)
                velocity.z = (jump_force/3)*move_vector.z;
                velocity.x = (jump_force/3)*move_vector.x;
            }
        }
        else
        {
            // Apply generic gravity and play landing animation if applicable
            animator.SetTrigger("Land");
            velocity.y -= gravity * -1.5f * Time.deltaTime;

            // Gentler gravity for x and z directions (LM)
            velocity = Vector3.MoveTowards(velocity, new Vector3(0, velocity.y, 0), jump_force*Time.deltaTime);                       
        }

        // Move the character with the controller move script
        character_controller.SimpleMove(move_vector * speed);
        character_controller.Move(velocity * Time.deltaTime);

        // Rotate horizontally
        transform.Rotate(0f, Input.GetAxis("Horizontal") * sensitivity, 0f);

        // Set animation move-speed
        animator.SetFloat("MoveSpeed", move_vector.magnitude);
    }

    // Only 1 bool is valid, so set the valid bool
    void SetAnimator(AnimStates state)
    {
        animator.SetBool("IsWalking", state == AnimStates.Walk);
        animator.SetBool("IsRunning", state == AnimStates.Run);
        animator.SetBool("IsIdle", state == AnimStates.Idle);
        animator.SetBool("IsJumping", state == AnimStates.Jump);
        animator.SetBool("IsLoose", state == AnimStates.Loose);
    }
}

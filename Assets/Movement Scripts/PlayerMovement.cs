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
    public float speed = 5;
    public float jump_force = 6;
    public float sensitivity = 0.7f;
    public float gravity = -9.81f;

    private Vector3 velocity;
    private Vector3 player_movement_input;

    // Object variables
    private Animator animator;
    private CharacterController character_controller;
    public Transform camera_transform;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        character_controller = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Get the player mouse inputs. Prevent backwards walking
        float air_speed = 1f;
        if(!character_controller.isGrounded)
        {
            air_speed *= 0.6f; // Reduce air-movement
        }

        player_movement_input = new Vector3(0f, 0f, Input.GetAxis("Vertical") * air_speed);

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
                //velocity.z = (jump_force/3)*move_vector.z;
                //velocity.x = (jump_force/3)*move_vector.x;
            }
        }
        else
        {
            // Apply generic gravity and play landing animation if applicable
            animator.SetTrigger("Land");
            velocity.y -= gravity * -2f * Time.deltaTime;
        }


        // Combine movement vectors to reduce calls to the move script and fix the no-mid-air movement bug
        Vector3 combined_move_vector = (move_vector * speed + velocity) * Time.deltaTime;

        // Move the character with the controller move script
        character_controller.Move(combined_move_vector);

        // Rotate horizontally
        transform.Rotate(0f, Input.GetAxis("Horizontal") * sensitivity, 0f);

        // Set animation move-speed
        animator.SetFloat("MoveSpeed", move_vector.magnitude);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceMovement : MonoBehaviour
{
    public float speed = 6f;

    Vector3 movement;
    Rigidbody playerRigidbody;

    void Awake ()
        {
            // Set up references.
            playerRigidbody = GetComponent <Rigidbody> ();
        }

    void FixedUpdate ()
        {
            // Store the input axes.
            // float h = Input.GetAxis("Horizontal");
            // float v = Input.GetAxis("Vertical");

            // // Move the player around the scene.
            // PieceMove (h, v);

            playerRigidbody.MovePosition(transform.position + (transform.forward * Time.deltaTime));
        }


        // void PieceMove (float h, float v)
        // {
        //     // Set the movement vector based on the axis input.
        //     movement.Set (h, 0f, v);
            
        //     // Normalise the movement vector and make it proportional to the speed per second.
        //     movement = movement.normalized * speed * Time.deltaTime;

        //     // Move the player to it's current position plus the movement.
        //     playerRigidbody.MovePosition (transform.position + movement);
        // }
}

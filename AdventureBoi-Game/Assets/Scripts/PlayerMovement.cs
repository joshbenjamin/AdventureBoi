using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody body;
    private float moveForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();        
    }

    // Update is called once per frame
    void Update()
    {
        CheckControls();
    }

    private void CheckControls()
    {
        // Right
        if(Input.GetKey(KeyCode.D))
        {
            MovePlayer(1 * moveForce, 0);
        }
        //Left
        if (Input.GetKey(KeyCode.A))
        {
            MovePlayer(-1 * moveForce, 0);
        }
        // Up
        if (Input.GetKey(KeyCode.W))
        {
            MovePlayer(0, 1 * moveForce);
        }
        // Down
        if (Input.GetKey(KeyCode.S))
        {
            MovePlayer(0, -1 * moveForce);
        }
    }

    private void MovePlayer(float horizontal, float vertical)
    {
        body.AddForce(new Vector3(horizontal, 0, vertical));
    }
}

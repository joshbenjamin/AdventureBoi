using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 startPosition;

    private Rigidbody body;
    private float moveForce = 10f;

    private bool hasTouchedMove = false;
    private Vector2 initialTouchPointMove = new Vector2();
    private Vector2 nextTouchPointMove = new Vector2();
    private float moveDifferenceMultiplier = 5f;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        startPosition = body.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //CheckPCControls();
        CheckPhoneControls();

        //Stop him from falling off
        if(body.transform.position.y < 0.5)
        {
            body.transform.position = startPosition;
        }
    }

    private void CheckPhoneControls()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.touches[i];
            Debug.Log("Position: " + CheckCoordinates(t.position));

            if (CheckCoordinates(t.position) == "RD")
            {
                if (hasTouchedMove == false)
                {
                    hasTouchedMove = true;
                    initialTouchPointMove = t.position;
                    nextTouchPointMove = t.position;
                }
                //We've already set anchor
                else
                {
                    nextTouchPointMove = t.position;
                }
            }
        }

        if(hasTouchedMove)
        {
            Vector2 move = CalculateMoveDifference();
            MovePlayer(move.x, move.y);
        }

        if(Input.touchCount == 0)
        {
            hasTouchedMove = false;
        }
    }

    private Vector2 CalculateMoveDifference()
    {
        float diffHor = nextTouchPointMove.x - initialTouchPointMove.x;
        float diffVer = nextTouchPointMove.y - initialTouchPointMove.y;
        float angleRad = Mathf.Atan2(diffVer, diffHor);

        float percDiffHor = (diffHor / Screen.width) * moveDifferenceMultiplier;
        float percDiffVer = (diffVer / Screen.height) * moveDifferenceMultiplier;

        return new Vector2(percDiffHor, percDiffVer);
    }

    private string CheckCoordinates(Vector2 touchPoint)
    {
        //Right
        if(touchPoint.x >= Screen.width/2)
        {
            //Up
            if(touchPoint.y >= Screen.height/2)
            {
                return "RU";
            }
            //Down
            else
            {
                return "RD";
            }
        }
        //Left
        else
        {
            //Up
            if (touchPoint.y >= Screen.height / 2)
            {
                return "LU";
            }
            //Down
            else
            {
                return "LD";
            }
        }
    }

    private void CheckPCControls()
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 startPosition;

    private Rigidbody body;
    private float moveSpeed = 0.05f;
    private float maxSpeed = 0.05f;

    private bool hasTouchedMove = false;
    private Vector2 initialTouchPointMove = new Vector2();
    private Vector2 nextTouchPointMove = new Vector2();
    private float moveDifferenceMultiplier = 5f;

    public GameObject analogPrefab;
    private GameObject activeAnalog;

    private Camera camera;
    private float constCameraDistance = 10f;

    private enum ScreenLocation
    {
        LU,
        LD,
        RU,
        RD
    }

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        camera = Camera.main;
        startPosition = body.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        CheckMouseControls();
        CheckPCControls();
        //CheckPhoneControls();

        //Stop him from falling off
        if(body.transform.position.y < 0.5)
        {
            body.transform.position = startPosition;
            hasTouchedMove = false;
        }
    }

    private void CheckMouseControls()
    {
        if (Input.GetMouseButton(0))
        {
            if (hasTouchedMove == false)
            {
                hasTouchedMove = true;
                initialTouchPointMove = Input.mousePosition;
                nextTouchPointMove = Input.mousePosition;

                //CalculateCameraDistance();

                Vector3 mousePos = new Vector3(initialTouchPointMove.x, initialTouchPointMove.y, 1);
                Vector3 instPos = camera.ScreenToWorldPoint(mousePos);

                instPos.z = 1;
                activeAnalog = Instantiate(analogPrefab, instPos, Quaternion.identity);
                
            }
            //We've already set anchor
            else
            {
                nextTouchPointMove = Input.mousePosition;
            }
        }
        else
        {
            hasTouchedMove = false;
            if (activeAnalog != null)
            {
                Destroy(activeAnalog);
            }
        }
        
        if (hasTouchedMove)
        {
            Vector2 move = CalculateMoveDifference();
            MovePlayer(move.x, move.y);

            activeAnalog.GetComponent<AnalogMoverScript>().MoveStick(move.x, move.y);
        }
    }

    private void CalculateCameraDistance()
    {
        Vector3 camPosition = camera.transform.position;
        Vector3 mousePositionPix = Input.mousePosition;
        mousePositionPix.z = Mathf.Abs(camera.transform.position.z);

        Vector3 mousePositionWorld = camera.ScreenToWorldPoint(mousePositionPix);
        Debug.Log(mousePositionWorld);
    }

    private void CheckPhoneControls()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.touches[i];
            Debug.Log("Position: " + CheckCoordinates(t.position));

            if (CheckCoordinates(t.position) == ScreenLocation.LD)
            {
                if (hasTouchedMove == false)
                {
                    hasTouchedMove = true;
                    initialTouchPointMove = t.position;
                    nextTouchPointMove = t.position;

                    activeAnalog = Instantiate(analogPrefab, new Vector3(initialTouchPointMove.x, initialTouchPointMove.y, -9f), Quaternion.identity);
                }
                //We've already set anchor
                else
                {
                    nextTouchPointMove = t.position;
                }
            }
            else
            {
                if(activeAnalog != null)
                {
                    Destroy(activeAnalog);
                }
            }
        }

        if(hasTouchedMove)
        {
            Vector2 move = CalculateMoveDifference();
            MovePlayer(move.x, move.y);

            activeAnalog.GetComponent<AnalogController>().Rotate(move.x, move.y);
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

    private ScreenLocation CheckCoordinates(Vector2 touchPoint)
    {
        //Right
        if(touchPoint.x >= Screen.width/2)
        {
            //Up
            if(touchPoint.y >= Screen.height/2)
            {
                return ScreenLocation.RU;
            }
            //Down
            else
            {
                return ScreenLocation.RD;
            }
        }
        //Left
        else
        {
            //Up
            if (touchPoint.y >= Screen.height / 2)
            {
                return ScreenLocation.LU;
            }
            //Down
            else
            {
                return ScreenLocation.LD;
            }
        }
    }

    private void CheckPCControls()
    {
        // Right
        if(Input.GetKey(KeyCode.D))
        {
            MovePlayer(1, 0);
        }
        //Left
        if (Input.GetKey(KeyCode.A))
        {
            MovePlayer(-1, 0);
        }
        // Up
        if (Input.GetKey(KeyCode.W))
        {
            MovePlayer(0, 1);
        }
        // Down
        if (Input.GetKey(KeyCode.S))
        {
            MovePlayer(0, -1);
        }
    }

    private void MovePlayer(float horizontal, float vertical)
    {
        body.transform.position += CapMoveSpeed(horizontal, vertical);
    }

    private Vector3 CapMoveSpeed(float hor, float ver)
    {
        Vector3 toMove = new Vector3(hor, 0, ver) * moveSpeed;
        if(toMove.magnitude > maxSpeed)
        {
            float multi = maxSpeed / toMove.magnitude;
            toMove = toMove * multi;
        }
        return toMove;
    }
}

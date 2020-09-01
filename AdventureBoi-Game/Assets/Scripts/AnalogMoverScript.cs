using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogMoverScript : MonoBehaviour
{
    public GameObject innerStick;
    public SpriteRenderer innerBaseRenderer;

    private Vector3 innerStickStartPos;
    private float circleRadius = 0.04f;
    private float stickMoveMultiplier = 0.1f;

    private Color baseColour;
    private float timeToColour = 2f;
    private float elapsedColourTime = 0f;

    private void Start()
    {
        baseColour = innerBaseRenderer.color;
        innerBaseRenderer.color = new Color(baseColour.r, baseColour.g, baseColour.b, 0f);

        innerStickStartPos = innerStick.transform.localPosition;
    }

    private void Update()
    {
        if(elapsedColourTime < timeToColour)
        {
            float temp = elapsedColourTime / timeToColour;
            innerBaseRenderer.color = new Color(baseColour.r, baseColour.g, baseColour.b, temp);
            elapsedColourTime += Time.deltaTime;
        }
    }

    public void MoveStick(float x, float y)
    {
        innerStick.transform.localPosition = CapMoveDistance(new Vector3(x, y, 0));
    }

    private Vector3 CapMoveDistance(Vector3 val)
    {
        val = val * stickMoveMultiplier;
        Vector3 difference = val - innerStickStartPos;
        if(difference.magnitude > circleRadius)
        {
            float multi = circleRadius / difference.magnitude;
            val = val * multi;
        }
        return val;
    }
}

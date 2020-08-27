using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalogController : MonoBehaviour
{
    private Transform transform;
    private float rotationMultiplier = 10f;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        transform.rotation = new Quaternion(270f, 0f, 0f, 1f);
    }

    public void Rotate(float x, float z)
    {
        transform.eulerAngles = new Vector3(x*rotationMultiplier, 0, z*rotationMultiplier);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    public float SpinSpeed = 20f;
    
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0,SpinSpeed*Time.deltaTime,0));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {

    public float amplitude = 2.5f;
    public float frequency = 0.3f;
 
    Vector3 posOffset = new Vector3 ();
    Vector3 tempPos = new Vector3 ();
    void Start () {
        posOffset = transform.position;
    }
     

    void Update () {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin (Time.fixedTime * Mathf.PI * frequency) * amplitude;
 
        transform.position = tempPos;
    }
}

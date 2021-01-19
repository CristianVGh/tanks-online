
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileMove : MonoBehaviour
{   
    void Start(){
        FloatingTextController.Initialize();
    }
    
    void Update()
    {
        float power = - GameStats.Instance.GetPower();
        
        if (power != 0)
            this.transform.Translate(Time.deltaTime * power, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {   Debug.Log("Was hit " + collision.transform.name);
        GameStats.WasHit(collision.transform.tag, collision.transform.name);
        gameObject.SetActive(false);        
    }
}

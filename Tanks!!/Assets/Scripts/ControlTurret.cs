using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlTurret : MonoBehaviour
{
    public static ControlTurret Instance {set; get;}
    int value_used_to_increment_angle =  3;
    int value_used_to_decrement_angle = -3;  
    

    float p1_angle_value = 0;
    float p2_angle_value = 0;
    float p1_max_value = 0.255F;
    float p1_min_value = 0.094F;

    float p2_max_value = -0.255F;
    float p2_min_value = -0.094F;

    public GameObject turret1;
    public GameObject turret2;
    private GameObject theTurret;

    private Client client;

    public void Start(){
        Instance = this;
        client = FindObjectOfType<Client>();
    }

    public void Plus_Button()
    {
        p1_angle_value = turret1.transform.rotation.x;
        p2_angle_value = turret2.transform.rotation.x;
       
        if (GameStats.Instance.player1Turn){ 
            if (p1_angle_value < p1_max_value){
                client.Send("CTMV|" + turret1.name + "|1"); 
            }
        } else {
             if (p2_angle_value > p2_max_value){
                client.Send("CTMV|" + turret2.name + "|1"); 
            }
        }
         
    }

    public void Minus_Button()
    {
        p1_angle_value = turret1.transform.rotation.x;
        p2_angle_value = turret2.transform.rotation.x;
        
        if (GameStats.Instance.player1Turn){ 
            if (p1_angle_value > p1_min_value){
                client.Send("CTMV|" + turret1.name + "|0"); 
            }
        } else {    
            if (p2_angle_value < p2_min_value){ 
                client.Send("CTMV|" + turret2.name + "|0"); 
            }
        } 
    }

    public void MoveTurret(GameObject gb, bool inc){
        if(inc){
            gb.transform.Rotate(value_used_to_increment_angle, 0, 0);
        } else {
            gb.transform.Rotate(value_used_to_decrement_angle, 0, 0);
        }
    }


  
}

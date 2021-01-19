using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Movement Instance {set; get;}
    public GameObject tank1;

    public GameObject tank2;
    private GameObject tank;

    float current_distance = 0.0F;
    float new_right_distance = 0.0F;
    float new_left_distance = 0.0F;
    float distance = 1.0F;

    bool move_right = false;
    bool move_left = false;

    private Client client;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        client = FindObjectOfType<Client>();
    }

    // Update is called once per frame
    void Update()
    {      
        Set_Tank(); 
        current_distance = tank.transform.position.x;

        Move_Right();    
        Move_Left();   
       
    }

     public void Move_Right_Button()
    {   
        client.Send("CMOV|" + tank.name + "|1");
    }

     public void Move_Left_Button()
    { 
        client.Send("CMOV|" + tank.name + "|0");
    }

    public void Begin_Move(GameObject gb, string direction){
        tank = gb;
        current_distance = tank.transform.position.x; 
        if(direction.Equals("1"))
        {
            new_right_distance = current_distance + distance;
            move_right = true;
        }
        else 
        {
            new_left_distance = current_distance - distance;
            move_left = true;
        }
    }

    public void Move_Right()
    {
        if(current_distance > new_right_distance)
        {
            move_right = false;
        }
        if (move_right == true)
        {
            if(GameStats.Instance.player1Turn)
                tank.transform.Translate(0, 0, Time.deltaTime);
            else 
                tank.transform.Translate(0, 0, -Time.deltaTime);

            move_left = false;
        }   
    }

    public void Move_Left()
    {
        if (current_distance < new_left_distance)
        {
            move_left = false;
        }

        if(move_left == true)
        {
            if(GameStats.Instance.player1Turn)
                tank.transform.Translate(0, 0, - Time.deltaTime );
            else
                tank.transform.Translate(0, 0,  Time.deltaTime ); 

            move_right = false;
        }
    }

    private void Set_Tank()
    {
        if(GameStats.Instance.player1Turn)
            tank = tank1;
        else 
            tank = tank2;
    }

   
}

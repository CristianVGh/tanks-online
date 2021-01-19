using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance {set; get;}
    private static int player1HP = 100;
    private static int player2HP = 100;
    private static int turn = 1;
    public static float power;
    public bool isFirst;
    public bool player1Turn;

    public GameObject shell;
    public Slider slider;

    public GameObject barrel1;
    public GameObject barrel2;
    private GameObject spawnPlace;

    public  Image foregroundImage1;
    public  Image foregroundImage2; 

    public GameObject bomb;

    public Transform ChatMessageContainer;
    public GameObject messagePrefab;

    public Text p1Name;
    public Text p2Name;

    private Client client;

    public void Start(){
        Instance = this;
        player1Turn = true;
        client = FindObjectOfType<Client>();
        client.Send("CNM|");
    }

    private void Update(){
        DrawBars();
    }

     public void Fire(){
        power = slider.value;

        if(player1Turn)
            spawnPlace = barrel1;
        else
            spawnPlace = barrel2;

        float randValue = UnityEngine.Random.value;
        string msg = "CSPWN";
        msg += "|" + spawnPlace.name;
        msg += "|" + power.ToString();
        msg += "|" + randValue.ToString();
        client.Send(msg);      
   }

    public void SpawnBullet(Transform t){
        GameObject shellInstance = Instantiate(shell, t.position, t.rotation) as GameObject;
        Destroy(shellInstance, 5);
    }

    public void SpawnBomb(float randValue){
        if (randValue < .45f) {
            Vector3 bomb_position;
            GameObject bombInstance;

            if(player1Turn){
                bomb_position = new Vector3(8, 8, 12.9f);
                bombInstance = Instantiate(bomb, bomb_position, Quaternion.identity) as GameObject;
                bombInstance.tag = "Player2";

            }
            else {
                bomb_position = new Vector3(-8, 8, 12.9f);
                bombInstance = Instantiate(bomb, bomb_position, Quaternion.identity) as GameObject;
                bombInstance.tag = "Player1";
            }

            Destroy(bombInstance, 10);
        }
    }

    private void DrawBars(){
        foregroundImage1.fillAmount = (float)player1HP / 100;
        foregroundImage2.fillAmount = (float)player2HP / 100;         
    }

    public void CheckVictory(){
        if (player1HP <= 0){

        } 
        
        if (player2HP <= 0){

        }
    }

    public static void WasHit(string tag, string name){
        int damage = 0;
        GameObject tank_obj = new GameObject();
        Transform tf = tank_obj.transform;
        switch (name){
            case "Abrams_BF3":
                damage = 15;
            break;
            case "Abrams_BF3 (1)":
                damage = 15;
            break;
            case "Cylinder":
                damage = 15;
            break;
            case "Barrel1":
                damage = 15;
            break;
            case "Barrel2":
                damage = 15;
            break;
            case "Manual_MG":
                damage = 20;
            break;
            case "RC_MG":
                damage = 20;
            break;
            case "Turret":
                damage = 20;
            break;
            case "Body":
                damage = 10;
            break;
            case "Reactive_Armor":
                damage = 10;
            break;
            case "Track":
                damage = 10;
            break;
            case "Bomb(Clone)":
                damage = 40;
            break;
       }

        if(tag == "Player1"){
            player1HP -= damage;
            tank_obj = GameObject.Find("Abrams_BF3");
        }

         if(tag == "Player2"){
            tank_obj = GameObject.Find("Abrams_BF3 (1)");
            player2HP -= damage;
        }

        if(damage > 0){
            tf = tank_obj.transform;
            FloatingTextController.CreateFloatingText(damage.ToString(), tf);        
        }
    }

    public void ChatMessage(string message){
        GameObject go = Instantiate(messagePrefab) as GameObject;
        go.transform.SetParent(ChatMessageContainer);
        go.GetComponentInChildren<Text>().text = message;
        Debug.Log("Mesajul este " + message);
    }

    public void SendChatMessage(){
        InputField in_field = GameObject.Find("MessageInput").GetComponent<InputField>();
        if(in_field.text == "")
            return;
        Debug.Log("Vom trimite " + in_field.text);
        client.Send("CMSG|" + in_field.text);
        in_field.text = "";
    }
    
    public void SetNames(string p1, string p2){
        p1Name.text = p1;
        p2Name.text = p2;
    }
    public void SetPower(float p){
        power = p;
    }

    public float GetPower(){
        return power;
    }

    public void NextTurn(){
        turn ++;
        player1Turn = !player1Turn;
    }


}

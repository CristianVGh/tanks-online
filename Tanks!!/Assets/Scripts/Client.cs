using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.IO;
using System;
using UnityEngine;

public class Client : MonoBehaviour {
	public string clientName;
	public bool isHost;
	private bool socketReady;
	private TcpClient socket;
	private NetworkStream stream;
	private StreamWriter writer;
	private StreamReader reader;

	private List<GameClient> players = new List<GameClient>();

	private void Start(){
		DontDestroyOnLoad(gameObject);
	}

	public bool ConnectToServer(string host, int port){
		if(socketReady){
			return false;
		}

		try{
			socket = new TcpClient(host, port);
			stream = socket.GetStream();
			writer = new StreamWriter(stream);
			reader = new StreamReader(stream);

			socketReady = true;
		} catch(Exception e){
			Debug.Log("Socket error" + e.Message);
		}
		
		return socketReady;
	}

	private void Update(){
		if(socketReady){
			if(stream.DataAvailable){
				string data = reader.ReadLine();
				if(data != null){
					OnIncomingData(data);
				}
			}
		}
	}
	
	//send message
	public void Send(string data){
		if(!socketReady)
			return;
		
		writer.WriteLine(data);
		writer.Flush();
	}

	//read messages
	private void OnIncomingData(string data){
		string[] aData = data.Split('|');
		//Debug.Log("Client: " + data);

		switch(aData[0]){
			case "SWHO":
				for(int i = 1; i < aData.Length - 1; i++){
					UserConnected(aData[i], false);
				}
				Send("CWHO|" + clientName + "|" + ((isHost)?1:0).ToString());
			break;

			case "SCNN":
				UserConnected(aData[1], false);
			break;

			case "SSPWN":
				GameObject gb = GameObject.Find(aData[1]);
				Transform t = gb.transform;
				GameStats.Instance.SetPower(float.Parse(aData[2]));
				GameStats.Instance.SpawnBullet(t);
				GameStats.Instance.SpawnBomb(float.Parse(aData[3]));
				GameStats.Instance.NextTurn();
			break;
			
			case "STMV":
				GameObject tr = GameObject.Find(aData[1]);
				bool inc = ((aData[2] == "1") ? true : false);
				ControlTurret.Instance.MoveTurret(tr, inc);
			break;

			case "SMOV":
				GameObject mv = GameObject.Find(aData[1]);
				Movement.Instance.Begin_Move(mv, aData[2]);
			break;

			case "SMSG":
				Debug.Log("Am primit si ama preg de scris");
				GameStats.Instance.ChatMessage(aData[1]);
			break;

			case "SNM":
				GameStats.Instance.SetNames(aData[1], aData[2]);
			break;
		}
	}

	private void UserConnected(string name, bool host){
		GameClient c = new GameClient();
		c.name = name;

		players.Add(c);

		if(players.Count == 2)
			GameManager.Instance.StartGame();
	}

	private void OnApplicationQuit(){
		CloseSocket();
	}

	private void OnDisable(){
		CloseSocket();
	}
	private void CloseSocket(){
		if(!socketReady)
			return;

			writer.Close();
			reader.Close();
			socket.Close();
			socketReady = false;
	}
}

public class GameClient{
	public string name;
	public bool isHost;
}

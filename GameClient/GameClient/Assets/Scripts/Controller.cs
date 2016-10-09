using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SocketIO;

public class Controller:MonoBehaviour {
	public string name;
	public SocketIOComponent socket;

	// Use this for initialization
	void Start() {
		// Attempt to connect to the server
		StartCoroutine(ConnectToServer());

		// Create bindings to different server events
		// these are brodcast messages comming from the server
		socket.On("USER_CONNECTED", OnUserConnected);
		socket.On("PLAY", OnUserPlaying);
	}
	
	IEnumerator ConnectToServer() {
		// Send a message to the server indicating a new user connection
		yield return new WaitForSeconds(0.5f);
		socket.Emit("USER_CONNECT");

		// Wait a second for the user to get added to the server,
		// then brodcast the new user data to the server using PLAY
		yield return new WaitForSeconds(1f);
		Dictionary<string, string> data = new Dictionary<string, string>();
		data["name"] = name;
		Vector3 position = gameObject.transform.position;
		data["position"] = position.x + ", " + position.y + ", " + position.z;

		// Send a message to the server that a new user has 
		socket.Emit("PLAY", new JSONObject(data));
	}

	private void OnUserConnected(SocketIOEvent evt) {
		Debug.Log("Server -> USER_CONNECTED: " + evt.data);
	}

	private void OnUserPlaying(SocketIOEvent evt) {
		Debug.Log("Server -> PLAY: " + evt.data);
	}
}

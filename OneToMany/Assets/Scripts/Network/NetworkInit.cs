using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkInit : MonoBehaviour {
	public string mainScene = "Main";
	public string host = "localhost";
	int port = 8080;
	public bool useNat;
	void OnGUI() {
		if(GUILayout.Button("Start Server")) {
			Network.InitializeServer(5, port, useNat);
			SceneManager.LoadScene(mainScene);
		}

		host = GUILayout.TextField(host);
		if(GUILayout.Button("Connect Client")) {
			Network.Connect(host, port);
			SceneManager.LoadScene(mainScene);
		}
	}
}

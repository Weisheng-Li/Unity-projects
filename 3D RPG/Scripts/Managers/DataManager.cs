using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// import binary formatter
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class DataManager : MonoBehaviour, IGameManager {
	public ManagerStatus status { get; private set;}
	private string _filename;
	private NetworkService _network;

	public void Startup(NetworkService service) {
		Debug.Log ("Data Manager Starting...");
		_network = service;
		_filename = Path.Combine (Application.persistentDataPath, "game.dat");
		status = ManagerStatus.Started;
	}

	public void SaveGameState() {
		Dictionary<string, object> gameState = new Dictionary<string, object> ();

		gameState.Add ("Inventory",Managers.Inventory.GetData());
		gameState.Add ("health", Managers.Player.health);
		gameState.Add ("maxHealth", Managers.Player.maxHealth);
		gameState.Add ("maxLevel", Managers.Mission.maxLevel);
		gameState.Add ("curLevel", Managers.Mission.curLevel);

		// file.create will create a binary file
		FileStream stream = File.Create (_filename);
		BinaryFormatter formatter = new BinaryFormatter ();
		formatter.Serialize (stream, gameState);
		stream.Close ();
	}

	public void LoadGameState() {
		if (!File.Exists (_filename)) {
			Debug.Log ("No Saved Game");
			return;
		}

		Dictionary<string, object> gameState;
		BinaryFormatter formatter = new BinaryFormatter ();
		FileStream stream = File.Open (_filename, FileMode.Open);
		gameState = formatter.Deserialize (stream) as Dictionary<string, object>;
		stream.Close();

		Managers.Inventory.UpdateData ((Dictionary<string, int>)gameState ["Inventory"]);
		Managers.Player.UpdateData ((int)gameState ["health"], (int)gameState ["maxHealth"]);
		Managers.Mission.UpdateData ((int)gameState ["curLevel"], (int)gameState ["maxLevel"]);
		Managers.Mission.RestartCurrent ();
	}
}

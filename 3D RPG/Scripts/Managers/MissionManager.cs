﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MissionManager : MonoBehaviour,IGameManager {
	public ManagerStatus status { get; private set;}

	public int maxLevel{ get; private set;}
	public int curLevel{ get; private set;}

	private NetworkService _network;

	public void Startup(NetworkService service) {
		Debug.Log ("Mission Manager Started...");

		_network = service;

		UpdateData (0, 1);

		status = ManagerStatus.Started;
	}

	public void GoToNext() {
		if (curLevel < maxLevel) {
			curLevel++;
			string name = "Level" + curLevel;
			Debug.Log ("Loading " + name);
			SceneManager.LoadScene (name);
		} else {
			Debug.Log("Last level");
			Messenger.Broadcast (GameEvent.GAME_COMPLETE);
		}
	}

	public void ReachObjective() {
		Messenger.Broadcast (GameEvent.LEVEL_COMPLETED);
	}

	public void RestartCurrent() {
		string name = "level" + curLevel;
		Debug.Log ("loading " + name);
		SceneManager.LoadScene (name);
	}

	// provide access to private data when loading the player's progress.
	public void UpdateData(int curLevel, int maxLevel) {
		this.curLevel = curLevel;
		this.maxLevel = maxLevel;
	}
}

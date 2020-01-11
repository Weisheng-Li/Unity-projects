using UnityEngine;
using System.Collections;

public class PlayerCharacter : MonoBehaviour {
	private int _health;

	void Start() {
		_health = 5;
	}

	public void Hurt(int damage) {
		Managers.Player.ChangeHealth (-damage);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUi : MonoBehaviour {

	void StartCountTime () {

		GameObject.Find ("GameManager").GetComponent<GameManager> ().CountStart ();

	}
}

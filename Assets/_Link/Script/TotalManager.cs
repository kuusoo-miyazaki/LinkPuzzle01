using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TotalManager : MonoBehaviour {

	public static string PlayingStage;


	public static TotalManager Instance {
		get;
		private set;
	}

	void Awake () {

		if (Instance != null) {
			Destroy (this.gameObject);
			return;
		}

		Instance = this;

		DontDestroyOnLoad (this.gameObject);

	}
	

	public static void StartStage () {
		
		SceneManager.LoadSceneAsync ("Main");

	}
}

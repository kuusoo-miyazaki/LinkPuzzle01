using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	

	public void GoTitle () {
		
		SceneManager.LoadScene ("Title");
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUi : MonoBehaviour {



	void LateUpdate () {
		transform.rotation = Camera.main.transform.rotation;
	}
}

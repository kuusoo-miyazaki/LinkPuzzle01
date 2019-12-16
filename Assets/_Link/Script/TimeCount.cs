using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCount : MonoBehaviour {

	public float CountSpeed;

	public GameObject CountUi;

	public int countNum;

	public int CountNum{

		get{return countNum;}

		set{
			countNum = value;
			CountUi.GetComponent<Text> ().text = countNum.ToString();

			if (countNum == 0) {
				GetComponent<GameManager> ().EndUi.SetActive (true);
				StopCoroutine ("CountTime");
				GetComponent<ControllManager> ().GameDefeat = true;
			}
		}
	}


	IEnumerator CountTime () {

		int cnt = 0;

		while (true) {

			cnt++;

			if (cnt > 100f / CountSpeed) {

				CountNum--;

				cnt = 0;
			}

			yield return new WaitForSeconds (0.01f);

		}
	}
}

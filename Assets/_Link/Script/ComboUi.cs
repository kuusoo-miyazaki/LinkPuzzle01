using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboUi : MonoBehaviour {

	public bool IsCombo;

	public float NowRemaining;

	public float ComboSpeed;

	public float SpeedRate;

	public float MaxSpeed;

	public GameObject ComboRemainingUi;

	public GameObject ComboNumUi;

	public int comboNum;

	public int ComboNum{

		get{return comboNum;}

		set{
			comboNum = value;
			ComboNumUi.GetComponent<Text> ().text = comboNum.ToString();

			if (comboNum == 0) {
				IsCombo = false;
				StopCoroutine ("ComboRemaining");
			} else {

				NowRemaining = 1000f;

				if (IsCombo == false) {
					IsCombo = true;
					StartCoroutine ("ComboRemaining");
				}
			}
		}
	}


	public void StartCombo () {
		StartCoroutine ("ComboRemaining");
	}


	IEnumerator ComboRemaining(){

		float Reduce;

		while (true) {

			Reduce = ComboSpeed + (ComboNum / SpeedRate);

			if (Reduce >= MaxSpeed)
				Reduce = MaxSpeed;

			NowRemaining -= Reduce;

			ComboRemainingUi.GetComponent<Image> ().fillAmount = NowRemaining / 1000f;

			if (NowRemaining <= 0) {

				NowRemaining = 0;

				ComboNum = 0;
				yield return null;
			}

			yield return new WaitForSeconds (0.01f);
		}
	}

}

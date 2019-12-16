using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

	static GameManager GM;
	static ControllManager CM;
	static ComboUi CU;

	void Start () {

		GM = GetComponent<GameManager> ();
		CM = GetComponent<ControllManager> ();
		CU = GetComponent<ComboUi> ();
	}

	public static void CalculationScore(int erasedNum){

		//基礎スコア×消去数
		float addScore = CM.GemToScore　* erasedNum;

		//チェーン倍率
		addScore *= CM.ChainToScoreRate * (1f + (float)CM.ChainNum / 100f);
		//print("Add Score1 +" + addScore);
		//コンボ倍率
		addScore *= CM.ComboToScoreRate * (1f + (float)CU.ComboNum / 100f);
		//print("Add Score2 +" + addScore);

		//スコア加算
		GM.Score += (int)addScore;
		//print("Add Score3 +" + addScore);
	}
}

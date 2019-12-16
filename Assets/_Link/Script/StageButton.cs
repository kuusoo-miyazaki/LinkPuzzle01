using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageButton : MonoBehaviour {

	public string AreaNum;

	public string StageNum;

	public GameObject Text_AreaNum;

	public GameObject Text_StageNum;


	public void SetUp () {



	}
	

	public void OnStageButton () {

		TotalManager.PlayingStage = StageNum;

        GameObject _symbol = Instantiate(Resources.Load("LoadSymbol"), transform.position, Quaternion.identity) as GameObject;

        _symbol.transform.SetParent(GameObject.Find("Canvas").transform);

        _symbol.transform.localPosition = new Vector3(0, 0, 0);

        SceneManager.LoadSceneAsync ("Main");

	}
}

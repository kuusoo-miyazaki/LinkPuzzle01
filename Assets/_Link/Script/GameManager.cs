using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	TimeCount timeCount;

	public int score;

	public int Score{

		get{return score;}

		set{
			score = value;
			ScoreUi.GetComponent<Text> ().text = score.ToString ();
		}
	}

	public int coin;

	public int Coin{

		get{return coin;}

		set{
			coin = value;
			CoinUi.GetComponent<Text> ().text = coin.ToString ();
		}
	}

	public int chain;

	public int Chain{

		get{return chain;}

		set{
			chain = value;

			ChainUi.SetActive (true);
			ChainUi.GetComponent<Text> ().text = chain.ToString ();
			ChainUi.GetComponent<Animator> ().Play ("Activate",0,0.0f);
		}
	}

	public float exPoint;

	public float ExPoint{

		get{return exPoint;}

		set{
			exPoint = value;

			if (exPoint >= controllManager.MaxExPoint) {
				
				exPoint = controllManager.MaxExPoint;

				controllManager.IsExMode = true;

                ExSign.SetActive(true);

				StartCoroutine ("CountExMode",controllManager.Speed_DecayExPoint);
			}

			ExGauge.GetComponent<Image> ().fillAmount = value / controllManager.MaxExPoint;
		}
	}

	public float[] skillPoint = new float[4];

	public float SkillPoint_R{

		get{return skillPoint[0];}

		set{
			skillPoint[0] = value;

            if (skillPoint[0] >= controllManager.MaxSkillPoint[0])
            {
                skillPoint[0] = controllManager.MaxSkillPoint[0];

                controllManager.PermissionSkillUse[0] = true;

                SkillGauge[0].transform.parent.transform.parent.transform.Find("FullSign").gameObject.SetActive(true);
            }
			
			SkillGauge[0].GetComponent<Image> ().fillAmount = value / controllManager.MaxSkillPoint[0];
            
		}
	}

	public float SkillPoint_G{

		get{return skillPoint[1];}

		set{
			skillPoint[1] = value;

            if (skillPoint[1] >= controllManager.MaxSkillPoint[1])
            {
                skillPoint[1] = controllManager.MaxSkillPoint[1];

                controllManager.PermissionSkillUse[1] = true;

                SkillGauge[1].transform.parent.transform.parent.transform.Find("FullSign").gameObject.SetActive(true);
            }

			SkillGauge[1].GetComponent<Image> ().fillAmount = value / controllManager.MaxSkillPoint[1];
		}
	}

	public float SkillPoint_B{

		get{return skillPoint[2];}

		set{
			skillPoint[2] = value;

            if (skillPoint[2] >= controllManager.MaxSkillPoint[2])
            {
                skillPoint[2] = controllManager.MaxSkillPoint[2];

                controllManager.PermissionSkillUse[2] = true;

                SkillGauge[2].transform.parent.transform.parent.transform.Find("FullSign").gameObject.SetActive(true);
            }

			SkillGauge[2].GetComponent<Image> ().fillAmount = value / controllManager.MaxSkillPoint[2];
		}
	}

	public float SkillPoint_Y{

		get{return skillPoint[3];}

		set{
			skillPoint[3] = value;

            if (skillPoint[3] >= controllManager.MaxSkillPoint[3])
            {
                skillPoint[3] = controllManager.MaxSkillPoint[3];

                controllManager.PermissionSkillUse[3] = true;

                SkillGauge[3].transform.parent.transform.parent.transform.Find("FullSign").gameObject.SetActive(true);
            }

			SkillGauge[3].GetComponent<Image> ().fillAmount = value / controllManager.MaxSkillPoint[3];
		}

	}




	public GameObject ScoreUi;

	public GameObject CoinUi;

	public GameObject ChainUi;

	public GameObject StartUi;

	public GameObject EndUi;

    public GameObject ClearUi;

	public GameObject ExGauge;

    public GameObject ExSign;

    public string[] SkillID = new string[4];

	public GameObject[] SkillGauge = new GameObject[4];

	public GameObject gemGenerator;

	public GameObject EnemyGroup;

   


	ControllManager controllManager;


	void Start () {

		controllManager = GetComponent<ControllManager> ();

		StartingProcess ();
	}

	void StartingProcess(){

		StartUi.GetComponent<Animator> ().enabled = true;

		gemGenerator.GetComponent<GemGenerator> ().Del ();

		gemGenerator.GetComponent<GemGenerator>().GenerateGem ();

	}

	public void CountStart(){

		timeCount = GetComponent<TimeCount> ();

        controllManager.GameStarted = true;

		timeCount.CountUi.GetComponent<Text> ().text = timeCount.CountNum.ToString();

		timeCount.StartCoroutine ("CountTime");

	}

	public void EnemySetUp(){

		GameObject _enemy = Instantiate (Resources.Load ("Stage/" + (TotalManager.PlayingStage).ToString ()), transform.position, Quaternion.identity) as GameObject;

		_enemy.transform.parent = EnemyGroup.transform;

        if (_enemy.GetComponent<StageData>().ClearByKillBoss == true)
            controllManager.ClearByKillBoss = true;
	}

	public void ReturnHome(){

		GameObject _symbol = Instantiate (Resources.Load ("LoadSymbol"), transform.position, Quaternion.identity) as GameObject;

        _symbol.transform.SetParent(GameObject.Find("Canvas").transform);

        _symbol.transform.localPosition = new Vector3(0, 0, 0);

		SceneManager.LoadSceneAsync ("Home");

	}

	IEnumerator CountExMode (float speed) {

		while (true) {
			
			ExPoint -= speed;

			if (ExPoint <= 0) {
				
				ExPoint = 0;

                ExSign.SetActive(false);

				controllManager.IsExMode = false;

				StopCoroutine ("CountExMode");
				yield return null;
			}

			yield return new WaitForSeconds (0.01f);

		}
	}
}

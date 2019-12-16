using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GemParameter : MonoBehaviour {

	public bool IsSelected;

	public bool IsDead;

	public bool IsEnemy;

    public bool IsBoss;

	public bool IsBomb;

    GameManager gameManager;

    ControllManager controllManager;




	public enum GemColor
	{
		Red,
		Green,
		Blue,
		Yellow,
	}

	public GemColor G_color;

	public int MaxLife;

	int life;
	public int Life{

		get{ return life; }

        set {

            if (value <= 0) {

                life = 0;

                if (IsBoss == true && controllManager.ClearByKillBoss == true && controllManager.GameStarted == true)
                {
                    GameObject.Find("GameManager").GetComponent<TimeCount>().StopCoroutine("CountTime");
                    controllManager.GameClear = true;
                    gameManager.ClearUi.SetActive(true);
                }
            }

            life = value;

            if (LifeGauge != null) {
				LifeGauge.GetComponent<Image> ().fillAmount = (float)life / (float)MaxLife;
			}
        }
	}

	public GameObject LifeGauge;

	void Awake(){

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        controllManager = GameObject.Find("GameManager").GetComponent<ControllManager>();

        Life = MaxLife;
	}
}

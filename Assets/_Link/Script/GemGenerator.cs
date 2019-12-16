using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGenerator : MonoBehaviour {

	public GameObject StageData;

	public GameObject[] GemPrefab = new GameObject[4];

	public GameObject[] BombPrefab = new GameObject[4];

	public GameObject[] SpawnPos = new GameObject[4];

	public static List<GameObject> ExistGems = new List<GameObject>();

	public static List<GameObject> ErasedGems = new List<GameObject>();

	public int MaxNumber;

	public static int ExistNum;

	public float GenerateSpeed;

	public int SpawnedNumber;

	public int NowSpawnPos;

	public GameObject GemsFolder;

	public Vector3 BombGeneratePos;

	bool SetUpEnemy;





	void Awake () {
        ErasedGems.Clear();
	}
	

	public void GenerateGem () {

		for (int num = 0; ExistNum < MaxNumber; num++) {
			
			ExistNum++;

			Invoke ("GemSpawn",((float)num) / (10 * GenerateSpeed));

			if (SetUpEnemy == false && ExistNum == MaxNumber) {

				GameObject.Find("GameManager").GetComponent<GameManager>().Invoke("EnemySetUp",((float)num) / (10 * GenerateSpeed));
				SetUpEnemy = true;
			}
		}
	}

	void GemSpawn(){

		NowSpawnPos++;
		if (NowSpawnPos >= 4)
			NowSpawnPos = 0;

		GameObject gem = Instantiate (GemPrefab[Random.Range (0, GemPrefab.Length)], SpawnPos[NowSpawnPos].transform.position, Quaternion.identity) as GameObject;

		gem.name = SpawnedNumber.ToString ();
		gem.transform.parent = GemsFolder.transform;
		SpawnedNumber++;

		ExistGems.Add (gem);

	}

	public void GenerateBomb(){
		GameObject _bomb = Instantiate (BombPrefab [Random.Range (0, BombPrefab.Length - 1)], BombGeneratePos, Quaternion.identity) as GameObject;
		_bomb.transform.parent = GemsFolder.transform;
	}

	public void Del(){
		foreach (GameObject d_gem in ExistGems) {
			Destroy (d_gem);
		}
		ExistGems = new List<GameObject>();
		ExistNum = 0;
		ControllManager.NowLinkNumber = 0;
	}
}

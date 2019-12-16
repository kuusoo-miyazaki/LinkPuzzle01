using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControllManager : MonoBehaviour {

	public static int NowLinkNumber;

	public int MinLinkNumber;

	public float MinLinkDistance;

	public float EraseSpeed;

	public float BombRange;

	public int BombDamage;

	GameManager gameManager;

	GemGenerator gemGenerator;

	ComboUi comboUi;

	GameObject LastSelectGem;

    public bool GameStarted;

    public bool GameDefeat;

    public bool GameClear;

    public bool ClearByKillBoss;

	public bool IsExMode;

    public bool IsProcessingSkill;

	public bool IsLongTap;

	public bool IsLockEnemy;

	public bool IsLockFirstBomb;

	public bool IsLockSecondBomb;

	bool IsBombDamage;

	public int ErasedNum;

	public int ChainNum;

	public int ComboNum;

	GameObject DeadEnemy;

	public float GemToScore;

	//スコア・コイン関連↓

	public float GemToCoin;

	public float GemToBoostPoint;

	public float ChainToCoinRate;

	public float ChainToScoreRate;

	public float ComboToScoreRate;


	//スコア関連↑

	public float MinComboSpeed;

	public float MaxComboSpeed;

	public float ComboSpeedUp;

	public GameObject ChainLine;

    GameObject EffectGroup;

    List<Vector3> EffectPos = new List<Vector3>();

    bool PlayDeleteSE;

	[HideInInspector]
	public List<GameObject> ChainableGems = new List<GameObject>();

	public List<GameObject> CheckedGems = new List<GameObject>();

	enum GemColor
	{
		Red,
		Green,
		Blue,
		Yellow,
	}

	GemColor _G_color;

	public int NumForGenerateBomb;//ボム生成に必要な消去数

	public float AddExPoint; 

	public float MaxExPoint;//EXゲージの最大値

	public float Speed_DecayExPoint;//エクシードモード中のEXゲージ減衰速度

	public int NowLinkNumInExChain;


	public float[] MaxSkillPoint = new float[4];//加算値は１で統一する

    public bool[] PermissionSkillUse = new bool[4];


    AudioSource audioSource;


	void Start () {

		gameManager = GetComponent<GameManager> ();

		gemGenerator = GameObject.Find ("GemGenerator").GetComponent<GemGenerator> ();

		comboUi = GetComponent<ComboUi> ();

        EffectGroup = GameObject.Find("Effects");

        audioSource = GetComponent<AudioSource>();

	}
	

	void Update () {

		if (Input.GetMouseButton (0)) {

			IsLongTap = true;

            if (GameDefeat)
                return;

            if (GameClear)
                return;

			if (IsLockEnemy)
				return;

			if (IsLockSecondBomb)
				return;

            if (IsProcessingSkill)
                return;

			Ray ray = new Ray ();

			RaycastHit hit = new RaycastHit ();

			ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray.origin, ray.direction, out hit, Mathf.Infinity)) {

				//■２個目の選択がボムの場合
				if (hit.collider.gameObject.CompareTag ("Gem") && IsLockFirstBomb == true && hit.collider.gameObject.GetComponent<GemParameter>().IsSelected == false
					&& hit.collider.gameObject.GetComponent<GemParameter>().IsBomb) {

					SelectGem (hit.collider.gameObject);

					IsLockSecondBomb = true;

					return;
				}

				if (hit.collider.gameObject.CompareTag ("Gem") && hit.collider.gameObject.GetComponent<GemParameter>().IsSelected == false 
					&& hit.collider.gameObject.GetComponent<GemParameter>().IsDead == false) {

					if (NowLinkNumber < 1) {
						//■最初の選択がジェムの場合
						if(hit.collider.gameObject.GetComponent<GemParameter>().IsEnemy == false)
							SelectGem (hit.collider.gameObject);
						//■最初の選択がボムの場合
						if (hit.collider.gameObject.GetComponent<GemParameter> ().IsBomb == true)
							IsLockFirstBomb = true;

					} else {
						
						//■2個目以降の選択がジェムの場合
						Collider[] ChainableGems = Physics.OverlapSphere (LastSelectGem.transform.position, MinLinkDistance);

						foreach (Collider same in ChainableGems) {
							
							if (same.gameObject == hit.collider.gameObject) {
								
								//■2個目以降の選択がジェムの場合
								if (IsLockFirstBomb == false) {

									if (hit.collider.gameObject.GetComponent<GemParameter> ().G_color == LastSelectGem.GetComponent<GemParameter> ().G_color) {

									SelectGem (hit.collider.gameObject);

									return;

								} else {
										if(IsExMode == true && NowLinkNumInExChain >= MinLinkNumber){//エクシードチェーン判定

											SelectGem (hit.collider.gameObject);

											NowLinkNumInExChain = 1;

											return;
										}
								}
								}

								//■2個目の選択がエネミーの場合
								if (hit.collider.gameObject.GetComponent<GemParameter>().IsEnemy == true
									&& NowLinkNumber >= MinLinkNumber) {
									SelectGem (hit.collider.gameObject);

									IsLockEnemy = true;
								}
							}
						}
					}
				}
			}

            if (GemGenerator.ErasedGems.Count > 0 && GemGenerator.ErasedGems[0].GetComponent<GemParameter>().IsBomb == false)
            {
                DrawChainLine();

                foreach (GameObject gem in CheckedGems)
                {

                    if (gem != null)
                    {
                        gem.GetComponent<Animator>().enabled = false;

                        gem.transform.Find("Selected").gameObject.SetActive(false);

                    }
                }
                CheckedGems.Clear();
                
                SearchChainableGem(GemGenerator.ErasedGems[0]);

            }
        }

		if(Input.GetMouseButtonUp(0)){

			IsLongTap = false;


            if (GameDefeat)
                return;

            if (GameClear)
                return;

            if (NowLinkNumber < MinLinkNumber) {//選択キャンセル処理

				if (IsLockSecondBomb == true) {
					BombLink ();

					IsLockFirstBomb = false;
					IsLockSecondBomb = false;

					ChainLine.GetComponent<LineRenderer> ().positionCount = 0;

					return;
				}

				if (IsLockFirstBomb == true) {
					ActivateBomb (GemGenerator.ErasedGems[0]);

					IsLockFirstBomb = false;
					IsLockSecondBomb = false;
					return;
				}

				for (int num = 0; GemGenerator.ErasedGems.Count > 0; num++) {
					GemGenerator.ErasedGems [GemGenerator.ErasedGems.Count - 1].GetComponent<GemParameter>().IsSelected = false;
					//GemGenerator.ErasedGems [GemGenerator.ErasedGems.Count - 1].transform.Find ("Selected").gameObject.SetActive (false);
					GemGenerator.ErasedGems.RemoveAt (GemGenerator.ErasedGems.Count -1);
				}

			} else {
				
				ErasedNum = 0;
				//選択ジェム消去処理
				for (int num = 0; GemGenerator.ErasedGems.Count > 0; num++) {

					if (GemGenerator.ErasedGems [0].GetComponent<GemParameter> ().IsEnemy) {
						
						ChainNum = ErasedNum;

						DeadEnemy = GemGenerator.ErasedGems [0];

						GemGenerator.ErasedGems [0].GetComponent<GemParameter>().IsSelected = false;

						ChainNum = ErasedNum;

						GemGenerator.ErasedGems.RemoveAt (0);

						if(IsExMode == false)
						gameManager.ExPoint += AddExPoint;

						ResetExistGems ();

						Invoke ("EnemyLifeDamage", num / (10 * EraseSpeed) + 0.5f);

						break;
					}else{

					ErasedNum++;

						if (ErasedNum >= NumForGenerateBomb && GemGenerator.ErasedGems.Count == 1) {

							gemGenerator.BombGeneratePos = GemGenerator.ErasedGems [0].transform.position;

							gemGenerator.Invoke("GenerateBomb",num / (10 * EraseSpeed) + 0.5f);
						}

					GemGenerator.ErasedGems [0].GetComponent<GemParameter> ().IsDead = true;

						AddSkillPoint ((int)GemGenerator.ErasedGems [0].GetComponent<GemParameter>().G_color);

					Destroy (GemGenerator.ErasedGems [0], num / (10 * EraseSpeed));

                        PlayDeleteSE = true;

                    DeleteEffect(GemGenerator.ErasedGems[0].transform.position, num);

					GemGenerator.ErasedGems.RemoveAt (0);
					
					if(IsExMode == false)
					gameManager.ExPoint += AddExPoint;

					ResetExistGems ();

					Invoke ("DelayGenerate", num / (10 * EraseSpeed) + 0.5f);
				}
				}

				ChainNum = ErasedNum;
				comboUi.ComboNum++;
				gameManager.Chain = ChainNum;


				ScoreManager.CalculationScore (ChainNum);
			}

			GemGenerator.ExistNum = GemGenerator.ExistGems.Count;

			LastSelectGem = null;
			NowLinkNumber = 0;
			NowLinkNumInExChain = 0;
			IsLockEnemy = false;
			IsLockFirstBomb = false;
			IsLockSecondBomb = false;

			ChainLine.GetComponent<LineRenderer> ().positionCount = 0;


			foreach (GameObject gem in CheckedGems) {

				if (gem != null) {
					gem.GetComponent<Animator> ().enabled = false;

					gem.transform.Find ("Selected").gameObject.SetActive (false);

				}
			}
			CheckedGems.Clear ();


			foreach (Transform gem in gemGenerator.GemsFolder.transform) {
				gem.transform.Find ("Selected").gameObject.SetActive (false);
			}
		}

	}

	void ResetExistGems(){

		for (int num = 0; num < GemGenerator.ExistGems.Count; num++) {

			if (GemGenerator.ExistGems [num].GetComponent<GemParameter>().IsDead == true) {

				GemGenerator.ExistGems.RemoveAt (num);

			}

		}

	}

	void DelayGenerate(){

		gemGenerator.GenerateGem ();
	}

    void SelectGem(GameObject selectGem)
    {

        selectGem.GetComponent<GemParameter>().IsSelected = true;

        GemGenerator.ErasedGems.Add(selectGem);

        LastSelectGem = selectGem;

        NowLinkNumber++;

        if (IsExMode)
            NowLinkNumInExChain++;

        if (selectGem.GetComponent<GemParameter>().IsBomb && GemGenerator.ErasedGems.Count < 3) {

            selectGem.transform.Find("Canvas").gameObject.SetActive(true);

            audioSource.clip = Resources.Load("Sound/SelectBomb" + (GemGenerator.ErasedGems.Count).ToString()) as AudioClip;

            audioSource.Play();
        }
        else
        {
            audioSource.clip = Resources.Load("Sound/SelectGem") as AudioClip;

            audioSource.Play();
        }
    }

	void EnemyLifeDamage(){

		if (IsBombDamage) {
			DeadEnemy.GetComponent<GemParameter> ().Life -= BombDamage;
			IsBombDamage = false;
		} else {
			DeadEnemy.GetComponent<GemParameter> ().Life -= ChainNum;
		}

		if (DeadEnemy.GetComponent<GemParameter> ().Life <= 0) {
			Destroy (DeadEnemy);
		}

	}

	void ActivateBomb(GameObject bomb){

		ErasedNum = 0;

		Collider[] DamagedGems = Physics.OverlapSphere (bomb.transform.position, BombRange);
	
		foreach(Collider gem in DamagedGems){

			GemParameter GP = gem.gameObject.GetComponent<GemParameter> ();

			if (gem.gameObject.CompareTag ("Gem") && GP.IsBomb == false) {

				if (GP.IsEnemy == true) {

					DeadEnemy = gem.gameObject;
					IsBombDamage = true;
					EnemyLifeDamage ();
				} else {

					if (GP.IsBomb == false) {
						GP.IsDead = true;

						if(IsExMode == false)
						gameManager.ExPoint += AddExPoint;

						ResetExistGems ();

						Destroy (gem.gameObject);

                        DeleteEffect(gem.gameObject.transform.position, 0);

                        ErasedNum++;

                        AddSkillPoint((int)GP.G_color);
					}
				}
			}
		}

		LastSelectGem = null;
		NowLinkNumber = 0;
		NowLinkNumInExChain = 0;
		GemGenerator.ExistNum = GemGenerator.ExistGems.Count;
		Invoke ("DelayGenerate",0.5f);
		Destroy (bomb);
		GemGenerator.ErasedGems.RemoveAt (0);

		comboUi.ComboNum++;

		ChainNum = 1;

		ScoreManager.CalculationScore (ErasedNum);

        audioSource.clip = Resources.Load("Sound/BombDelete") as AudioClip;

        audioSource.Play();

        BombCancel ();
	}

	void BombLink(){

		ErasedNum = 0;

		foreach (RaycastHit hit in Physics.SphereCastAll(GemGenerator.ErasedGems[0].transform.position,0.5f,
			(GemGenerator.ErasedGems [1].transform.position - GemGenerator.ErasedGems [0].transform.position).normalized,
			(GemGenerator.ErasedGems[0].transform.position - GemGenerator.ErasedGems[1].transform.position).magnitude))
		{
			
			GemParameter GP = hit.collider.gameObject.GetComponent<GemParameter> ();

			if (hit.collider.gameObject.CompareTag ("Gem") && GP.IsBomb == false) {
				
				if (GP.IsEnemy == true) {

					DeadEnemy = hit.collider.gameObject;
					IsBombDamage = true;
					Invoke("EnemyLifeDamage",ErasedNum * 0.01f);
				} else {

					if (GP.IsBomb == false) {
						GP.IsDead = true;

						if(IsExMode == false)
						gameManager.ExPoint += AddExPoint;

						ResetExistGems ();

                        PlayDeleteSE = true;
                        DeleteEffect(hit.collider.gameObject.transform.position, ErasedNum);
                        
                        Destroy (hit.collider.gameObject, ErasedNum / (10 * EraseSpeed));
                        

                        ErasedNum++;

                        AddSkillPoint((int)GP.G_color);
                    }
				}
			}

		}

		LastSelectGem = null;
		NowLinkNumber = 0;
		NowLinkNumInExChain = 0;
		GemGenerator.ExistNum = GemGenerator.ExistGems.Count;
		Invoke ("DelayGenerate",0.5f);
		Destroy(GemGenerator.ErasedGems [0]);
		Destroy(GemGenerator.ErasedGems [1]);
		GemGenerator.ErasedGems.RemoveAt (0);
		GemGenerator.ErasedGems.RemoveAt (0);

		comboUi.ComboNum++;
		ChainNum = ErasedNum;
        gameManager.Chain = ChainNum;
        ScoreManager.CalculationScore (ErasedNum);

        audioSource.clip = Resources.Load("Sound/BombDelete") as AudioClip;

        audioSource.Play();

        //BombCancel ();
	}

	void DrawChainLine(){

        ChainLine.GetComponent<LineRenderer>().positionCount = GemGenerator.ErasedGems.Count;

        int cnt = 0;

        foreach (GameObject _eg in GemGenerator.ErasedGems)
        {
 
            Vector3 pos = _eg.transform.position;

            ChainLine.GetComponent<LineRenderer>().SetPosition(cnt, new Vector3(pos.x, pos.y, pos.z - 1f));
            
            cnt++;
        }
	}

	void SearchChainableGem(GameObject CheckGem){
        
        CheckedGems.Add(CheckGem);

        CheckGem.GetComponent<Animator> ().enabled = true;
        CheckGem.transform.Find("Selected").gameObject.SetActive(true);
        

        Collider[] ChainableGems = Physics.OverlapSphere (CheckGem.transform.position, MinLinkDistance);

		foreach(Collider gem in ChainableGems){
			
			GemParameter GP = gem.gameObject.GetComponent<GemParameter> ();

			if (gem.gameObject.CompareTag ("Gem") && GP.G_color == GemGenerator.ErasedGems[0].GetComponent<GemParameter>().G_color
				&& GP.IsEnemy == false) {

                bool same = false;
                
				foreach (GameObject SameGem in CheckedGems) {

                    if (SameGem == gem.gameObject)
                    {
                        same = true;
                    }
                }

                if (same == false)
                {

                    CheckedGems.Add(gem.gameObject);

                    SearchChainableGem(gem.gameObject);
                }


            }
		}
	}

	void BombCancel(){
       
		foreach (Transform e_gem in gemGenerator.GemsFolder.transform) {

			if(e_gem.gameObject.GetComponent<GemParameter>().IsDead == true)
				Destroy (e_gem.gameObject);

		}
	}

	void AddSkillPoint(int colorNum){

		switch (colorNum) {

		case 0:
			if (gameManager.SkillPoint_R < MaxSkillPoint [colorNum])
				gameManager.SkillPoint_R += 1f;
			break;

		case 1:
			if (gameManager.SkillPoint_G < MaxSkillPoint [colorNum])
				gameManager.SkillPoint_G += 1f;
			break;

		case 2:
			if (gameManager.SkillPoint_B < MaxSkillPoint [colorNum])
				gameManager.SkillPoint_B += 1f;
			break;

		case 3:
			if (gameManager.SkillPoint_Y < MaxSkillPoint [colorNum])
				gameManager.SkillPoint_Y += 1f;
			break;

		default:
			break;

		}
	}

    public void DeleteBySkillInfluenceArea(GameObject bomb)
    {

        ErasedNum = 0;

        Collider[] DamagedGems = Physics.OverlapSphere(bomb.transform.position, bomb.transform.localScale.x / 2f);

        foreach (Collider gem in DamagedGems)
        {

            GemParameter GP = gem.gameObject.GetComponent<GemParameter>();

            if (gem.gameObject.CompareTag("Gem") && GP.IsBomb == false)
            {

                if (GP.IsEnemy == true)
                {

                    DeadEnemy = gem.gameObject;
                    IsBombDamage = true;
                    EnemyLifeDamage();
                }
                else
                {

                    if (GP.IsBomb == false)
                    {
                        GP.IsDead = true;

                        if (IsExMode == false)
                            gameManager.ExPoint += AddExPoint;

                        ResetExistGems();

                        Destroy(gem.gameObject);

                        DeleteEffect(gem.gameObject.transform.position, 0);

                        ErasedNum++;

                        AddSkillPoint((int)GP.G_color);
                    }
                }
            }
        }

        gemGenerator.BombGeneratePos = bomb.transform.position;

        bomb.transform.parent.GetComponent<SkillInfluenceArea>().AreaCollider.Remove(bomb);

    }

    public void FinishDeleteBySkill(GameObject _parent)
    {
        
        LastSelectGem = null;
        NowLinkNumber = 0;
        NowLinkNumInExChain = 0;
        GemGenerator.ExistNum = GemGenerator.ExistGems.Count;
        Invoke("DelayGenerate", 0.5f);
        //GemGenerator.ErasedGems.RemoveAt(0);

        comboUi.ComboNum++;

        ChainNum = ErasedNum;

        gameManager.Chain = ChainNum;

        ScoreManager.CalculationScore(ErasedNum);

        BombCancel();

        if (ErasedNum >= NumForGenerateBomb)
        {
            gemGenerator.GenerateBomb();
        }

        Destroy(_parent);

        audioSource.clip = Resources.Load("Sound/AreaDelete") as AudioClip;

        audioSource.Play();

        IsProcessingSkill = false;
    }

    public void DeleteEffect(Vector3 pos,int num)
    {
        EffectPos.Add (pos);
        
        Invoke("InstanceDeleteEffect", num / (10 * EraseSpeed));
        
    }

    void InstanceDeleteEffect()
    {

        GameObject _effect = Instantiate(Resources.Load("Effect/Ef_Delete"), EffectPos[0], Quaternion.identity) as GameObject;

        EffectPos.RemoveAt(0);

        _effect.transform.SetParent(EffectGroup.transform);

        if(PlayDeleteSE)
        _effect.GetComponent<AudioSource>().Play();

        Destroy(_effect, 1f);

        if (EffectPos.Count == 0)
            PlayDeleteSE = false;

    }

}

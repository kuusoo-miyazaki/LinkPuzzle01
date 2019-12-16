using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    public int ButtonColorNum;

    ControllManager controllManager;

    GameManager gameManager;


    void Start()
    {
        controllManager = GameObject.Find("GameManager").GetComponent<ControllManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }


    public void OnSkillButton()
    {
        if (controllManager.PermissionSkillUse[ButtonColorNum] == true)
        {
            controllManager.PermissionSkillUse[ButtonColorNum] = false;

            switch (ButtonColorNum)
            {

                case 0:
                    gameManager.SkillPoint_R = 0;
                    break;

                case 1:
                    gameManager.SkillPoint_G = 0;
                    break;

                case 2:
                    gameManager.SkillPoint_B = 0;
                    break;

                case 3:
                    gameManager.SkillPoint_Y = 0;
                    break;

                default:
                    break;

            }


            controllManager.IsProcessingSkill = true;

            GameObject sAnim = Instantiate(Resources.Load("SkillAnimation/" + gameManager.SkillID[ButtonColorNum]), transform.position, Quaternion.identity) as GameObject;

            sAnim.transform.SetParent(GameObject.Find("Canvas").transform,false);

            sAnim.transform.localPosition = new Vector3(0, 0, 0);

            sAnim.transform.name = gameManager.SkillID[ButtonColorNum];

            sAnim.GetComponent<SkillAnimation>().ExecuteSkillButton = ButtonColorNum;

            transform.parent.transform.Find("FullSign").gameObject.SetActive(false);

            //Time.timeScale = 0;

        }
    }
}

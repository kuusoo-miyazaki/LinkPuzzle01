using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillProcess : MonoBehaviour
{

    void Start()
    {
        
    }

    // Update is called once per frame
    public void StartSkillProcess(string ID,int ButtonNum)
    {
        print("skill " + ID);

        switch (GetComponent<UnitManager>().SkillType[ButtonNum])
        {

            case 1:
                Instantiate(Resources.Load("SkillInfluenceArea/" + ID), new Vector3(0, 0, 0), Quaternion.identity);

                return;

            default:
                return;

        }



    }
    
}

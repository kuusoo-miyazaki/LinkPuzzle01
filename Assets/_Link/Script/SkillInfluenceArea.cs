using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfluenceArea : MonoBehaviour
{

    public List<GameObject> AreaCollider = new List<GameObject>();


    void Start()
    {

        StartCoroutine("WaitingCheck");

    }

    
    IEnumerator WaitingCheck()
    {

        while (true)
        {
            if(AreaCollider.Count <= 0)
            {

                GameObject.Find("GameManager").GetComponent<ControllManager>().FinishDeleteBySkill(this.gameObject);

                StopCoroutine("WaitingCheck");
                yield return null;
            }

            yield return new WaitForSeconds(0.01f);

        }

    }
}

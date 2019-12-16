using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaCollider : MonoBehaviour
{
    
    void Start()
    {

        GameObject.Find("GameManager").GetComponent<ControllManager>().DeleteBySkillInfluenceArea(this.gameObject);

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAnimation : MonoBehaviour
{

    public int ExecuteSkillButton;
    
    void EndSkillAnimation()
    {
        //Time.timeScale = 1;

        GameObject.Find("GameManager").GetComponent<SkillProcess>().StartSkillProcess(transform.name,ExecuteSkillButton);

        Destroy(this.gameObject);
    }
}

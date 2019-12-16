using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeButton : MonoBehaviour
{

    public float Power;

    public float ReuseTime;

    public bool NotUse;

    GemGenerator gemGenerator;

    void Start()
    {
        gemGenerator = GameObject.Find("GemGenerator").GetComponent<GemGenerator>();
    }

    public void OnShakeButton()
    {

        if (NotUse)
            return;

        NotUse = true;

        StartCoroutine("ReuseCount");
        
        foreach(Transform gem in gemGenerator.GemsFolder.transform)
        {

            gem.GetComponent<Rigidbody>().AddForce(transform.up * (Power  * Random.Range(1f,3f)));

        }

    }

    IEnumerator ReuseCount()
    {

        float cnt = 0;

        while (true)
        {

            cnt += ReuseTime;

            if(cnt >= 100)
            {
                NotUse = false;
                StopCoroutine("ReuseCount");
                yield return null;
            }

            yield return new WaitForSeconds(0.01f);
        }

    }
}

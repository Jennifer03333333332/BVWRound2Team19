using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    private GameObject EndSceneLotus;
    private float speed = 0.2f;
    private float spreadEle = 2f;
    //private GameObject[] EndLotus;
    //flower
    private void Start()
    {
        EndSceneLotus = GameObject.Find("EndSceneLotus");

    }
    private void Update()
    {
        foreach (var i in EndSceneLotus.GetComponentsInChildren<EndSceneLotus>())
        {
            i.transform.position = Vector3.MoveTowards(i.transform.position, new Vector3(i.transform.position.x * spreadEle, 100, i.transform.position.z), Time.deltaTime * speed * Mathf.Abs(i.transform.position.x));

        }
    }



}

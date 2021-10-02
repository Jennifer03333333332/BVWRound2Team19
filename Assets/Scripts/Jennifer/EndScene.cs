using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    public float thrust = 1.0f;




    private GameObject EndSceneLotus;
    private float speed = 0.2f;
    private float spreadEle = 2f;

    private bool StartEndScene;

    
    //private GameObject[] EndLotus;
    //flower
    private void Start()
    {
        EndSceneLotus = GameObject.Find("EndSceneLotus");
        StartEndScene = false;
    }
    private void Update()
    {
        if (StartEndScene)
        {
            foreach (var i in EndSceneLotus.GetComponentsInChildren<EndSceneLotus>())
            {
                //i.transform.position = Vector3.MoveTowards(i.transform.position, new Vector3(i.transform.position.x * spreadEle, 100, i.transform.position.z), Time.deltaTime * speed * Mathf.Abs(i.transform.position.x));
                float ele = UnityEngine.Random.Range(-10, 9) / 10;
                print(ele);
                i.GetComponent<Rigidbody>().AddForce(0, thrust+ele, 0, ForceMode.Impulse);

            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boat"))
        {
            //?Control the boat positon
            StartEndScene = true;

        }
    }

}

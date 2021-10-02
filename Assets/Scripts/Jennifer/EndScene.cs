using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour
{
    public float thrust;


    //Where the boat would stay and watch the light
    public Vector3 BoatStays;
    private GameObject EndSceneLotus;
    private GameObject BoatManager;
    private float speed = 0.2f;
    private float spreadEle = 2f;

    private bool StartEndScene;

    
    //private GameObject[] EndLotus;
    //flower
    private void Start()
    {
        BoatManager = GameObject.Find("BoatManager");
        thrust = 0.5f;
        EndSceneLotus = GameObject.Find("EndSceneLotus");
        StartEndScene = false;
        BoatStays = new Vector3(0, 1, 171);
    }
    private void Update()
    {
        if (StartEndScene)
        {
            StartEndScene = false;


                



            foreach (var i in EndSceneLotus.GetComponentsInChildren<EndSceneLotus>())
            {
                //i.transform.position = Vector3.MoveTowards(i.transform.position, new Vector3(i.transform.position.x * spreadEle, 100, i.transform.position.z), Time.deltaTime * speed * Mathf.Abs(i.transform.position.x));
                float ele = ((float)Random.Range(-20, 50)) / 100;
                //print(thrust+ele);
                i.GetComponent<Rigidbody>().AddForce(0, thrust+ele, 0, ForceMode.Impulse);

            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boat"))
        {
            //?Control the boat positon
            BoatManager.SendMessage("ChangeBoatMoving", true);
            other.gameObject.SendMessage("ControlBoatStay", BoatStays);

            //Play the ending
            StartEndScene = true;
        }
    }
}

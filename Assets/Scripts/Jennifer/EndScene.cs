using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public float thrust;
    //Where the boat would stay and watch the light
    public Vector3 BoatStays;
    private GameObject BoatManager;
    GameManager gameManager;

    private GameObject EndSceneLotus;
    private float speed = 0.2f;
    private float spreadEle = 2f;

    private bool StartEndScene;
    private GameObject[] EndLotus;

    
    //private GameObject[] EndLotus;
    //flower
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        BoatManager = GameObject.Find("BoatManager");
        BoatStays = new Vector3(0, 1, 171);
        thrust = 0.5f;
        EndSceneLotus = GameObject.Find("EndSceneLotus");
        StartEndScene = false;
        EndLotus = GameObject.FindGameObjectsWithTag("FlyingLotus");
        foreach(var item in EndLotus)
        {
            item.SetActive(false);
        }
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
        if (other.gameObject.CompareTag("Boat") && gameManager.NowStage == (gameManager.gameStageStructs.Count))
        {
            BoatManager.SendMessage("ChangeBoatMoving", true);
            other.gameObject.SendMessage("ControlBoatStay", BoatStays);

            //?Control the boat positon
            //foreach (var item in EndLotus)
            //{
            //    item.SetActive(true);
            //    //StartCoroutine(LotusActiveRandomTime(item));
            //}
            StartEndScene = true;
            //Play the ending
            //SoundManager.instance.PlayingSound("EndSceneBGM");
            SoundManager.instance.StopPlayingMainBGM();
            StartEndScene = true;
        }
    }

    IEnumerator LotusActiveRandomTime(GameObject go)
    {
        yield return new WaitForSeconds(Random.RandomRange(0f, 2f));
        go.SetActive(true);
    }

    //IEnumerator JumpToEndScene()
    //{
    //    yield return new WaitForSeconds(5f);
    //    SceneManager.LoadScene("EndScene");

    //}

}

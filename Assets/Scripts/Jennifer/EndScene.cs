
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScene : MonoBehaviour
{
    public float thrust;
    public float thrustZ;
    //Where the boat would stay and watch the light
    public Vector3 BoatStays;
    public float LotusDelay;

    private GameObject BoatManager;
    GameManager gameManager;

    private GameObject EndSceneLotus;
    private float speed = 0.2f;
    private float spreadEle = 2f;

    private bool StartEndScene;
    private GameObject[] EndLotus;

    public GameObject EndingCanvas;
    //flower
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        BoatManager = GameObject.Find("BoatManager");
        BoatStays = new Vector3(0, 1, 171);
        LotusDelay = 3f;
        thrust = 0.5f;
        thrustZ = 0.2f;
        EndSceneLotus = GameObject.Find("EndSceneLotus");
        StartEndScene = false;
        EndLotus = GameObject.FindGameObjectsWithTag("FlyingLotus");
        foreach (var item in EndLotus)
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
                float eleY = ((float)Random.Range(-20, 50)) / 100;
                float eleZ = ((float)Random.Range(-20, 50)) / 100;
                //print(thrustZ + eleZ);
                //print(thrust+ele);
                i.GetComponent<Rigidbody>().AddForce(0, thrust + eleY, thrustZ + eleZ, ForceMode.Impulse);
            }
            StartCoroutine("DestoryFlowers");
            StartCoroutine("CreateEndCanvas");
        }




    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boat"))// && gameManager.NowStage == (gameManager.gameStageStructs.Count)
        {
            BoatManager.SendMessage("ChangeBoatMoving", true);
            other.gameObject.SendMessage("ControlBoatStay", BoatStays);

            //?Control the boat positon


            //Play the ending

            SoundManager.instance.PlayingSound("EndSceneBGM");
            SoundManager.instance.StopPlayingMainBGM();
            StartCoroutine("DelayLotus");

        }
    }

    IEnumerator DelayLotus()
    {
        yield return new WaitForSeconds(LotusDelay);
        foreach (var item in EndLotus)
        {
            item.SetActive(true);
            //StartCoroutine(LotusActiveRandomTime(item));
        }
        StartEndScene = true;

        //show Thank you and credit


    }
    IEnumerator DestoryFlowers()
    {
        yield return new WaitForSeconds(60);
        if (EndSceneLotus) Destroy(EndSceneLotus);
    }

    IEnumerator CreateEndCanvas()
    {
        yield return new WaitForSeconds(10);
        Instantiate(EndingCanvas, BoatStays + new Vector3(0, 0, 1.5f), Quaternion.Euler(0, 0, 0), this.transform);
    }
    //IEnumerator LotusActiveRandomTime(GameObject go)
    //{
    //    yield return new WaitForSeconds(Random.RandomRange(0f, 2f));
    //    go.SetActive(true);
    //}

    //IEnumerator JumpToEndScene()
    //{
    //    yield return new WaitForSeconds(5f);
    //    SceneManager.LoadScene("EndScene");

    //}

}

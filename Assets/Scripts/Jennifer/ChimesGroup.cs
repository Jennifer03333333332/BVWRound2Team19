using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimesGroup : MonoBehaviour
{
    [SerializeField]
    public static ChimesGroup instance;
    public int[] MusicOrder;

    //Manage Players action

    public int currentStep;
    public int currentAction;
    public int stepsCount;

    public bool StickHitTheBell;

    [Header("Bells Time")]
    public int whenErrorWaitInterval;
    public int ShowHintsWaitInterval;

    private Vector3 BoatStays;
    //Lotus in Range
    public List<GameObject> lotus = new List<GameObject>();

    public GameObject RingStick;
    private GameObject RingStickController;
    private GameObject BoatManager;
    private bool startThePuzzle;
    private int[] playerChoiceArr;

    private GameManager gm;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        
        currentStep = 0;
        stepsCount = 3;
        startThePuzzle = false;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        BoatManager = GameObject.Find("BoatManager");
        BuildRandomMusicOrder();
        //RingStick = GameObject.FindGameObjectWithTag("RingStick");
        StartCoroutine("WaitForTest");
        //TestField
        //WhenPlayerEntered();
        BoatStays = transform.position + new Vector3(0, 0, -0.2f);
    }
    


    IEnumerator WaitForTest()
    {
        yield return new WaitForSeconds(2);
        //Test code
        //SolvedBellPuzzle();
    }
    //When entered, player picked up the ringstick
    //Play the Hint Music

    //For each bell, sounds are different 

    //correct order:resolve the spirits
    //wrong order:
    private void BuildRandomMusicOrder()
    {
        MusicOrder = GlobalUtility.RandomIndex(0, 5, stepsCount);//12345 not 12356
        foreach(var a in MusicOrder)
        {
            print(a);
        }
        playerChoiceArr = new int[stepsCount];
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug Code delet later
        //print(other.gameObject.name);
        if (other.gameObject.CompareTag("Boat") && !startThePuzzle)
        {
            //Change animation. Player pickes up the ringing-stick on the boat
            other.gameObject.SendMessage("ControlBoatStay", BoatStays);
            startThePuzzle = true;
            WhenPlayerEntered();
        }
    }

    //1
    public void WhenPlayerEntered()
    {
        BoatManager.SendMessage("ChangeBoatMoving",true);
        SoundManager.instance.PlayingSound("PickUpBar");
        //RingStick.SetActive(true);
        
        //Play the Hint Music
        StartCoroutine("PlayHintMucis");
        //start detect
        StartCoroutine("BeginKnockBell");
    }
    IEnumerator PlayHintMucis() {
        //Play sounds in the order of BellMusicArray
        //print(MusicOrder);
        yield return new WaitForSeconds(1);
        foreach (var i in MusicOrder)
        {
            //print(GlobalUtility.IndexToToneName(i));
            SoundManager.instance.PlayingSound(GlobalUtility.IndexToToneName(i));
            yield return new WaitForSeconds(ShowHintsWaitInterval);
        }
    }

    //2
    IEnumerator BeginKnockBell()
    {
        print("BeginKnockBell");
        ControlRingStick(true);
        currentStep = 0;
        while (currentStep < stepsCount)
        {
            yield return StartCoroutine("EachStep");
            currentStep++;
        }
        FinishedPuzzle();
        
        yield return null;
    }
    //3
    IEnumerator EachStep()
    {
        print("wait for " + currentStep);
        yield return new WaitUntil(() => DoTheStep() == true);
        //yield return new WaitUntil(() => Check() == true);
        print("Finish Step "+ currentStep);//+ " Successfully"
        yield return null;
    }
    //4
    private bool DoTheStep()
    {
        if (StickHitTheBell)
        {
            print("Need " + MusicOrder[currentStep] + " , you choose " + currentAction);
            //Wrong bell
            playerChoiceArr[currentStep] = currentAction;
            StickHitTheBell = false;
            return true;
        }
        return false;
    }

    //xiao mings create and Destroy RingStick
    public void ControlRingStick(bool mode)
    {
        if (mode)
        {
            RingStickController = Instantiate(RingStick, GameObject.Find("RightHand Controller").transform, false);
            RingStickController.transform.localPosition = Vector3.zero;
            RingStickController.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 0));
            RingStickController.name = "RingStick";
        }
        else
        {
            Destroy(RingStickController);
        }
    }

    public void FinishedPuzzle()
    {
        bool res = IfPlayersRight();
        if (res != true)
        {
            //StartCoroutine("ErrorStep");
            ErrorStep();
        }
        else
        {
            SolvedBellPuzzle();
        }
    }

    private bool IfPlayersRight()
    {
        for(int i = 0;i < stepsCount; i++)
        {
            if (playerChoiceArr[i] != MusicOrder[i])
            {
                return false;
            }
        }
        return true;
    }
    public void ErrorStep()
    {
        print("error");
        ControlRingStick(false);

        SoundManager.instance.PlayingSound("PuzzleFailed1");//+ UnityEngine.Random.Range(1, 4)


        StopCoroutine("BeginKnockBell");
        //effects

        //Wrong Sounds


        //StartCoroutine()
        //yield return new WaitForSeconds(whenErrorWaitInterval);
        //start again
        StartCoroutine("PlayHintMucis");
        StartCoroutine("BeginKnockBell");
    }
    //Good end
    public void SolvedBellPuzzle()
    {
        print("Bell Puzzle solved");
        BoatManager.SendMessage("ChangeBoatMoving", false);
        //RingStick.SetActive(false);
        ControlRingStick(false);
        //Music
        //foreach (var item in lotus)
        //{
        //    if(item)item.GetComponent<Torch>().StartFire();
        //}
        this.GetComponent<GameStage>().SuccessPass();
        //Absorb the particles
        gameObject.GetComponentInChildren<Fireflies>().SendMessage("AbsorbTheParticle","Lantern");
        SoundManager.instance.PlayingSound("PuzzleSolvedRewards");
    }

}

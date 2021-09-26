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
    public bool checkIfStepCorrect;
    [Header("Bells Time")]
    public int whenErrorWaitInterval;
    public int ShowHintsWaitInterval;

    public enum BellPuzzleStatus { Enter, Step1, planeAni, cutscene2, babyAni, cutscene3, battle, end };
    public static BellPuzzleStatus gameStatus = BellPuzzleStatus.Enter;

    private GameObject RingStick;
    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        BuildRandomMusicOrder();
        currentStep = 0;
        stepsCount = 3;
        RingStick = GameObject.FindGameObjectWithTag("RingStick");
        StartCoroutine("WaitForTest");
        //TestField
        //WhenPlayerEntered();

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
        MusicOrder = GlobalUtility.RandomIndex(0, 5, 3);//12345 not 12356
        foreach(var a in MusicOrder)
        {
            print(a);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            WhenPlayerEntered(other.gameObject);
        }
    }

    //1
    public void WhenPlayerEntered(GameObject player)
    {
        //Change animation. Player pickes up the ringing-stick on the boat
        
        //Play the Hint Music
        StartCoroutine("PlayHintMucis");
        //start detect
        StartCoroutine("BeginKnockBell");
    }
    IEnumerator PlayHintMucis() {
        //Play sounds in the order of BellMusicArray
        print(MusicOrder);
        foreach(var i in MusicOrder)
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
        currentStep = 0;
        while (currentStep < stepsCount)
        {
            yield return StartCoroutine("EachStep");
            currentStep++;
        }
        SolvedBellPuzzle();
        yield return null;
    }
    //3
    IEnumerator EachStep()
    {
        print("wait for " + currentStep);
        yield return new WaitUntil(() => Check() == true);
        print("Do Step "+ currentStep + " Successfully");
        yield return null;
    }
    //4
    private bool Check()
    {
        if (checkIfStepCorrect)
        {
            print("Need "+MusicOrder[currentStep]  + " , you choose " + currentAction);
            //Wrong bell
            if (currentAction != MusicOrder[currentStep])
            {
                checkIfStepCorrect = false;
                //ErrorStep();
                StartCoroutine("ErrorStep");
                
                return false;
            }
            //Right Bell
            checkIfStepCorrect = false;
            return true;
        }
        //Haven't took action
        return false;
    }
    //Bad end
    IEnumerator ErrorStep()
    {
        print("error");
        StopCoroutine("BeginKnockBell");
        //effects

        //Wrong Sounds


        //StartCoroutine()
        yield return new WaitForSeconds(whenErrorWaitInterval);
        //start again
        StartCoroutine("BeginKnockBell");
        
    }
    //Good end
    public void SolvedBellPuzzle()
    {
        print("Bell Puzzle solved");
        //Music

        //Absorb the particles
        gameObject.GetComponentInChildren<Fireflies>().SendMessage("AbsorbTheParticle","RingStick");
    }

}

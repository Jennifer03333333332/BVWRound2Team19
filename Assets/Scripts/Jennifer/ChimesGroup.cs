using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimesGroup : MonoBehaviour
{
    public static ChimesGroup instance;
    //public int[] BellMusicArray;
    public int[] MusicOrder;
    //public ChimesBell[] ChimesBellsArray;

    //Manage Players action

    public int currentStep;
    public int currentAction;
    public int stepsCount;
    public bool checkIfStepCorrect;
    
    public int whenErrorWaitInterval;
    public int ShowHintsWaitInterval;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        BuildRandomMusicOrder();
        //int index = 0;
        //foreach (ChimesBell bell in gameObject.GetComponentsInChildren<ChimesBell>())
        //{
        //    BellMusicArray[index] = bell.ToneID;
        //    index++;
        //}
        currentStep = 0;
        stepsCount = 3;

        //WhenPlayerEntered();
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


    IEnumerator BeginKnockBell()
    {
        print("BeginKnockBell");
        currentStep = 0;
        while (currentStep < stepsCount)
        {
            yield return StartCoroutine("EachStep");
            currentStep++;
        }
        print("Bell Puzzle solves");

        yield return null;
    }
    IEnumerator EachStep()
    {
        print("wait for " + currentStep);
        yield return new WaitUntil(() => Check() == true);
        print("Do Step "+ currentStep + " Successfully");
        yield return null;
    }

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

    IEnumerator ErrorStep()
    {
        print("error");
        StopCoroutine("BeginKnockBell");
        //effects
        //StartCoroutine()
        yield return new WaitForSeconds(whenErrorWaitInterval);
        //start again
        StartCoroutine("BeginKnockBell");
        
    }



}

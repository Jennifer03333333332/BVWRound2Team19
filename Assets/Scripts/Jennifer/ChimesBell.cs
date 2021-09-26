using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimesBell : MonoBehaviour
{

    public int ToneID;
    public String ToneName;
    private bool isInCollision;

    private void Start()
    {
        //isInCollision = false;
    }
    //
    private void OnCollisionEnter(Collision collision)
    {
        //if (isInCollision) return;
        //print(collision.collider.gameObject.name);
        if (collision.collider.gameObject.name == "RingStick")
        {
            //
            SoundManager.instance.PlayingSound(ToneName);
            //print(ToneID);
            //might be Thread insecure
            if (!gameObject.GetComponentInParent<ChimesGroup>().checkIfStepCorrect)
            {
                gameObject.GetComponentInParent<ChimesGroup>().currentAction = ToneID;
                gameObject.GetComponentInParent<ChimesGroup>().checkIfStepCorrect = true;
            }
            
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //isInCollision = false;
    }
}

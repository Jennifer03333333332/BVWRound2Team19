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

    private void OnTriggerEnter(Collider other)
    {
        //if (isInCollision) return;
        //print(collision.collider.gameObject.name);
        if (other.gameObject.name == "RingStick")
        {
            //
            SoundManager.instance.PlayingSound(ToneName);
            //print(ToneID);
            //might be Thread insecure
            gameObject.GetComponentInParent<ChimesGroup>().currentAction = ToneID;
            gameObject.GetComponentInParent<ChimesGroup>().checkIfStepCorrect = true;

            
        }
    }

}

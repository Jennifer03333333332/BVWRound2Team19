using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimesBell : MonoBehaviour
{

    public int ToneID;
    public String ToneName;
    private bool isInCollision;


    public Material normalMaterial;
    public Material CollideMaterial;

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
            gameObject.GetComponentInParent<ChimesGroup>().StickHitTheBell = true;

            //change material
            Material[] materArr = gameObject.GetComponentsInChildren<Material>();
            for (int i=0; i< materArr.Length; i++)
            {
                materArr[i] = CollideMaterial;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "RingStick")
        {
            //change material
            Material[] materArr = gameObject.GetComponentsInChildren<Material>();
            for (int i = 0; i < materArr.Length; i++)
            {
                materArr[i] = normalMaterial;
            }

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionDoor : MonoBehaviour
{
    public Transform boatTransform;
    public float distance = 100f;
    private IntroductionManager introductionManager;
    private bool onlyOnce = false;

    private void OnEnable()
    {
        boatTransform = GameObject.Find("BoatIntro").GetComponent<Transform>();
        introductionManager = FindObjectOfType<IntroductionManager>();
        //this.transform.position = boatTransform.position + boatTransform.forward * distance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boat")
        {
            if (!onlyOnce)
            {
                onlyOnce = true;
                introductionManager.nowStage++;
              
            }

        }
    }
}

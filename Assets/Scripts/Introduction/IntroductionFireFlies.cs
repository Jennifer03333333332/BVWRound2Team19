using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionFireFlies : MonoBehaviour
{
    public Transform boatTransform;
    public float distance = 5f;
    private IntroductionManager introductionManager;
    private bool onlyOnce = false;

    // Start is called before the first frame update
    private void OnEnable()
    {
        boatTransform = GameObject.Find("BoatIntro").GetComponent<Transform>();
        introductionManager = FindObjectOfType<IntroductionManager>();
        this.transform.position = boatTransform.position + boatTransform.forward * distance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Boat")
        {
            if (!onlyOnce)
            {
                onlyOnce = true;
                introductionManager.nowStage++;
                this.GetComponentInChildren<Fireflies>().AbsorbTheParticle("Boat");
            }
            
        }
    }
}

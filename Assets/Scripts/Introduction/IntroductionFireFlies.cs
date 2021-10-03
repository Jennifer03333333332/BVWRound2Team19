using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionFireFlies : MonoBehaviour
{
    public Transform boatTransform;
    public float distance = 5f;
    private IntroductionManager introductionManager;

    private void Start()
    {
        boatTransform = GameObject.Find("BoatIntro").GetComponent<Transform>();
        introductionManager = FindObjectOfType<IntroductionManager>();
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        this.transform.position = boatTransform.position + boatTransform.forward * distance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Boat")
        {
            introductionManager.nowStage++;
        }
    }
}

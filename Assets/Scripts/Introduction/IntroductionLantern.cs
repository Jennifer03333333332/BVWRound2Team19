using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionLantern : MonoBehaviour
{
    private IntroductionManager introductionManager;
    private bool OneTimeIntro = false;
    private void OnEnable()
    {
        introductionManager = FindObjectOfType<IntroductionManager>();
        
    }
    private void Update()
    {
        if(!OneTimeIntro)
        {
            OneTimeIntro = true;
            introductionManager.nowStage++;
        }
             
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntroduction : MonoBehaviour
{
    GameManager gm;

    //被控制着生成的物体
    
    public enum IntroStage
    {
        TeachRide,
        ShowLantern,

    }
    private void Start()
    {
        //gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }



    private void GoToNextStage()
    {
          this.GetComponent<GameStage>().SuccessPass();
         Destroy(this.gameObject);
  

    }
}

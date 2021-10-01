using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Boat")
        {
            print("跳到endscene");
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.EndGame();
        }
       
    }
}

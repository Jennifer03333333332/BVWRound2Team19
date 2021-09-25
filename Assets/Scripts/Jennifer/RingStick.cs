using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingStick : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //sound: boat, water, chimesbell
        print(collision.collider.gameObject.name);
    }
    
}

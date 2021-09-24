using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public bool ThisFireIsOn = false;
    public GameObject fire;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Torch" )
        {
            Torch t = other.gameObject.GetComponentInParent<Torch>();
            if(ThisFireIsOn)
            {
                t.StartFire();
            }
            else if(t.ThisFireIsOn)
            {
                StartFire();
            }
        }    
        
    }

    public void StartFire()
    {
        ThisFireIsOn = true;
        fire.GetComponent<Light>().intensity = 2f;
    }
}

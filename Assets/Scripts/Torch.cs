using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public bool ThisFireIsOn = false;
    public GameObject fire;

    
    public void StartFire()
    {
        ThisFireIsOn = true;
        fire.GetComponent<Light>().intensity = 1.5f;
    }
}

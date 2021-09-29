using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public bool ThisFireIsOn = false;
    public GameObject fire;
    public float FireIntensity = 5f;
    private GameManager gm;
    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void StartFire()
    {
        ThisFireIsOn = true;
        fire.GetComponent<Light>().intensity = FireIntensity;
        
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.transform.tag == "Boat" && !ThisFireIsOn)
        {
            StartFire();
            gm.stage++;
            gameObject.GetComponentInChildren<Fireflies>().SendMessage("AbsorbTheParticle", "Lantern");

        }
    }
}

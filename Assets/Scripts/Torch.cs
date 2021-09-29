using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is Lotus
public class Torch : MonoBehaviour
{
    public bool ThisFireIsOn = false;
    public GameObject fire;
    public float FireIntensity = 5f;
    private GameManager gm;



    static float t = 0.0f;

    private void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void StartFire()
    {
        ThisFireIsOn = true;
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

    private void Update()
    {
        if (ThisFireIsOn)
        {
            fire.GetComponent<Light>().intensity = Mathf.Lerp(0, FireIntensity,t);
            t += 0.1f*Time.deltaTime;
            if (fire.GetComponent<Light>().intensity == FireIntensity)
            {
                ThisFireIsOn = false;
            }
        }
    }
}

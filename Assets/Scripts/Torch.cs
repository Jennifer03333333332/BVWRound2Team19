using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This is Lotus
public class Torch : MonoBehaviour
{
    public bool ThisFireIsOn = false;
    public GameObject fire;
    public float FireIntensity;
    private GameManager gm;

    public float lightspeed;

    static float t;

    private void Start()
    {
        FireIntensity = 5f;
        lightspeed = 0.1f;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void StartFire()
    {
        SoundManager.instance.PlayingSound("LightUp" + UnityEngine.Random.Range(1, 4));
        ThisFireIsOn = true;
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.transform.tag == "Boat" && !ThisFireIsOn)
        {
            StartFire();
            this.GetComponent<GameStage>().SuccessPass();
            gameObject.GetComponentInChildren<Fireflies>().SendMessage("AbsorbTheParticle", "Lantern");

        }
    }

    private void Update()
    {
        if (ThisFireIsOn)
        {
            fire.GetComponent<Light>().intensity = Mathf.Lerp(0, FireIntensity,t);
            t += lightspeed * Time.deltaTime;
            if (fire.GetComponent<Light>().intensity == FireIntensity)
            {
                ThisFireIsOn = false;
            }
        }
    }
}

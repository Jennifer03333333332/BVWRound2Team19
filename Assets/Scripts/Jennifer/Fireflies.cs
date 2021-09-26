using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireflies : MonoBehaviour
{
    public float speed;
    private bool StartAbsorbing;
    private GameObject target;
    private ParticleSystem particleSystem;
    //Effects
    List<ParticleCollisionEvent> particleCollisionEvents = new List<ParticleCollisionEvent>();
    public GameObject instantiateOnParticleCollision;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        StartAbsorbing = false;
        speed = 1;
    }
    public void AbsorbTheParticle(string tagName)
    {
        if (GameObject.FindGameObjectWithTag(tagName) != null) {
            target = GameObject.FindGameObjectWithTag(tagName);
        }
        
        StartAbsorbing = true;
    }

    private void Update()
    {
        if (StartAbsorbing)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
            if ((transform.position - target.transform.position).magnitude < 0.1)
            {
                //TODO new effects
                Destroy(gameObject);
            }
        }
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particleSystem.GetCollisionEvents(other, particleCollisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            //Instantiate(instantiateOnParticleCollision, particleCollisionEvents[i].intersection, Quaternion.identity);
        }
    }
}

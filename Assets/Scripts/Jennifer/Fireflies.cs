using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireflies : MonoBehaviour
{
    public float speed;
    public GameObject DestroyPrefab;
    
    private bool StartAbsorbing;
    private GameObject target;
    private ParticleSystem particleSystem;
    private ParticleSystem.Particle[] m_Particles;
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
            version2Particle();
            


            //rotation face to Stick
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
            if ((transform.position - target.transform.position).magnitude < 0.1)
            {
                //TODO new effects
                //particleSystem.shape = 
                print(transform);


                //end
                Destroy(gameObject);
            }
        }
    }

    //void OnParticleCollision(GameObject other)
    //{
    //    int numCollisionEvents = particleSystem.GetCollisionEvents(other, particleCollisionEvents);

    //    for (int i = 0; i < numCollisionEvents; i++)
    //    {
    //        //Instantiate(instantiateOnParticleCollision, particleCollisionEvents[i].intersection, Quaternion.identity);
    //    }
    //}

    void InitializeIfNeeded()
    {
        if (particleSystem == null)
            particleSystem = GetComponent<ParticleSystem>();

        if (m_Particles == null || m_Particles.Length < particleSystem.main.maxParticles)
            m_Particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
    }

    void versionOneParticle()
    {
        //if(particleSystem.isPaused == false) particleSystem.Pause();
        var no = particleSystem.noise;
        no.enabled = false;
        var shape = particleSystem.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 0;
        //Smooth rotate
        Quaternion quaternion = Quaternion.LookRotation(transform.position - target.transform.position);


        //shape.rotation = quaternion.eulerAngles;//Quaternion.Lerp(transform.rotation, quaternion, 5 * Time.deltaTime).eulerAngles;

        // GetParticles is allocation free because we reuse the m_Particles buffer between updates
        InitializeIfNeeded();
        int numParticlesAlive = particleSystem.GetParticles(m_Particles);

        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++)
        {
            m_Particles[i].velocity = new Vector3(0, 0, 0);
        }
    }
    void version2Particle()
    {
        StartAbsorbing = false;
        var no = particleSystem.noise;
        no.enabled = false;

        var trail = particleSystem.trails;
        trail.enabled = false;
        particleSystem.Stop();
        //Destroy(particleSystem);

        Instantiate(DestroyPrefab, transform);
    }
}

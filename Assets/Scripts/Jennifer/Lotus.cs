using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lotus : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            gameObject.GetComponentInChildren<Fireflies>().SendMessage("AbsorbTheParticle", "MainCamera");
        }
    }
}

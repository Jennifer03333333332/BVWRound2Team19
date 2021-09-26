using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    private Rigidbody rigidbody;
    public string RightHandMovement;
    public string LeftHandMovement;
    public float Speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }

    public void AddFroceOnBoat(string MovementName)
    {
        print("唤起划船" + MovementName);
        if(MovementName == RightHandMovement)
        {
            Vector3 dir = new Vector3(-1, 0, 1);
            dir.Normalize();
            rigidbody.AddForce(dir * Speed, ForceMode.Impulse);
        }
        if (MovementName == LeftHandMovement)
        {
            Vector3 dir = new Vector3(1, 0, 1);
            dir.Normalize();
            rigidbody.AddForce(dir * Speed, ForceMode.Impulse);
        }
    }
}

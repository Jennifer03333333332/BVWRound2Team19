using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBoatOnChime : MonoBehaviour
{
    public float speed;



    private bool StartControlling;
    private Vector3 targetPos;
    
    private void Start()
    {
        StartControlling = false;
        speed = 1f;
    }

    void ControlBoatStay(Vector3 target)
    {
        StartControlling = true;
        targetPos = new Vector3(target.x, transform.position.y, target.z);
    }

    private void Update()
    {
        if (StartControlling)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
            if ((transform.position - targetPos).magnitude < 0.1)
            {
                StartControlling = false;
            }
        }
    }
}

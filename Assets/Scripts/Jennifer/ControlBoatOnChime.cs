using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBoatOnChime : MonoBehaviour
{
    public float speed;



    private bool StartControlling;
    private Vector3 targetPos;
    private Quaternion startRotate;
    private void Start()
    {
        StartControlling = false;
        speed = 1f;
    }

    void ControlBoatStay(Vector3 target)
    {
        StartControlling = true;
        targetPos = new Vector3(target.x, transform.position.y, target.z);
        startRotate = transform.localRotation;
    }

    private void Update()
    {
        if (StartControlling)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * speed);
            transform.localRotation = Quaternion.Slerp(startRotate, Quaternion.EulerAngles(Vector3.zero), 1f);
            if ((transform.position - targetPos).magnitude < 0.1)
            {
                StartControlling = false;
            }
        }
    }
}

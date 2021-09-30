using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatController : MonoBehaviour
{
    private Rigidbody rigidbody;
    public string RightHandMovementforward = "rForward";
    public string RightHandMovementbackward = "rBackward";
    public string LeftHandMovementforward = "lForward";
    public string LeftHandMovementbackward = "lBackward";
    public float Speed = 2.5f;
    public float froceDegree = 30f;

    public float dirX = 0;
    public float dirY = 0;
    public Quaternion startRotate;
    public Quaternion EndRoatate;
    public float startTime;
    public float rotateTime;
    public float rotateAngle = 30f;
    public float rotateSpeed = 15f;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //jerry 神仙代码不准改
        float a = (Time.time - startTime) / (rotateAngle / rotateSpeed);
       transform.localRotation = Quaternion.Slerp(startRotate, EndRoatate, a);
        //print("船的朝向" + transform.forward);
        //jerry 神仙代码不准改
    }

    public void AddFroceOnBoat(string MovementName)
    {
        print("唤起划船" + MovementName);
        if(MovementName == RightHandMovementforward)
        {
            print("往前划");
            //jerry 神仙代码不准改
            startTime = Time.time;
            startRotate = transform.localRotation;
            EndRoatate = Quaternion.AngleAxis(rotateAngle*-1, Vector3.up) * startRotate;
            //jerry 神仙代码不准改
            //Vector3 dir = new Vector3(-1, 0, 1);
            Vector3 nowNormalPos = transform.forward.normalized;
            float nowDegree = Mathf.Acos(nowNormalPos.x);
            float newDegree = nowDegree + Mathf.PI * froceDegree / 180;
            if(newDegree > 360) { newDegree -= 30; }
            Vector3 dir = new Vector3(Mathf.Cos(newDegree), transform.position.y, Mathf.Sin(newDegree));
            print("新方向"+dir);
            dir.Normalize();
            rigidbody.AddForce(dir * Speed, ForceMode.Impulse);
        }
        else if(MovementName == RightHandMovementbackward)
        {
            print("往后划");
            dirX = -1;
            dirY = -1;
            Vector3 dir = new Vector3(-1, 0, -1);
            dir.Normalize();
            rigidbody.AddForce(dir * Speed, ForceMode.Impulse);
        }
        else if (MovementName == LeftHandMovementforward)
        {
            startTime = Time.time;
            startRotate = transform.localRotation;
            EndRoatate = Quaternion.AngleAxis(rotateAngle, Vector3.up) * startRotate;
            Vector3 nowNormalPos = transform.forward.normalized;
            float nowDegree = Mathf.Acos(nowNormalPos.x);
            float newDegree = nowDegree + Mathf.PI * froceDegree / 180;
            if (newDegree > 360) { newDegree -= 30; }
            Vector3 dir = new Vector3(Mathf.Cos(newDegree), transform.position.y, Mathf.Sin(newDegree));
            dir.Normalize();
            rigidbody.AddForce(dir * Speed, ForceMode.Impulse);
        }
        else if (MovementName == LeftHandMovementbackward)
        {
            Vector3 dir = new Vector3(1, 0, -1);
            dir.Normalize();
            rigidbody.AddForce(dir * Speed, ForceMode.Impulse);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class IntroBoatController : MonoBehaviour
{
    private Rigidbody rigidbody;
    public string RightHandMovementforward = "rForward";
    public string RightHandMovementbackward = "rBackward";
    public string LeftHandMovementforward = "lForward";
    public string LeftHandMovementbackward = "lBackward";
    public float Speed = 2.5f;
    public float froceDegree = 30f;
    public float movingAxis = 0f;//用于记录左右输入
    public float InputIntervalThreshold = 0.5f;//假的间隔,用于在玩家同时输入的时候,对移动做一定的改动;

    public float dirX = 0;
    public float dirY = 0;
    public Quaternion startRotate;
    public Quaternion EndRoatate;
    public float startTime;
    public float rotateTime;
    public float rotateAngle = 30f;
    public float rotateSpeed = 15f;

    public Animator RightPaddle;
    public Animator LeftPaddle;

    public Vector3 movingDir = Vector3.zero;

    public bool FirstForwardRide = false;

    private IntroductionManager introductionManager;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();
        introductionManager = FindObjectOfType<IntroductionManager>();
    }
    private void Update()
    {
        float a = (Time.time - startTime) / (rotateAngle / rotateSpeed);
        transform.localRotation = Quaternion.Slerp(startRotate, EndRoatate, a);
      


    }


   





    public void AddFroceOnBoat(string MovementName)
    {
        //print("唤起划船" + MovementName);
        if (MovementName == RightHandMovementforward)
        {
            //print("往前划");
            if(FirstForwardRide)
            {
                RightPaddle.SetBool("rightRide", true);
                SoundManager.instance.PlayingSound("RowingRight");
                StartCoroutine(TurnAround());
                StartCoroutine(EndRide(RightPaddle, "rightRide"));
            }
            StartCoroutine(RidingBoat());
            movingAxis -= 1;
            movingAxis = Mathf.Clamp(movingAxis, -1, 1);
          



        }
        else if (MovementName == LeftHandMovementforward)
        {
            if(FirstForwardRide)
            {
                LeftPaddle.SetBool("leftRide", true);
                SoundManager.instance.PlayingSound("RowingLeft");
                StartCoroutine(TurnAround());
                StartCoroutine(EndRide(LeftPaddle, "leftRide"));

            }

            StartCoroutine(RidingBoat());
            movingAxis += 1;
            movingAxis = Mathf.Clamp(movingAxis, -1, 1);
            
        }

    }


    IEnumerator EndRide(Animator ride, string bName)
    {
        yield return new WaitForSeconds(0.5f);
        print("关闭划船动画" + bName);
        ride.SetBool(bName, false);

    }

    IEnumerator TurnAround()
    {
        yield return new WaitForSeconds(InputIntervalThreshold);

        if (movingAxis != 0)
        {
            startTime = Time.time;
            startRotate = transform.localRotation;
            EndRoatate = Quaternion.AngleAxis(rotateAngle * movingAxis, Vector3.up) * startRotate;
        }

    }



    public Material normalMaterial;
    public Renderer Left;
    public Renderer Right;
    IEnumerator RidingBoat()
    {
        yield return new WaitForSeconds(InputIntervalThreshold);
        if (movingAxis == 0)
        {
            //player's First Ride;
            if(!FirstForwardRide)
            {
                Left.material = normalMaterial;
                Right.material = normalMaterial;
                RightPaddle.SetBool("rightRide", true);
                LeftPaddle.SetBool("leftRide", true);
                SoundManager.instance.PlayingSound("RowingRight");
                SoundManager.instance.PlayingSound("RowingLeft");
                StartCoroutine(EndRide(RightPaddle, "rightRide"));
                StartCoroutine(EndRide(LeftPaddle, "leftRide"));
                introductionManager.nowStage++;
                FirstForwardRide = true;
            }
            rigidbody.AddForce(transform.forward * Speed, ForceMode.Impulse);
        }
        else if (movingAxis != 0)
        {
            if(FirstForwardRide)
            {
                Vector3 dir = FroceDir(froceDegree * movingAxis);
                rigidbody.AddForce(dir * Speed, ForceMode.Impulse);
            }
       
        }

    }

    private Vector3 FroceDir(float FroceAngle)
    {
        Vector3 nowNormalPos = transform.forward.normalized;
        float nowDegree = Mathf.Acos(nowNormalPos.x);
        //Asin范围是[-90,90]
        //Acos范围是[0,180];
        if (nowNormalPos.z < 0)
        {
            nowDegree = Mathf.PI * 2 - nowDegree;
        }
        //对大于180的角度,进行处理
        float newDegree = nowDegree - Mathf.PI * FroceAngle / 180;
        if (newDegree > 360) { newDegree -= 360; }
        else if (newDegree < 0) { newDegree += 360; }
        Vector3 dir = new Vector3(Mathf.Cos(newDegree), transform.position.y, Mathf.Sin(newDegree));
        print("新方向" + dir);
        dir.Normalize();
        return dir;
    }
}

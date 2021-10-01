using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using PDollarGestureRecognizer;
using System.IO;
using UnityEngine.Events;
using UnityEngine.UI;


[System.Serializable]
public class NewStringEvent : UnityEvent<string> { }
public class BoatMovingAdvance : MonoBehaviour
{
    public InputDevice rightHand;
    public InputDevice leftHand;
    public Transform leftT;
    public Transform rightT;
    public Transform rigT;
    public Vector3 ReversoRightT;//相对右手动作
    public Vector3 ReversoLeftT;//相对左手动作
    public GameObject debugCubePrefab;
    public GameObject debugCubePrefab2;
    private Vector3 planeNormal;
    private List<Gesture> gesturesList = new List<Gesture>();
    public float recordInterval = 0.05f;
    public float recognitionThreshold = 0.6f;
    public Text debugText;

    public NewStringEvent onRecognized;
    public GestureStore gestureStore;

    public bool creatMode = true;
    public string newGestureName;
    public bool RisMove = false;
    public bool RisPress = false;
    public bool LisMove = false;
    public bool LisPress = false;
    public bool recordForwardGesture = true;
    public List<Vector3> rightPositionList = new List<Vector3>();
    public List<Vector3> leftPositionList = new List<Vector3>();
    //jennifer
    private GameObject RingStick;
    private bool rightTriggerValue;
    //jennifer ends


    [Header("无按键移动")]
    public Queue<Vector3> rightHandPos = new Queue<Vector3>();//控制无缝衔接移动
    public Queue<Vector3> leftHandPos = new Queue<Vector3>();//控制无缝衔接移动
    public bool canMove = true; //控制整个船能不能动
    public int HandListLimitCount = 15;
    public string forwardGestureName = "Forward";
    public string backwardGestureName = "Backward";

    // Start is called before the first frame update
    void Start()
    {
        //Gesture init
        planeNormal = new Vector3(1, 0, 0);
        //string[] gesturefile = Directory.GetFiles(Application.dataPath + "/Gesture/", "*.xml");
        BuildGestureFromPosition();
        //foreach (var gfile in gesturefile)
        //{
        //    gesturesList.Add(GestureIO.ReadGestureFromFile(gfile));
        //}
        debugText.text = "手势列表长度:" + gesturesList.Count;
        //get Input device
        List<InputDevice> rightdevices = new List<InputDevice>();
        List<InputDevice> leftdevices = new List<InputDevice>();
        InputDeviceRole righthand = InputDeviceRole.RightHanded;
        InputDeviceRole lefthand = InputDeviceRole.LeftHanded;
        InputDevices.GetDevicesWithRole(righthand, rightdevices);
        if (rightdevices.Count > 0)
        {
            rightHand = rightdevices[0];
        }
        InputDevices.GetDevicesWithRole(lefthand, leftdevices);
        if (leftdevices.Count > 0)
        {
            leftHand = leftdevices[0];
        }
        //jennifer
        RingStick = GameObject.FindGameObjectWithTag("RingStick");
        RingStick.SetActive(false);
        rightTriggerValue = false;
        //jennifer ends
    }

    // Update is called once per frame
    void Update()
    {
        if (rightHand.TryGetFeatureValue(CommonUsages.gripButton, out bool rgb))
        {
            //print("右手按下:" + rgb);
            RisPress = rgb;
        }

        if (leftHand.TryGetFeatureValue(CommonUsages.gripButton, out bool lgb))
        {
            //print("左手按下:" + lgb);
            LisPress = lgb;
        }
        //jennifer
        if (rightHand.TryGetFeatureValue(CommonUsages.triggerButton, out bool test))
        {
            rightTriggerValue = test;
            if (rightTriggerValue)
            {
                RingStick.SetActive(true);
            }
            else
            {
                RingStick.SetActive(false);
            }
        }
        //Jennifer end


        ReversoRightT = rightT.position - rigT.position;
        ReversoLeftT = leftT.position - rigT.position;
        //记录姿势数据
        if (!RisMove && RisPress)
        {
            RightStartMovement();
        }
        else if (RisMove && !RisPress)
        {
            RightEndMovement();
        }
        else if (RisMove && RisPress)
        {
            RightUpdateMovement();
        }
        //记录姿势数据 end

        //无缝输入input
        if (canMove)
        {
            BuildGestureFromPosition();
            rightHandGestureRecognize();
            leftHandGestureRecognize();
        }



    }


    /// <summary>
    /// 右手无按键移动
    /// </summary>
    private void rightHandGestureRecognize()
    {
        //将数据泵入queue
        if(rightHandPos.Count == 0)
        {
            rightHandPos.Enqueue(ReversoRightT);
            //debugText.text = "成功泵入第一个值";
        }
        if(rightHandPos.Count > 0)
        {
            Vector3[] rightHandPosArray = rightHandPos.ToArray();
            Vector3 latestPos = rightHandPosArray[rightHandPosArray.Length - 1];
            if(Vector3.Distance(latestPos, ReversoRightT) > recordInterval && rightHandPos.Count < HandListLimitCount)
            {
                if (debugCubePrefab) Destroy(Instantiate(debugCubePrefab, ReversoRightT, Quaternion.identity), 1);
                rightHandPos.Enqueue(ReversoRightT);
                //debugText.text = "成功泵入后续值" + ReversoRightT;
            }
            //queue满了之后更新队列
            else if (Vector3.Distance(latestPos, ReversoRightT) > recordInterval && rightHandPos.Count == HandListLimitCount)
            {
                //debugText.text = "queue满了";
                rightHandPos.Dequeue();
                rightHandPos.Enqueue(ReversoRightT);
                //每次更新后执行一次姿势建立,并且比对,正确姿势触发event
                Point[] pointArray = new Point[rightHandPos.Count];
                rightHandPosArray = rightHandPos.ToArray();

                for (int i = 0; i < rightHandPosArray.Length; i++)
                {
                    Vector3 rightPoint = Vector3.ProjectOnPlane(rightHandPosArray[i], GameObject.Find("Main Camera").transform.right);
                    if (debugCubePrefab2) Destroy(Instantiate(debugCubePrefab2, rightPoint, Quaternion.identity), 3);
                    pointArray[i] = new Point(rightPoint.z, rightPoint.y, 0);
                }
                Gesture newGesture = new Gesture(pointArray);
                Result result = PointCloudRecognizer.Classify(newGesture, gesturesList.ToArray());
                string movementName = result.GestureClass;
                //Debug.Log(result.GestureClass + "" + result.Score);
                //Debug.Log(movementName);
                debugText.text = result.GestureClass + "  :" + result.Score;
                if (result.Score > recognitionThreshold)
                {
                    if(result.GestureClass == forwardGestureName)
                    {
                        onRecognized.Invoke("rForward");
                        rightHandPos.Clear();
                    }
                    else if(result.GestureClass == backwardGestureName)
                    {
                        onRecognized.Invoke("rBackward");
                        rightHandPos.Clear();
                    }
                    
                }

            }
        }
        
      
            
    }

    /// <summary>
    /// 左手无按键移动
    /// </summary>
    private void leftHandGestureRecognize()
    {
        //将数据泵入queue
        if (leftHandPos.Count == 0)
        {
            leftHandPos.Enqueue(ReversoLeftT);
        }
        if (leftHandPos.Count > 0)
        {
            Vector3[] leftHandPosArray = leftHandPos.ToArray();
            Vector3 latestPos = leftHandPosArray[leftHandPosArray.Length - 1];
            if (Vector3.Distance(latestPos, ReversoLeftT) > recordInterval && leftHandPos.Count < HandListLimitCount)
            {
                if (debugCubePrefab) Destroy(Instantiate(debugCubePrefab, ReversoLeftT, Quaternion.identity), 1);
                leftHandPos.Enqueue(ReversoLeftT);
            }
            //queue满了之后更新队列
            else if (Vector3.Distance(latestPos, ReversoLeftT) > recordInterval && leftHandPos.Count == HandListLimitCount)
            {
                leftHandPos.Dequeue();
                leftHandPos.Enqueue(ReversoLeftT);
                //每次更新后执行一次姿势建立,并且比对,正确姿势触发event
                Point[] pointArray = new Point[leftHandPos.Count];
                leftHandPosArray = leftHandPos.ToArray();

                for (int i = 0; i < leftHandPosArray.Length; i++)
                {
                    Vector3 leftPoint = Vector3.ProjectOnPlane(leftHandPosArray[i], GameObject.Find("Main Camera").transform.right);
                    if (debugCubePrefab2) Destroy(Instantiate(debugCubePrefab2, leftPoint, Quaternion.identity), 3);
                    pointArray[i] = new Point(leftPoint.z, leftPoint.y, 0);
                }
                Gesture newGesture = new Gesture(pointArray);
                Result result = PointCloudRecognizer.Classify(newGesture, gesturesList.ToArray());
                string movementName = result.GestureClass;
                //Debug.Log(result.GestureClass + "" + result.Score);
                //Debug.Log(movementName);
                debugText.text = result.GestureClass + "  :" + result.Score;
                if (result.Score > recognitionThreshold)
                {
                    if (result.GestureClass == forwardGestureName)
                    {
                        onRecognized.Invoke("lForward");
                        leftHandPos.Clear();
                    }
                    else if (result.GestureClass == backwardGestureName)
                    {
                        onRecognized.Invoke("lBackward");
                        leftHandPos.Clear();
                    }
                }

            }
        }
    }







    /// <summary>
    /// RightHandMovement
    /// </summary>
    void RightStartMovement()
    {
        //Debug.Log("开始手动");
        rightPositionList.Clear();
        RisMove = true;
        rightPositionList.Add(ReversoRightT);


        if (recordForwardGesture && creatMode)
        {
            gestureStore.ForwardPositionList.Clear();
            gestureStore.ForwardPositionList.Add(ReversoRightT);
        }
        else if (!recordForwardGesture && creatMode)
        {
            gestureStore.BackwardPositionList.Clear();
            gestureStore.BackwardPositionList.Add(ReversoRightT);
        }
        if (debugCubePrefab) Destroy(Instantiate(debugCubePrefab, ReversoRightT, Quaternion.identity), 3);
    }
    void RightEndMovement()
    {
        //Debug.Log("结束手动");
        RisMove = false;
        Debug.Log(rightPositionList.Count);
        //创造手势
        Point[] pointArray = new Point[rightPositionList.Count];

        for (int i = 0; i < rightPositionList.Count; i++)
        {
            Vector3 rightPoint = Vector3.ProjectOnPlane(rightPositionList[i], GameObject.Find("Main Camera").transform.right);
            //Vector3 rightPoint = Vector3.ProjectOnPlane(rightPositionList[i], planeNormal);

            if (debugCubePrefab2) Destroy(Instantiate(debugCubePrefab2, rightPoint, Quaternion.identity), 3);
            pointArray[i] = new Point(rightPoint.z, rightPoint.y, 0);


        }
        Gesture newGesture = new Gesture(pointArray);

        if (creatMode)
        {
            newGesture.Name = newGestureName;
            gesturesList.Add(newGesture);

            //string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
            GestureIO.WriteGesture(pointArray, newGestureName, fileName);
        }
        //else
        //{
        //    Result result = PointCloudRecognizer.Classify(newGesture, gesturesList.ToArray());
        //    string movementName = result.GestureClass;
        //    //Debug.Log(result.GestureClass + "" + result.Score);
        //    //Debug.Log(movementName);
        //    debugText.text = result.GestureClass + "  :" + result.Score;
        //    if (result.Score > recognitionThreshold)
        //    {
        //        onRecognized.Invoke("r");
        //    }
        //}
    }
    void RightUpdateMovement()
    {
        //Debug.Log("正在手动");

        Vector3 latestPos = rightPositionList[rightPositionList.Count - 1];
        if (Vector3.Distance(latestPos, ReversoRightT) > recordInterval)
        {
            if (debugCubePrefab) Destroy(Instantiate(debugCubePrefab, ReversoRightT, Quaternion.identity), 1);
            rightPositionList.Add(ReversoRightT);
            if (recordForwardGesture && creatMode) gestureStore.ForwardPositionList.Add(ReversoRightT);
            else if (!recordForwardGesture && creatMode) gestureStore.BackwardPositionList.Add(ReversoRightT);
        }

    }



    ///// <summary>
    ///// RightHandMovement
    ///// </summary>
    //void LeftStartMovement()
    //{
    //    //Debug.Log("开始手动");
    //    leftPositionList.Clear();
    //    LisMove = true;
    //    leftPositionList.Add(ReversoLeftT);

    //    if (debugCubePrefab) Destroy(Instantiate(debugCubePrefab, ReversoLeftT, Quaternion.identity), 3);
    //}
    //void leftEndMovement()
    //{
    //    //Debug.Log("结束手动");
    //    LisMove = false;
    //    Debug.Log(leftPositionList.Count);
    //    //创造手势
    //    Point[] pointArray = new Point[leftPositionList.Count];

    //    for (int i = 0; i < leftPositionList.Count; i++)
    //    {
    //        Vector3 leftPoint = Vector3.ProjectOnPlane(leftPositionList[i], GameObject.Find("Main Camera").transform.right);
       

    //        if (debugCubePrefab2) Destroy(Instantiate(debugCubePrefab2, leftPoint, Quaternion.identity), 3);
    //        pointArray[i] = new Point(leftPoint.z, leftPoint.y, 0);


    //    }
    //    Gesture newGesture = new Gesture(pointArray);

    //    if (creatMode)
    //    {
    //        newGesture.Name = newGestureName;
    //        gesturesList.Add(newGesture);

    //        //string fileName = Application.persistentDataPath + "/" + newGestureName + ".xml";
    //        string fileName = Application.dataPath + "/Gesture/" + newGestureName + ".xml";
    //        GestureIO.WriteGesture(pointArray, newGestureName, fileName);
    //    }
    //    //else
    //    //{
    //    //    Result result = PointCloudRecognizer.Classify(newGesture, gesturesList.ToArray());
    //    //    string movementName = result.GestureClass;
    //    //    //Debug.Log(result.GestureClass + "" + result.Score);
    //    //    // Debug.Log(movementName);

    //    //    if (result.Score > recognitionThreshold)
    //    //    {
    //    //        onRecognized.Invoke("l");
    //    //    }
    //    //}
    //}
    //void LeftUpdateMovement()
    //{
    //    //Debug.Log("正在手动");

    //    Vector3 latestPos = leftPositionList[leftPositionList.Count - 1];
    //    if (Vector3.Distance(latestPos, ReversoLeftT) > recordInterval)
    //    {
    //        if (debugCubePrefab) Destroy(Instantiate(debugCubePrefab, ReversoLeftT, Quaternion.identity), 1);
    //        leftPositionList.Add(ReversoLeftT);
    //    }

    //}

    private void BuildGestureFromPosition()
    {

        Debug.Log(gestureStore.ForwardPositionList.Count);
        gesturesList.Clear();
        //创造前进手势
        if(gestureStore.ForwardPositionList.Count != 0)
        {
            Point[] pointArray = new Point[gestureStore.ForwardPositionList.Count];

            for (int i = 0; i < gestureStore.ForwardPositionList.Count; i++)
            {
                Vector3 rightPoint = Vector3.ProjectOnPlane(gestureStore.ForwardPositionList[i], GameObject.Find("Main Camera").transform.right);
                //Vector3 rightPoint = Vector3.ProjectOnPlane(rightPositionList[i], planeNormal);

                if (debugCubePrefab2) Destroy(Instantiate(debugCubePrefab2, rightPoint, Quaternion.identity), 3);
                pointArray[i] = new Point(rightPoint.z, rightPoint.y, 0);


            }
            Gesture newGesture = new Gesture(pointArray);


            newGesture.Name = forwardGestureName;
            gesturesList.Add(newGesture);
        }
      

        //创造后退手势
        //Point[] pointArray2 = new Point[gestureStore.BackwardPositionList.Count];

        //if(gestureStore.BackwardPositionList.Count != 0)
        //{
        //    for (int i = 0; i < gestureStore.BackwardPositionList.Count; i++)
        //    {
        //        Vector3 rightPoint = Vector3.ProjectOnPlane(gestureStore.BackwardPositionList[i], GameObject.Find("Main Camera").transform.right);
        //        //Vector3 rightPoint = Vector3.ProjectOnPlane(rightPositionList[i], planeNormal);

        //        if (debugCubePrefab2) Destroy(Instantiate(debugCubePrefab2, rightPoint, Quaternion.identity), 3);
        //        pointArray2[i] = new Point(rightPoint.z, rightPoint.y, 0);


        //    }
        //    Gesture newGesture2 = new Gesture(pointArray2);


        //    newGesture2.Name = backwardGestureName;
        //    gesturesList.Add(newGesture2);
        //}
    



    }
}

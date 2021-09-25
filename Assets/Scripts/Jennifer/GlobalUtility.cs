using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalUtility : MonoBehaviour
{
    //produce counts's random int between[left,right)
    public static int[] RandomIndex(int left, int right, int counts)
    {
        int[] resultArr = new int[counts];
        int currentCount = 0;
        while (currentCount<counts)
        {
            int index = UnityEngine.Random.Range(left, right);
            resultArr[currentCount] = index;
            currentCount++;
        }

        return resultArr;
    }
}

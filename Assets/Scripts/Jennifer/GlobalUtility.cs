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


    //Only for this proj
    //01234 - toneName
    public static string IndexToToneName(int index)
    {
        string res = "Gong";
        if (index == 0) res = "Gong";
        else if (index == 1) res = "Shang";
        else if (index == 2) res = "Jue";
        else if (index == 3) res = "Zhi";
        else if (index == 4) res = "Yu";
        else { print("Wrong IndexToToneName"); }
        return res;
    }
}

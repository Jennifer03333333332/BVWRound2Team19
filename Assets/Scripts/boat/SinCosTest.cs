using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinCosTest : MonoBehaviour
{

    public float angle;
    public float cosAngle;
    public float sinangle=.5f;
    public float cosangle = .5f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        angle = Mathf.Asin(sinangle);
        print((angle*360)/(2*Mathf.PI));
        cosAngle = Mathf.Acos(cosangle);
        print((cosAngle * 360) / (2 * Mathf.PI));
        
    }
}

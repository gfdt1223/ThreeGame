using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Independent : MonoBehaviour
{
    // Start is called before the first frame update
    public float mindistance;
    public bool isNeedIndepend;
    public Vector3 Position;
    public Vector3 Angle;
    void Start()
    {
        isNeedIndepend=false;
        Position=transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.GetComponent<Character>().CloestTargetDistance < mindistance&&this.GetComponent<Character>().Stage!=2)
        {
            isNeedIndepend = true;
        }
        else
        {
            isNeedIndepend = false;
        }      
        Angle = transform.position - Position;
        Position = transform.position;
        transform.position += Angle.normalized * Time.deltaTime;
    }

}

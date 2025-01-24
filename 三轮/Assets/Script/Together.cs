using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Together : MonoBehaviour
{
    // Start is called before the first frame update
    public Character character;
    public Vector3 Center;//群落中心
    public Vector3 Sum;//坐标总和
    public int Amount;//个数
    public float MaxDistance;//离群落中心最大距离
    public float Distance;//离群落中心距离
    public Vector3 angle;//返回群落中心角度
    public bool isNeedBack;//是否需要返回
    public Vector3 Angle;//行走方向
    public Vector3 Position;//上一帧位置
    public GameObject MinObject;//最近同类
    void Start()
    {
        Position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Find();
        if (Distance > MaxDistance && character.Stage == 0)
        {
            isNeedBack = true;
        }
        else
        {
            isNeedBack= false;
        }
        Angle=(transform.position - Position).normalized;
        Position = transform.position;
        Fixangle();
    }
    public void Find()
    {
        Amount = 0;
        Sum = Vector3.zero;       
        foreach (GameObject t in character.SameAnimal)
        {
            Sum += t.transform.position;
            Amount++;
        }
        Center=Sum/Amount;
        Distance=Vector3.Distance(Center, transform.position);
        angle = (Center - transform.position).normalized;
    }
    public void Fixangle()//修正角度
    {
        float mindistance=float.MaxValue;
        foreach (GameObject t in character.SameAnimal)
        {
           float distance=Vector3.Distance(transform.position,t.transform.position);
            if(distance<  mindistance)
            {
                MinObject = t;
                mindistance = distance;
            }
        }
        Vector3 fixangle=MinObject.GetComponent<Together>().Angle;
        Angle=fixangle;
        transform.position += Angle * Time.deltaTime;
    }
}

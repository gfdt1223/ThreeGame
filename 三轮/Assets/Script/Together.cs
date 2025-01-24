using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Together : MonoBehaviour
{
    // Start is called before the first frame update
    public Character character;
    public Vector3 Center;//Ⱥ������
    public Vector3 Sum;//�����ܺ�
    public int Amount;//����
    public float MaxDistance;//��Ⱥ������������
    public float Distance;//��Ⱥ�����ľ���
    public Vector3 angle;//����Ⱥ�����ĽǶ�
    public bool isNeedBack;//�Ƿ���Ҫ����
    public Vector3 Angle;//���߷���
    public Vector3 Position;//��һ֡λ��
    public GameObject MinObject;//���ͬ��
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
    public void Fixangle()//�����Ƕ�
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

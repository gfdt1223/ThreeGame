using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    public float MaxEnergy;//�������
    public float CurrentEnergy;//��������
    public float EnergyCost;//��������
    public float Age;//����
    public float MultiplyAge;//���Է�ֳ����
    public float Speed;//�ٶ�
    public bool isCanMultiply;//�Ƿ���Է�ֳ
    public int BirthAmount;//��ֳ�������
    public float BirthCold;//��ֳ��ȴʱ��
    public float BirthColdTimer;//��ֳ��ȴ��ʱ��
    public float MultiplyCost;//��ֳ��������   
    public GameObject[] SameAnimal;//����ͬ������
    public GameObject MultiplyTarget;//�������
    public int BabyAmount;//������������
    public float TargetDistance;//����������
    public Vector3 TargetAngle;//�������Ƕ�
    public GameObject Father;//������
    public GameObject Mother;//ĸ����
    public float mindistance;
    public int Stage;//״̬(0Ѳ�ߣ�1��ʳ��2��ֳ)
    void Start()
    {
        SameAnimal = GameObject.FindGameObjectsWithTag(tag);//Ѱ������ͬ��
        FindParents();
        if (Father != null && Mother != null)
        {
            Variation();
        }
        Age = 0;
        BabyAmount = 0;
        CurrentEnergy = MaxEnergy*0.5f;
        BirthColdTimer = BirthCold;
    }

    // Update is called once per frame
    void Update()
    {
        Age += Time.deltaTime*0.1f;//��������
        BirthColdTimer -= Time.deltaTime;   
        CurrentEnergy-=EnergyCost*Time.deltaTime;//��������
        SameAnimal = GameObject.FindGameObjectsWithTag(tag);//Ѱ������ͬ��
        if (CurrentEnergy > MultiplyCost && Age >= MultiplyAge && BirthColdTimer <= 0)//���Ϸ�ֳ����
        {
            isCanMultiply = true;
        }
        else
        {
            isCanMultiply = false;
        }
        FindClosestTarget();
        if (isCanMultiply && MultiplyTarget != null)//��ʼ��ֳ
        {
            Stage = 2;
            if (TargetDistance > 1)//����
            {
                transform.position += Speed * TargetAngle*Time.deltaTime;
            }
            else
            {
                Multiply();
            }
        }
        if(CurrentEnergy<=0)//����
        {            
            Destroy(gameObject);
        }
    }
    public void Multiply()//��ֳ
    {               
            FindBaby();
            while(BabyAmount<BirthAmount)
            {
                Instantiate(MultiplyTarget);               
                BabyAmount++;
            }
            CurrentEnergy -= MultiplyCost;
            BirthColdTimer=BirthCold;
        
    }
    public void FindClosestTarget()//Ѱ�����ͬ��
    {

        TargetDistance = float.MaxValue;
        MultiplyTarget = null;
        foreach (GameObject target in SameAnimal)
        {
            float distance=Vector3.Distance(transform.position,target.transform.position);
            if (distance < TargetDistance&&target.GetComponent<Character>().isCanMultiply&&target!=this.gameObject)
            {
                TargetDistance=distance;
                TargetAngle =new Vector3(target.transform.position.x-transform.position.x, target.transform.position.y - transform.position.y,0).normalized;
                MultiplyTarget=target;
            }
        }        
    }
    public void FindBaby()//Ѱ�Ҹ�������
    {
        BabyAmount = 0;
        foreach (GameObject baby in SameAnimal)
        {
            float distance = Vector3.Distance(transform.position, baby.transform.position);
            if (baby.GetComponent<Character>().Age <= 0.1f&&distance<=3)
            {
                BabyAmount++;
            }
        }
    }
    public void FindParents()//Ѱ�Ҹ�ĸ
    {
        mindistance = float.MaxValue;
        foreach (GameObject father in SameAnimal)
        {                    
            float distance=Vector3.Distance(transform.position,father.transform.position);           
                if (distance < mindistance && father != this.gameObject)
                {
                    Father = father;
                    mindistance = distance;
                }           
        }
       foreach(GameObject mother in SameAnimal)
        {
            float secondmindistance = float.MaxValue;
            float distance = Vector3.Distance(transform.position, mother.transform.position);
            if (distance > mindistance &&distance< secondmindistance&&mother != this.gameObject)
            { 
                Mother = mother;
                secondmindistance = distance;
            }
        }
    }
    public void Variation()//�̳б���
    {
        MaxEnergy = (Father.GetComponent<Character>().MaxEnergy + Mother.GetComponent<Character>().MaxEnergy)*0.4f + Random.Range(0, 30.0f)*0.2f;
        Speed = (Father.GetComponent<Character>().Speed + Mother.GetComponent<Character>().Speed) * 0.4f + Random.Range(0, 1.0f) * 0.2f;
        BirthAmount = (int)((Father.GetComponent<Character>().BirthAmount + Mother.GetComponent<Character>().BirthAmount) * 0.4f + Random.Range(0, 2.0f) * 0.2f);
        BirthCold = (Father.GetComponent<Character>().BirthCold + Mother.GetComponent<Character>().BirthCold) * 0.4f + Random.Range(0, 10.0f) * 0.2f;
    }
}

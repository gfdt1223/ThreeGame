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
    public float RunTime;//�����ʱ�䣨������
    public float RunTimer;//������ʱ��
    public float Age;//����
    public float OldAge;//˥������
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
    public int Stage;//״̬(-1������0Ѳ�ߣ�1��ʳ��2��ֳ,3����)
    public bool isdecide;//�Ƿ����Ѳ�߷���
    float walktimer;
    float x;
    float y;
    float ScaleX;
    float ScaleY;
    public GameObject RunTarget;//���ܶ���
    public GameObject[] DangerousAnimal;//��в����
    public Vector3 RunTargetAngle;//���ܶ�����
    public float LookDistance;//������
    public bool isFood;//�Ƿ�Ϊ����
    public Together Together;
    public bool isEndRest;//�����Ƿ�ָ�
    void Start()
    {
        SameAnimal = GameObject.FindGameObjectsWithTag(tag);//Ѱ������ͬ��
        if (isFood)
        {
            DangerousAnimal = GameObject.FindGameObjectsWithTag("wolf");//Ѱ����в����
        }
   
        FindParents();
        if (Father != null && Mother != null)
        {
            Variation();
        }
        Age = 0;
        BabyAmount = 0;
        CurrentEnergy = MaxEnergy*0.5f;
        BirthColdTimer = BirthCold;
        Stage = 0;
        RunTimer=RunTime;
        isEndRest = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(MultiplyTarget == null&&Stage==2)
        {
            Stage = 0;
        }
        if(BirthAmount == 0)//��ֹ�������С��1
        {
            BirthAmount = 1;
        }
        if (isFood)
        {
            DangerousAnimal = GameObject.FindGameObjectsWithTag("wolf");//Ѱ����в����
        }
        if (TargetDistance < 1 && Stage != 2&&Stage!=-1)
        {
            transform.position-=Speed*TargetAngle*Time.deltaTime;
        }
        ScaleX =transform.localScale.x;
        ScaleY=transform.localScale.y;
        if (Stage != -1)
        {
            Age += Time.deltaTime * 0.1f;//��������
            BirthColdTimer -= Time.deltaTime;
            CurrentEnergy -= EnergyCost * Time.deltaTime * 0.5f;//��������
            if (Stage != 3&&RunTimer<RunTime)
            {
                RunTimer += Time.deltaTime;
            }
            if(RunTimer>=RunTime*0.95f)
            {
                isEndRest=true;
            }
            if (DangerousAnimal != null)
            {
                FindClosestDanger();
            }
            if (RunTarget!=null&&isEndRest&&isFood&&RunTarget.GetComponent<Character>().Stage==3)//���ܻ�׷��
            {
                Stage = 3;
                if (Stage == 3)
                {
                    RunTimer -= Time.deltaTime;
                    if (isFood)
                    {
                        transform.position -= Speed * RunTargetAngle * Time.deltaTime * 1.5f;
                    }
                    else
                    {
                        transform.position += Speed * RunTargetAngle * Time.deltaTime * 1.5f;
                    }
                }
            }
            if (RunTimer < 0 && Stage == 3)
            {
                Stage = 0;
                isEndRest = false;
            }
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
            if (isCanMultiply && MultiplyTarget != null&&Stage!=3)//��ʼ��ֳ
            {
                Stage = 2;
                if (TargetDistance > 1)//����
                {
                    transform.position += Speed * TargetAngle * Time.deltaTime;
                }
                else
                {
                    Multiply();
                }
            }
            if (CurrentEnergy <= 0)//����
            {
                Destroy(gameObject);
            }
            if (Age >= OldAge)//˥��
            {
                MaxEnergy -= Time.deltaTime * 0.5f;
            }
            if (Stage == 0)
            {
                StartCoroutine("Walk");
            }
            else
            {
                StopCoroutine("Walk");
            }
            if (!IfAdd(transform.position.x) && ScaleX > 0)//�����߶���
            {
                transform.localScale = new Vector3(-ScaleX, ScaleY, 0);
            }
            if (IfAdd(transform.position.x) && ScaleX < 0)//�����߶���
            {
                transform.localScale = new Vector3(-ScaleX, ScaleY, 0);
            }
        }
        if(Stage==-1)
        {
            gameObject.tag = "body";
            Speed = 0;
            RunTimer = 0;
            StopAllCoroutines();
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
        Stage = 0;
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
    public void FindClosestDanger()//Ѱ�������в����
    {
        TargetDistance = float.MaxValue;
        RunTarget = null;
        foreach (GameObject target in DangerousAnimal)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < TargetDistance && target != this.gameObject&&distance<=LookDistance)
            {
                TargetDistance = distance;
                RunTargetAngle = new Vector3(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y, 0).normalized;
               RunTarget = target;
            }
            if (distance > LookDistance&&Stage==3)
            {
                RunTarget = null;
                Stage = 0;
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
        float RandomNumber = Random.Range(-1.0f, 1.0f);
        MaxEnergy = (Father.GetComponent<Character>().MaxEnergy + Mother.GetComponent<Character>().MaxEnergy+RandomNumber*10) * Random.Range(0.4f, 0.6f);
        Speed = (Father.GetComponent<Character>().Speed + Mother.GetComponent<Character>().Speed+ RandomNumber*0.2f) * Random.Range(0.4f, 0.6f);
        BirthAmount = (int)((Father.GetComponent<Character>().BirthAmount + Mother.GetComponent<Character>().BirthAmount+ RandomNumber) *Random.Range(0.4f, 0.6f));
        BirthCold = (Father.GetComponent<Character>().BirthCold + Mother.GetComponent<Character>().BirthCold+ RandomNumber*10) * Random.Range(0.4f, 0.6f);
    }
     IEnumerator Walk()
    {
        Vector3 angle=new Vector3(0,0,0);
        if (!isdecide)
        {
             walktimer = Random.Range(1.0f, 5.0f);
             x = Random.Range(-1f, 1f);
             y = Random.Range(-1f, 1f);
            angle = new Vector3(x, y, 0);
            isdecide = true;
            if (Together != null)
            {
                if (Together.isNeedBack)
                {
                    angle = Together.angle;
                }
                else
                {
                    angle = new Vector3(x, y, 0);
                }
            }
        }                  
        
        if (walktimer > 0)
        {
            transform.position += Speed * angle.normalized * Time.deltaTime;
        }
        else
        {
            isdecide=false;
        }
        walktimer -= Time.deltaTime;
        yield return null;
    }
    public bool IfAdd(float a)//�жϱ������ӻ����
    {
        float lasttime=0,lasta=0;        
        float change=a-lasta;
        if (change >= 0)
        {
            lasta = a;
            lasttime = Time.time;
            return true;
        }
        else
        {
            lasta = a;
            lasttime = Time.time;
            return false;
        }
        
    }
}

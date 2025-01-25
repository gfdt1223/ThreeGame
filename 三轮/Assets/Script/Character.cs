using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    public float MaxEnergy;//最大能量
    public float CurrentEnergy;//现有能量
    public float EnergyCost;//能量消耗
    public float RunTime;//最大奔跑时间（体力）
    public float RunTimer;//体力计时器
    public float Age;//年龄
    public float OldAge;//衰老年龄
    public float MultiplyAge;//可以繁殖年龄
    public float Speed;//速度
    public bool isCanMultiply;//是否可以繁殖
    public int BirthAmount;//繁殖后代数量
    public float BirthCold;//繁殖冷却时间
    public float BirthColdTimer;//繁殖冷却计时器
    public float MultiplyCost;//繁殖消耗能量   
    public GameObject[] SameAnimal;//所有同类生物
    public GameObject MultiplyTarget;//交配对象
    public int BabyAmount;//附近幼崽数量
    public float TargetDistance;//最近对象距离
    public Vector3 TargetAngle;//最近对象角度
    public GameObject Father;//父对象
    public GameObject Mother;//母对象
    public float mindistance;
    public int Stage;//状态(-1死亡，0巡逻，1觅食，2繁殖,3奔跑)
    public bool isdecide;//是否决定巡逻方向
    float walktimer;
    float x;
    float y;
    float ScaleX;
    float ScaleY;
    public GameObject RunTarget;//奔跑对象
    public GameObject[] DangerousAnimal;//威胁对象
    public Vector3 RunTargetAngle;//奔跑对象方向
    public float LookDistance;//侦察距离
    public bool isFood;//是否为猎物
    public Together Together;
    public bool isEndRest;//体力是否恢复
    void Start()
    {
        SameAnimal = GameObject.FindGameObjectsWithTag(tag);//寻找所有同类
        if (isFood)
        {
            DangerousAnimal = GameObject.FindGameObjectsWithTag("wolf");//寻找威胁对象
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
        if(BirthAmount == 0)//防止后代数量小于1
        {
            BirthAmount = 1;
        }
        if (isFood)
        {
            DangerousAnimal = GameObject.FindGameObjectsWithTag("wolf");//寻找威胁对象
        }
        if (TargetDistance < 1 && Stage != 2&&Stage!=-1)
        {
            transform.position-=Speed*TargetAngle*Time.deltaTime;
        }
        ScaleX =transform.localScale.x;
        ScaleY=transform.localScale.y;
        if (Stage != -1)
        {
            Age += Time.deltaTime * 0.1f;//年龄增长
            BirthColdTimer -= Time.deltaTime;
            CurrentEnergy -= EnergyCost * Time.deltaTime * 0.5f;//能量消耗
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
            if (RunTarget!=null&&isEndRest&&isFood&&RunTarget.GetComponent<Character>().Stage==3)//逃跑或追击
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
            SameAnimal = GameObject.FindGameObjectsWithTag(tag);//寻找所有同类
           
            if (CurrentEnergy > MultiplyCost && Age >= MultiplyAge && BirthColdTimer <= 0)//符合繁殖条件
            {
                isCanMultiply = true;
            }
            else
            {
                isCanMultiply = false;
            }
            FindClosestTarget();
            if (isCanMultiply && MultiplyTarget != null&&Stage!=3)//开始繁殖
            {
                Stage = 2;
                if (TargetDistance > 1)//靠近
                {
                    transform.position += Speed * TargetAngle * Time.deltaTime;
                }
                else
                {
                    Multiply();
                }
            }
            if (CurrentEnergy <= 0)//死亡
            {
                Destroy(gameObject);
            }
            if (Age >= OldAge)//衰老
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
            if (!IfAdd(transform.position.x) && ScaleX > 0)//向左走动画
            {
                transform.localScale = new Vector3(-ScaleX, ScaleY, 0);
            }
            if (IfAdd(transform.position.x) && ScaleX < 0)//向右走动画
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
    public void Multiply()//繁殖
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
    public void FindClosestTarget()//寻找最近同类
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
    public void FindClosestDanger()//寻找最近威胁对象
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
    public void FindBaby()//寻找附近幼崽
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
    public void FindParents()//寻找父母
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
    public void Variation()//继承变异
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
    public bool IfAdd(float a)//判断变量增加或减少
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

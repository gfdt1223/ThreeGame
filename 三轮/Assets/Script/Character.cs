using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Self;
    public float prebMaxEnergy;
    public float prebSpeed;
    public float prebBirthCold;
    public float MaxEnergy;//最大能量
    public float CurrentEnergy;//现有能量
    public float EnergyCost;//能量消耗
    public float Age;//年龄
    public float OldAge;//衰老年龄
    public float MultiplyAge;//可以繁殖年龄
    public float Speed;//速度
    public bool isCanMultiply;//是否可以繁殖
    public int BirthAmount;//繁殖后代数量
    public float BirthCold;//繁殖冷却时间
    public float BirthDuringTime;//繁殖过渡时间
    public float BirthColdTimer;//繁殖冷却计时器
    public float MultiplyCost;//繁殖消耗能量   
    public float GenesChangeRandom;//基因突变概率
    public float MultiplyRandom;//繁殖随机数
    public GameObject[] SameAnimal;//所有同类生物
    public GameObject MultiplyTarget;//交配对象
    public GameObject CloestTarget;//最近同类
    public float CloestTargetDistance;//最近同类距离
    public Vector3 CloestTargetAngle;//最近同类角度
    public GameObject[] Babys;//幼崽
    public int MultiplyAmount;//繁殖次数
    public int BabyAmount;//附近幼崽数量
    public float TargetDistance;//最近交配对象距离
    public Vector3 TargetAngle;//最近交配对象角度
    public GameObject Father;//父对象
    public GameObject Mother;//母对象
    public float mindistance;
    public int Stage;//状态(-1死亡，0巡逻，1觅食，2繁殖)
    public bool isdecide;//是否决定巡逻方向
    float walktimer;
    float x;
    float y;
    float ScaleX;
    float ScaleY;
    public float LookDistance;//侦察距离
    public bool isCanBeEat;//是否处于可以被捕食状态
    public Vector3 CurrentPosition;
    public Vector3 PositionAdd;
    public bool[] Genes;//基因库
    public GameObject[] DangerousAnimal;
    void Start()
    {
        Array.Resize(ref DangerousAnimal, 100);      
        BirthDuringTime = 0;
        isCanBeEat = true;
        TargetDistance = float.MaxValue;
        SameAnimal = GameObject.FindGameObjectsWithTag(tag);//寻找所有同类
        //FindParents();
        if (Father != null && Mother != null)
        {
            Variation();
        }
        MaxEnergy=prebMaxEnergy;
        Speed = prebSpeed;
        BirthCold = prebBirthCold;
        RandomGenes();
        for (int i = 0; i < Genes.Length; i++)//遍历基因并执行
        {
            if (Genes[i])
            {
                WorkGenes(i);
            }
        }
        Age = 0;
        BabyAmount = 0;
        CurrentEnergy =MultiplyCost*2;
        BirthColdTimer = BirthCold;
        Stage = 0;
        isdecide = false;
        CurrentPosition=transform.position;
        MultiplyAmount = 0;
        Array.Resize(ref Babys, 50);
        isCanMultiply = false;
    }

    // Update is called once per frame
    void Update()
    {
        BirthDuringTime-= Time.deltaTime;
        PositionAdd = transform.position-CurrentPosition;
        CurrentPosition = transform.position;
        if (this.gameObject.GetComponent<EatPlant>() != null)
        {
            EatMeat[] EatMeatObjects = FindObjectsOfType<EatMeat>();
            DangerousAnimal = new GameObject[EatMeatObjects.Length];
            for (int i = 0; i < EatMeatObjects.Length; i++)
            {
                DangerousAnimal[i] = EatMeatObjects[i].gameObject;
            }
        }
        if (MultiplyTarget == null&&Stage==2)
        {
            Stage = 0;
        }
        if(BirthAmount == 0)//防止后代数量小于1
        {
            BirthAmount = 1;
        }
        //if (TargetDistance < 1 && Stage != 2&&Stage!=-1)
        //{
        //    transform.position-=Speed*TargetAngle*Time.deltaTime;
        //}
        ScaleX =transform.localScale.x;
        ScaleY=transform.localScale.y;
        if (Stage != -1)
        {
            Age += Time.deltaTime * 0.02f;//年龄增长
            BirthColdTimer -= Time.deltaTime;
            CurrentEnergy -= EnergyCost * Time.deltaTime * 0.5f;//能量消耗                       
            SameAnimal = GameObject.FindGameObjectsWithTag(tag);//寻找所有同类
           if(CurrentEnergy>MaxEnergy)//超出能量加速消耗
            {
                CurrentEnergy-=EnergyCost*Time.deltaTime*0.5f;
            }
            if (CurrentEnergy > MultiplyCost && Age >= MultiplyAge&&Age<=OldAge && BirthColdTimer <= 0)//符合繁殖条件
            {
                isCanMultiply = true;
            }
            else
            {
                isCanMultiply = false;
            }
            if (MultiplyTarget != this.gameObject)
            {
                FindClosestTarget();
            }
            if (isCanMultiply && MultiplyTarget != null&&MultiplyTarget!=this.gameObject)//开始繁殖
            {
                Stage = 2;
                if (TargetDistance > 1)//靠近
                {
                    transform.position += Speed * TargetAngle * Time.deltaTime;
                }
                else
                {
                    Debug.Log("111");
                    Multiply();                    
                }
            }
            if (CurrentEnergy <= 0)//死亡
            {
                Destroy(gameObject);
            }
            if (Age >= OldAge)//衰老
            {
                MaxEnergy -= Time.deltaTime ;
                isCanMultiply=false;
            }
            if (Stage == 0)
            {
                StartCoroutine("Walk");
            }
            else
            {
                StopCoroutine("Walk");
            }
            if (PositionAdd.x<0 && ScaleX < 0)//向左走动画
            {
                transform.localScale = new Vector3(-ScaleX, ScaleY, 0);
            }
            if (PositionAdd.x >= 0 && ScaleX > 0)//向右走动画
            {
                transform.localScale = new Vector3(-ScaleX, ScaleY, 0);
            }
        }
        if(Stage==-1)
        {
            gameObject.tag = "body";
            Speed = 0;
            StopAllCoroutines();
        }
    }
    public void Multiply()//繁殖
    {
        BabyAmount = 0;
        MultiplyRandom = UnityEngine.Random.Range(-1.0f, 1.0f);
        //FindBaby();
        if (MultiplyRandom >= MultiplyTarget.GetComponent<Character>().MultiplyRandom)
        {
            while(BabyAmount < BirthAmount) 
            {
                Babys[BabyAmount + MultiplyAmount * BirthAmount]= Instantiate(Self);
                Babys[BabyAmount + MultiplyAmount * BirthAmount].GetComponent<Character>().Mother = this.gameObject;
                Babys[BabyAmount + MultiplyAmount * BirthAmount].GetComponent<Character>().Father = MultiplyTarget;
                BabyAmount++;          
            }
        }
          CurrentEnergy -= MultiplyCost;
          BirthColdTimer=BirthCold;
        BabyAmount = 0;
        Stage = 0;
        MultiplyAmount++;
        BirthDuringTime = 3;
        isCanMultiply = false;
    }
    public void FindClosestTarget()
    {
        if (BirthDuringTime < 0&&Stage!=2)
        {
            MultiplyTarget = null;
            TargetDistance = float.MaxValue;
        }
        CloestTarget = null;
        CloestTargetDistance = float.MaxValue;
        foreach (GameObject target in SameAnimal)
        {
            float distance=Vector3.Distance(transform.position,target.transform.position);
            if (distance < TargetDistance&&(target.GetComponent<Character>().isCanMultiply||Stage==2)&&target!=this.gameObject)//寻找最近可繁殖同类
            {
                TargetDistance=distance;
                TargetAngle =new Vector3(target.transform.position.x-transform.position.x, target.transform.position.y - transform.position.y,0).normalized;
                MultiplyTarget=target;
            }
            if (distance < CloestTargetDistance && target != this.gameObject)//寻找最近同类
            {
                CloestTargetDistance = distance;
                CloestTargetAngle = new Vector3(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y, 0).normalized;
                CloestTarget = target;
            }
        }        
    }
    //public void FindClosestDanger()//寻找最近威胁对象
    //{
    //    TargetDistance = float.MaxValue;
    //    RunTarget = null;
    //    foreach (GameObject target in DangerousAnimal)
    //    {
    //        float distance = Vector3.Distance(transform.position, target.transform.position);
    //        if (distance < TargetDistance && target != this.gameObject&&distance<=LookDistance)
    //        {
    //            TargetDistance = distance;
    //            RunTargetAngle = new Vector3(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y, 0).normalized;
    //           RunTarget = target;
    //        }
    //        if (distance > LookDistance&&Stage==3)
    //        {
    //            RunTarget = null;
    //            Stage = 0;
    //        }
    //    }
    //}
    public void FindBaby()//寻找附近幼崽
    {
        BabyAmount = 0;
        foreach (GameObject baby in SameAnimal)
        {
            float distance = Vector3.Distance(transform.position, baby.transform.position);
            if (baby.GetComponent<Character>().Age <= 0.4f && distance <= 2)
            {
                BabyAmount++;
            }
        }
    }
    //public void FindParents()//寻找父母
    //{
    //    mindistance = float.MaxValue;
    //    foreach (GameObject father in SameAnimal)
    //    {                    
    //        float distance=Vector3.Distance(transform.position,father.transform.position);           
    //            if (distance < mindistance && father != this.gameObject)
    //            {
    //                Father = father;
    //                mindistance = distance;
    //            }           
    //    }
    //   foreach(GameObject mother in SameAnimal)
    //    {
    //        float secondmindistance = float.MaxValue;
    //        float distance = Vector3.Distance(transform.position, mother.transform.position);
    //        if (distance > mindistance &&distance< secondmindistance&&mother != this.gameObject)
    //        { 
    //            Mother = mother;
    //            secondmindistance = distance;
    //        }
    //    }
    //}
    public void Variation()//继承变异
    {
        float RandomNumber = UnityEngine.Random.Range(-1.0f, 1.0f);
        prebMaxEnergy = (Father.GetComponent<Character>().prebMaxEnergy + Mother.GetComponent<Character>().prebMaxEnergy+RandomNumber*10) * UnityEngine.Random.Range(0.4f, 0.6f);
        prebSpeed = (Father.GetComponent<Character>().prebSpeed + Mother.GetComponent<Character>().prebSpeed + RandomNumber*0.2f) * UnityEngine.Random.Range(0.4f, 0.6f);
        prebBirthCold = (Father.GetComponent<Character>().prebBirthCold + Mother.GetComponent<Character>().prebBirthCold + RandomNumber * 10) * UnityEngine.Random.Range(0.4f, 0.6f);
        for (int i = 0; i < Genes.Length; i++)
        {
            if (Father.GetComponent<Character>().Genes[i] && Mother.GetComponent<Character>().Genes[i])
            {
                Genes[i] = true;
            }
            else if (Father.GetComponent<Character>().Genes[i] || Mother.GetComponent<Character>().Genes[i])
            {
                float random = UnityEngine.Random.Range(-1.0f, 1.0f);
                if (random > 0)
                {
                    Genes[i] = true;
                }
                else
                {
                    Genes[i] = false;
                }
            }
            else
            {
                Genes[i] = false;
            }
        }
    }
     IEnumerator Walk()
    {
        Vector3 angle=new Vector3(0,0,0);
        if (!isdecide)
        {
             walktimer = UnityEngine.Random.Range(1.0f, 5.0f);
             x = UnityEngine.Random.Range(-1f, 1f);
             y = UnityEngine.Random.Range(-1f, 1f);
            angle = new Vector3(x, y, 0);
            isdecide = true;
            if (this.gameObject.GetComponent<Independent>())
            {
                //Debug.Log("777");
                if (this.gameObject.GetComponent<Independent>().isNeedIndepend)
                {
                    angle = -CloestTargetAngle;
                }
                else
                {
                    angle = new Vector3(x, y, 0);
                }
            }
            if (this.gameObject.GetComponent<Together>())
            {
                if (this.gameObject.GetComponent<Together>().isNeedBack)
                {
                    angle = this.GetComponent<Together>().angle;
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
    public void RandomGenes()//基因突变
    {
        for (int i = 0; i < Genes.Length; i++) {
            float random = UnityEngine.Random.Range(0, 1.0f);
            if (random <= GenesChangeRandom)
            {
                if (Genes[i])
                {
                    Genes[i] = false;
                }
                else
                {
                    Genes[i]= true;
                }
            }
        }
    }
    public void WorkGenes(int i)//执行基因
    {
        switch(i)
        {
            case 0://疯狂繁殖：繁殖速度翻倍，繁殖消耗减少，寿命减半
                MultiplyAge = MultiplyAge * 0.5f;
                MultiplyCost = MultiplyCost * 0.5f;
                BirthCold = BirthCold * 0.5f;
                OldAge = OldAge * 0.5f;
                break;
            case 1://吃货：最大能量翻倍，速度减少，能量消耗加快
                MaxEnergy = MaxEnergy * 2;
                Speed = Speed * 0.66f;
                EnergyCost = EnergyCost * 1.2f;
                break;
            case 2://安于现状：侦察范围减半，寿命增加
                LookDistance = LookDistance * 0.5f;
                OldAge = OldAge * 1.5f;
                break;
            case 3://懒惰：能量消耗减少，繁殖冷却延长
                EnergyCost = EnergyCost * 0.5f;
                BirthCold = BirthCold * 2;
                break;
            case 4://精力充沛：能量消耗加快，其他属性上升
                EnergyCost = EnergyCost * 1.5f;
                MaxEnergy = MaxEnergy * 1.3f;
                Speed = Speed * 1.3f;
                LookDistance = LookDistance * 1.3f;
                OldAge = OldAge * 1.3f;
                BirthCold = BirthCold * 0.66f;
                break;
            case 5://差生：所有属性下降
                MaxEnergy = MaxEnergy * 0.66f;
                Speed = Speed * 0.66f;
                LookDistance = LookDistance * 0.66f;
                OldAge = OldAge * 0.66f;
                BirthCold = BirthCold * 1.3f;
                break;
            case 6://分食：在能量充足时分给能量最少个体
                StartCoroutine("Gene6");
                break;
            case 7://护子：幼崽遭受攻击时有概率由父母抵挡
                StartCoroutine("Gene7");
                break;
            case 8://合理分配：能量高时繁殖冷却减少，能量消耗加快；能量低时能量消耗减慢
                StartCoroutine("Gene8");
                break;
            case 9://充分消化：吃掉食物获得的能量更多
                if (this.GetComponent<EatMeat>() != null)
                {
                    this.GetComponent<EatMeat>().FoodTakein += 0.2f;
                }
                else
                {
                    this.GetComponent<EatPlant>().EatAmount *= 2;
                }
                break;
            case 10://克隆：繁殖方式变为自我复制，子代会继承全部基因和属性
                StartCoroutine("Gene10");
                break;
        }
        
    }
    IEnumerator Gene6()
    {
        while (true)
        {
            GameObject MinEnergyAnimal = null;
            float MinEnergy = float.MaxValue;
            float AnimalMaxEnergy = 0;
            if (CurrentEnergy >= MaxEnergy * 0.9f)
            {
                foreach (GameObject animal in SameAnimal)
                {
                    if (animal.GetComponent<Character>().CurrentEnergy < MinEnergy)
                    {
                        MinEnergy = animal.GetComponent<Character>().CurrentEnergy;
                        AnimalMaxEnergy = animal.GetComponent<Character>().MaxEnergy;
                        MinEnergyAnimal = animal;
                    }
                }
                if (MinEnergy < AnimalMaxEnergy * 0.5f)
                {
                    CurrentEnergy -= (CurrentEnergy + MinEnergy) / 2;
                    MinEnergyAnimal.GetComponent<Character>().CurrentEnergy += (CurrentEnergy + MinEnergy) / 2;
                }
            }
            yield return null;
        }   
    }
    IEnumerator Gene7()
    {
        while (true)
        {
            foreach (GameObject animal in DangerousAnimal)
            {
                if (animal.GetComponent<EatMeat>().EatTarget == this.gameObject)
                {
                    float random = UnityEngine.Random.Range(-1.0f, 1.0f);
                    if (random > 0.0f && (Father != null || Mother != null))
                    {
                        if (Mother != null)
                        {
                            animal.GetComponent<EatMeat>().EatTarget = Mother;
                        }
                        else
                        {
                            animal.GetComponent<EatMeat>().EatTarget = Father;
                        }
                    }
                }
            }
            yield return null;
        }
    }
    IEnumerator Gene8()
    {
        while (true)
        {
            if (CurrentEnergy >= MaxEnergy * 0.7f)
            {
                BirthColdTimer -= 0.5f * Time.deltaTime;
                CurrentEnergy -= EnergyCost * Time.deltaTime * 0.2f;
            }
            else
            {
                CurrentEnergy += EnergyCost * Time.deltaTime * 0.2f;
            }
            yield return null;
        }
    }
    IEnumerator Gene10()
    {
        while (true)
        {
            Debug.Log("888");
            MultiplyTarget = this.gameObject;
            if (isCanMultiply)
            {
                Stage = 2;
                Multiply();
                isCanMultiply = false;
            }
            yield return null;
        }
    }
}

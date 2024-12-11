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
    public float Age;//年龄
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
    public int Stage;//状态(0巡逻，1觅食，2繁殖)
    void Start()
    {
        SameAnimal = GameObject.FindGameObjectsWithTag(tag);//寻找所有同类
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
        Age += Time.deltaTime*0.1f;//年龄增长
        BirthColdTimer -= Time.deltaTime;   
        CurrentEnergy-=EnergyCost*Time.deltaTime;//能量消耗
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
        if (isCanMultiply && MultiplyTarget != null)//开始繁殖
        {
            Stage = 2;
            if (TargetDistance > 1)//靠近
            {
                transform.position += Speed * TargetAngle*Time.deltaTime;
            }
            else
            {
                Multiply();
            }
        }
        if(CurrentEnergy<=0)//死亡
        {            
            Destroy(gameObject);
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
        MaxEnergy = (Father.GetComponent<Character>().MaxEnergy + Mother.GetComponent<Character>().MaxEnergy)*0.4f + Random.Range(0, 30.0f)*0.2f;
        Speed = (Father.GetComponent<Character>().Speed + Mother.GetComponent<Character>().Speed) * 0.4f + Random.Range(0, 1.0f) * 0.2f;
        BirthAmount = (int)((Father.GetComponent<Character>().BirthAmount + Mother.GetComponent<Character>().BirthAmount) * 0.4f + Random.Range(0, 2.0f) * 0.2f);
        BirthCold = (Father.GetComponent<Character>().BirthCold + Mother.GetComponent<Character>().BirthCold) * 0.4f + Random.Range(0, 10.0f) * 0.2f;
    }
}

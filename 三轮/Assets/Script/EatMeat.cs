using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EatMeat : MonoBehaviour
{
    // Start is called before the first frame update
    public Character character;
    public GameObject[] AnimalCanBeEat;//猎物
    public GameObject[] Body;//尸体
    public float EatDistance;//吃尸体距离
    public GameObject EatTarget;//捕猎对象
    public GameObject BodyTarget;//获取食物对象
    public float EatAmount;//单次获取能量
    public float EatCold;//获取能量冷却时间
    public float EatColdTimer;//获取能量冷却计时器
    public float mindistance;//最小距离
    public bool isDuring;//是否正在捕食
    void Start()
    {
        isDuring=false;
    }

    // Update is called once per frame
    void Update()
    {
        if(character.Stage!=-1)
        {
            AnimalCanBeEat = GameObject.FindGameObjectsWithTag("sheep");
            Body = GameObject.FindGameObjectsWithTag("body");
            EatColdTimer -= Time.deltaTime;
            if (Body != null)
            {
                FindBody();
            }
            FindAnimal();
            if (EatTarget == null && character.Stage == 3)
            {
                character.Stage = 0;
            }
            if (character.MaxEnergy - character.CurrentEnergy > EatAmount && EatTarget != null && character.RunTimer >=character.RunTime*0.95f&&!isDuring&&BodyTarget==null)
            {
                
                    character.Stage = 3;
                    isDuring = true;                                              
            }
            if (isDuring)
            {
                StartCoroutine(RunToAnimal());
            }
            else
            {
                StopCoroutine(RunToAnimal());
            }
            if(character.MaxEnergy-character.CurrentEnergy<EatAmount||EatTarget==null||Body!=null)
            {
                isDuring = false;
                character.Stage = 0;
            }
                      
        }
    }
    public void FindAnimal()//寻找猎物
    {
        mindistance = float.MaxValue;
        foreach (GameObject animal in AnimalCanBeEat)
        {
            if (animal != null)
            {
                float distance = Vector3.Distance(transform.position, animal.transform.position);
                if (distance < mindistance&&distance<=character.LookDistance && animal.GetComponent<Character>().Stage != -1)
                {
                    mindistance = distance;
                    EatTarget = animal;
                }
                if(character.LookDistance<distance)
                {
                    EatTarget = null;
                }
            }
        }
    }
     IEnumerator RunToAnimal()//追击猎物
    {
        if (mindistance > EatDistance&&character.RunTimer>0&&EatTarget!=null&&character.Stage==3)//向猎物方向移动
        {
            character.RunTimer-=Time.deltaTime;
            transform.position += character.Speed * (EatTarget.transform.position - transform.position).normalized * Time.deltaTime * 1.5f;
        }
        if (mindistance <= EatDistance)//使猎物变为尸体
        {
            EatTarget.GetComponent<Character>().Stage = -1;          
                character.Stage = 0;
                isDuring = false;
                yield return null;
            
        }
        
        
    }
    public void FindBody()//寻找尸体
    {
        mindistance = float.MaxValue;
        foreach (GameObject animal in Body)
        {
            if (animal != null)
            {
                float distance = Vector3.Distance(transform.position, animal.transform.position);
                if (distance < mindistance)
                {
                    mindistance = distance;
                    BodyTarget = animal;
                }
            }
           
        }
        if (BodyTarget != null)
        {
            if (mindistance > EatDistance)
            {
                transform.position += character.Speed * (EatTarget.transform.position - transform.position).normalized * Time.deltaTime;
            }
            else if(EatColdTimer<=0)
            {
                if (BodyTarget.GetComponent<Character>().CurrentEnergy >= EatAmount)//获取尸体能量
                {
                    BodyTarget.GetComponent<Character>().CurrentEnergy -= EatAmount;
                    character.CurrentEnergy += EatAmount;
                }
                else
                {
                    character.CurrentEnergy += BodyTarget.GetComponent<Character>().CurrentEnergy;
                    BodyTarget.GetComponent<Character>().CurrentEnergy =0;                    
                }
                if (BodyTarget.GetComponent<Character>().CurrentEnergy <= 0)//尸体能量小于零摧毁尸体
                {
                    Destroy(BodyTarget);
                }
                EatColdTimer = EatCold;
                if (character.MaxEnergy - character.CurrentEnergy < EatAmount && character.Stage != 2)//回到巡逻状态
                {
                    character.Stage = 0;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(character.Stage!=-1)
        {
            AnimalCanBeEat = GameObject.FindGameObjectsWithTag("sheep");
            Body = GameObject.FindGameObjectsWithTag("body");
            EatColdTimer -= Time.deltaTime;
            FindAnimal();
            if (character.MaxEnergy - character.CurrentEnergy > EatAmount && EatTarget != null)
            {
                character.Stage = 3;
                RunToAnimal();
            }
            else if(character.Stage!=-1&& character.Stage != 1&&character.Stage != 2)
            {
                character.Stage = 0;
            }
            if (Body != null)
            {
                FindBody();
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
                if (distance < mindistance&&distance<=character.LookDistance)
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
    public void RunToAnimal()//追击猎物
    {
        if (mindistance > EatDistance)
        {
            transform.position += character.Speed * (EatTarget.transform.position - transform.position).normalized * Time.deltaTime;
        }
        else 
        {
            EatTarget.GetComponent<Character>().Stage = -1;
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
        if (mindistance > EatDistance)
        {
            transform.position += character.Speed * (EatTarget.transform.position - transform.position).normalized * Time.deltaTime;
        }
        else
        {
            BodyTarget.GetComponent<Character>().CurrentEnergy -= EatAmount;
            character.CurrentEnergy += EatAmount;
            if (BodyTarget.GetComponent<Character>().CurrentEnergy <= 0)
            {
                Destroy(BodyTarget);
            }
            EatColdTimer = EatCold;
        }
    }
}

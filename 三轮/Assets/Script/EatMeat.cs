using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EatMeat : MonoBehaviour
{
    // Start is called before the first frame update
    public Character character;
    public GameObject[] AnimalCanBeEat;//猎物
    public GameObject EatTarget;//捕猎对象
    public float EatCold;//获取能量冷却时间
    public float EatColdTimer;//获取能量冷却计时器
    public float mindistance;//最小距离
    public float DefendRandom;//抵抗随机数
    public float FoodTakein;//消化速率
    public string[] EatTag;

    void Start()
    {
        Array.Resize(ref AnimalCanBeEat, 100);
    }

    // Update is called once per frame
    void Update()
    {
        if (character.Stage != -1)
        {
            for (int i = 0; i < EatTag.Length; i++)
            {
                if (i == 0)
                {
                    GameObject.FindGameObjectsWithTag(EatTag[i]).CopyTo(AnimalCanBeEat, 0);
                }
                else
                {
                    GameObject.FindGameObjectsWithTag(EatTag[i]).CopyTo(AnimalCanBeEat, GameObject.FindGameObjectsWithTag(EatTag[i - 1]).Length);
                }

            }
            EatColdTimer -= Time.deltaTime;
            if (character.Stage != 3)
            {
                FindAnimal();
            }
            if (character.MaxEnergy - character.CurrentEnergy > character.MultiplyCost && EatTarget != null&&character.Stage!=3)
            {
                character.Stage = 1;
            }
            if ((character.MaxEnergy - character.CurrentEnergy < character.MultiplyCost || EatTarget == null) && character.Stage == 1)
            {
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
                if (distance < mindistance && distance <= character.LookDistance && animal.GetComponent<Character>().isCanBeEat)
                {
                    mindistance = distance;
                    EatTarget = animal;
                }
                if (character.LookDistance < mindistance)
                {
                    EatTarget = null;
                }
                if (distance <= 1 && character.Stage == 1)
                {
                    if (character.Speed < EatTarget.GetComponent<Character>().Speed)
                    {
                        float RandomNumber = UnityEngine.Random.Range(-1.0f, 1.0f);
                        if (RandomNumber < 0)
                        {
                            character.CurrentEnergy += 0.5f * EatTarget.GetComponent<Character>().CurrentEnergy;
                            Destroy(EatTarget);
                        }
                        else
                        {
                            Debug.Log("1");
                            character.Stage = 3;
                        }
                    }
                    else
                    {
                        character.CurrentEnergy += 0.5f * EatTarget.GetComponent<Character>().CurrentEnergy;
                        Destroy(EatTarget);
                    }

                }


                if (distance > 1 && EatTarget != null && character.Stage == 1)//向猎物方向移动
                {
                    transform.position += character.Speed * (EatTarget.transform.position - transform.position).normalized * Time.deltaTime * 0.05f;
                    character.CurrentEnergy -= Time.deltaTime * character.EnergyCost * 0.1f;
                }

            }
        }
    }
}

    // IEnumerator RunToAnimal()//追击猎物
    //{
    //    if (mindistance > EatDistance&&EatTarget!=null&&character.Stage==3)//向猎物方向移动
    //    {            
    //        transform.position += character.Speed * (EatTarget.transform.position - transform.position).normalized * Time.deltaTime * 1.5f;
    //    }
    //    if (mindistance <= EatDistance)//使猎物变为尸体
    //    {
    //        EatTarget.GetComponent<Character>().Stage = -1;          
    //            character.Stage = 0;
    //            isDuring = false;
    //            yield return null;
            
    //    }
        
        
    //}
//    public void FindBody()//寻找尸体
//    {
//        mindistance = float.MaxValue;
//        foreach (GameObject animal in Body)
//        {
//            if (animal != null)
//            {
//                float distance = Vector3.Distance(transform.position, animal.transform.position);
//                if (distance < mindistance)
//                {
//                    mindistance = distance;
//                    BodyTarget = animal;
//                }
//            }
           
//        }
//        if (BodyTarget != null)
//        {
//            if (mindistance > EatDistance)
//            {
//                transform.position += character.Speed * (EatTarget.transform.position - transform.position).normalized * Time.deltaTime;
//            }
//            else if(EatColdTimer<=0)
//            {
//                if (BodyTarget.GetComponent<Character>().CurrentEnergy >= EatAmount)//获取尸体能量
//                {
//                    BodyTarget.GetComponent<Character>().CurrentEnergy -= EatAmount;
//                    character.CurrentEnergy += EatAmount;
//                }
//                else
//                {
//                    character.CurrentEnergy += BodyTarget.GetComponent<Character>().CurrentEnergy;
//                    BodyTarget.GetComponent<Character>().CurrentEnergy =0;                    
//                }
//                if (BodyTarget.GetComponent<Character>().CurrentEnergy <= 0)//尸体能量小于零摧毁尸体
//                {
//                    Destroy(BodyTarget);
//                }
//                EatColdTimer = EatCold;
//                if (character.MaxEnergy - character.CurrentEnergy < EatAmount && character.Stage != 2)//回到巡逻状态
//                {
//                    character.Stage = 0;
//                }
//            }
//        }
//    }
//}

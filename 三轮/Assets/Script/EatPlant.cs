using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EatPlant : MonoBehaviour
{
    // Start is called before the first frame update
    public Character character;
    public GameObject[] Plant;
    public float EatDistance;//进食距离
    public GameObject EatTarget;
    public float EatAmount;//单次获取能量
    public float EatCold;//获取能量冷却时间
    public float EatColdTimer;//获取能量冷却计时器
    public float mindistance;//最小距离
    public string[] EatTag;
    public float speed;//存储速度
    public bool isGetSpeed;//是否获取速度
    void Start()
    {
        Array.Resize(ref Plant, 600);
        isGetSpeed=false;
        speed=character.Speed;
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
                    GameObject.FindGameObjectsWithTag(EatTag[i]).CopyTo(Plant, 0);
                }
                else
                {
                    GameObject.FindGameObjectsWithTag(EatTag[i]).CopyTo(Plant, GameObject.FindGameObjectsWithTag(EatTag[i - 1]).Length);
                }
            }
            EatColdTimer -= Time.deltaTime;
            FindPlant();
            if (character.MaxEnergy - character.CurrentEnergy > EatAmount && EatTarget != null&&character.Stage!=2)
            {
                if (!character.Genes[14])
                {
                    character.Speed = speed;
                    character.Stage = 1;
                    Eat();
                }
                else if(Plant.Length >30)
                {
                    character.Speed=speed;
                    character.Stage = 1;
                    Eat();
                }
                else
                {
                    character.Stage = 0;
                    character.CurrentEnergy += 0.2f;
                    if (!isGetSpeed)
                    {
                        speed = character.Speed;
                        isGetSpeed = true;
                    }
                    character.Speed = 0.2f*speed;
                }
            }
            else if (character.MaxEnergy - character.CurrentEnergy < EatAmount&&character.Stage!=2)
            {
                character.Stage = 0;
            }
        }
    }
    public void FindPlant()
    {
         mindistance=float.MaxValue;
        foreach (GameObject plant in Plant)
        {
            if (plant != null)
            {
                float distance = Vector3.Distance(transform.position, plant.transform.position);
                if (distance < mindistance)
                {
                    mindistance = distance;
                    EatTarget = plant;
                }
            }
        }
    }
    public void Eat()
    {
        if (mindistance > EatDistance)
        {
            transform.position += character.Speed * (EatTarget.transform.position - transform.position).normalized*Time.deltaTime;
        }
        else if(EatColdTimer<0)
        {
            EatTarget.GetComponent<Plant>().CurrentEnergy-=EatAmount;
            character.CurrentEnergy += EatAmount;
            if (EatTarget.GetComponent<Plant>().CurrentEnergy <= 0)
            {
                Destroy(EatTarget);
            }
            EatColdTimer = EatCold;
        }
       
    }
}

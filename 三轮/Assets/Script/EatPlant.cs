using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Plant = GameObject.FindGameObjectsWithTag("grass");
        EatColdTimer -= Time.deltaTime;
        FindPlant();
        if(character.MaxEnergy-character.CurrentEnergy>EatAmount)
        {
            character.Stage = 1;
            Eat();
        }
    }
    public void FindPlant()
    {
         mindistance=float.MaxValue;
        foreach (GameObject plant in Plant)
        {
            float distance = Vector3.Distance(transform.position, plant.transform.position);
            if (distance< mindistance)
            {
                mindistance = distance;
                EatTarget=plant;
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
            EatColdTimer = EatCold;
        }
       
    }
}

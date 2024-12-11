using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatPlant : MonoBehaviour
{
    // Start is called before the first frame update
    public Character character;
    public GameObject[] Plant;
    public float EatDistance;//��ʳ����
    public GameObject EatTarget;
    public float EatAmount;//���λ�ȡ����
    public float EatCold;//��ȡ������ȴʱ��
    public float EatColdTimer;//��ȡ������ȴ��ʱ��
    public float mindistance;//��С����
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

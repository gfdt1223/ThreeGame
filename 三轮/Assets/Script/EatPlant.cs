using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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
        if (character.Stage != -1)
        {
            Plant = GameObject.FindGameObjectsWithTag("grass");
            EatColdTimer -= Time.deltaTime;
            FindPlant();
            if (character.MaxEnergy - character.CurrentEnergy > EatAmount && EatTarget != null&&character.Stage!=3)
            {
                character.Stage = 1;
                Eat();
            }
            else if (character.Stage != 2)
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

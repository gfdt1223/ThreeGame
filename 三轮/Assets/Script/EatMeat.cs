using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatMeat : MonoBehaviour
{
    // Start is called before the first frame update
    public Character character;
    public GameObject[] AnimalCanBeEat;//����
    public GameObject[] Body;//ʬ��
    public float EatDistance;//��ʬ�����
    public GameObject EatTarget;//���Զ���
    public GameObject BodyTarget;//��ȡʳ�����
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
    public void FindAnimal()//Ѱ������
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
    public void RunToAnimal()//׷������
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
    public void FindBody()//Ѱ��ʬ��
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

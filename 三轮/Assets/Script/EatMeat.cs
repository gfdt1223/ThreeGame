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
    public bool isDuring;//�Ƿ����ڲ�ʳ
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
            if(character.MaxEnergy-character.CurrentEnergy<EatAmount||EatTarget==null)
            {
                isDuring = false;
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
     IEnumerator RunToAnimal()//׷������
    {
        if (mindistance > EatDistance&&character.RunTimer>0&&EatTarget!=null)
        {
            character.RunTimer-=Time.deltaTime;
            transform.position += character.Speed * (EatTarget.transform.position - transform.position).normalized * Time.deltaTime * 1.5f;
        }
        else if (mindistance <= EatDistance)
        {
            EatTarget.GetComponent<Character>().Stage = -1;
        }
        else
        {
            character.Stage = 0;
            isDuring=false;
            yield return null;
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
        if (BodyTarget != null)
        {
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
                if (character.MaxEnergy - character.CurrentEnergy < EatAmount && character.Stage != 2)
                {
                    character.Stage = 0;
                }
            }
        }
    }
}

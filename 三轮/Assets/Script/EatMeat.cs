using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class EatMeat : MonoBehaviour
{
    // Start is called before the first frame update
    public Character character;
    public GameObject[] AnimalCanBeEat;//����
    public GameObject EatTarget;//���Զ���
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
            AnimalCanBeEat = GameObject.FindGameObjectsWithTag("sheep");
            EatColdTimer -= Time.deltaTime;
            FindAnimal();          
            if (character.MaxEnergy - character.CurrentEnergy > character.MultiplyCost && EatTarget != null)
            {
                character.Stage = 1;
            }
            if ((character.MaxEnergy - character.CurrentEnergy < character.MultiplyCost || EatTarget == null) && character.Stage == 1)
            {
                character.Stage = 0;
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
                if (distance < mindistance && distance <= character.LookDistance)
                {
                    mindistance = distance;
                    EatTarget = animal;
                }
                if (character.LookDistance < distance)
                {
                    EatTarget = null;
                }
                if (distance <= 1 && character.Stage == 1)
                {
                    character.CurrentEnergy += 0.5f * EatTarget.GetComponent<Character>().CurrentEnergy;
                    Destroy(EatTarget);
                }
                if (distance > 1 && EatTarget != null && character.Stage == 1)//�����﷽���ƶ�
                {            
                     transform.position += character.Speed * (EatTarget.transform.position - transform.position).normalized * Time.deltaTime * 0.05f;
                    character.CurrentEnergy-=Time.deltaTime*character.EnergyCost*0.2f;
                }
            }
        }
    }
}
    // IEnumerator RunToAnimal()//׷������
    //{
    //    if (mindistance > EatDistance&&EatTarget!=null&&character.Stage==3)//�����﷽���ƶ�
    //    {            
    //        transform.position += character.Speed * (EatTarget.transform.position - transform.position).normalized * Time.deltaTime * 1.5f;
    //    }
    //    if (mindistance <= EatDistance)//ʹ�����Ϊʬ��
    //    {
    //        EatTarget.GetComponent<Character>().Stage = -1;          
    //            character.Stage = 0;
    //            isDuring = false;
    //            yield return null;
            
    //    }
        
        
    //}
//    public void FindBody()//Ѱ��ʬ��
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
//                if (BodyTarget.GetComponent<Character>().CurrentEnergy >= EatAmount)//��ȡʬ������
//                {
//                    BodyTarget.GetComponent<Character>().CurrentEnergy -= EatAmount;
//                    character.CurrentEnergy += EatAmount;
//                }
//                else
//                {
//                    character.CurrentEnergy += BodyTarget.GetComponent<Character>().CurrentEnergy;
//                    BodyTarget.GetComponent<Character>().CurrentEnergy =0;                    
//                }
//                if (BodyTarget.GetComponent<Character>().CurrentEnergy <= 0)//ʬ������С����ݻ�ʬ��
//                {
//                    Destroy(BodyTarget);
//                }
//                EatColdTimer = EatCold;
//                if (character.MaxEnergy - character.CurrentEnergy < EatAmount && character.Stage != 2)//�ص�Ѳ��״̬
//                {
//                    character.Stage = 0;
//                }
//            }
//        }
//    }
//}

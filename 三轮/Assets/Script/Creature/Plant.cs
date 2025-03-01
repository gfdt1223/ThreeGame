using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Plant : MonoBehaviour
{

    // Start is called before the first frame update
    private Tilemap Tilemap;
    public float GrowCold;//������ȴ
    public float GrowColdTimer;//������ȴ��ʱ��
    public float MaxAmount;//�������ֲ����
    public float CirleDistance;//��������
    public float PlantAmount;//����ֲ������
    public GameObject[] AllPlant;//����ֲ��   
    public float AllPlantAmount;//����ֲ������
    public float MaxEnergy;//�������
    public float CurrentEnergy;//��������
    public TileBase grassTile;
    void Start()
    {
        GrowColdTimer=GrowCold;
        CurrentEnergy=MaxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        AllPlant = GameObject.FindGameObjectsWithTag(tag);
        IfGrow();
        if (CurrentEnergy < MaxAmount)//�������
        {
            CurrentEnergy += Time.deltaTime * 0.1f;
        }
        if (CurrentEnergy <= 0)//����
        {
            Destroy(gameObject);
        }       
        GrowColdTimer-=Time.deltaTime;    
        FindPlant();
        if(GrowColdTimer < 0 &&PlantAmount<=MaxAmount&&AllPlantAmount<=300)//����
        {
            float x = transform.position.x + Random.Range(-3.0f,3.0f);
            float y = transform.position.y + Random.Range(-3.0f, 3.0f);
            Instantiate(this,new Vector3(x,y,0),Quaternion.identity);
            GrowColdTimer = GrowCold;
        }
    }
    public void FindPlant()
    {
        PlantAmount = 0;
        AllPlantAmount = 0;
        if (AllPlant != null)
        {
            foreach (GameObject plant in AllPlant)
            {
                float distance = Vector3.Distance(transform.position, plant.transform.position);
                AllPlantAmount++;
                if (distance <= CirleDistance)
                {
                    PlantAmount++;
                }
            }
        }
    }

    public void IfGrow()
    {
        Tilemap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        // ��ȡԤ�������������
        Vector3 worldPosition = gameObject.transform.position;

        // ����������ת��Ϊ��Ƭ��ͼ�ĵ�Ԫ������
        Vector3Int cellPosition = Tilemap.WorldToCell(worldPosition);
        if (Tilemap.GetTile(cellPosition) != grassTile)
        {

            Destroy(gameObject);
        }
    }

}

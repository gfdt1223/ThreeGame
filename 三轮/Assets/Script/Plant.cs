using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    // Start is called before the first frame update
    public float GrowCold;//生长冷却
    public float GrowColdTimer;//生长冷却计时器
    public float MaxAmount;//附近最多植物数
    public float CirleDistance;//附近距离
    public float PlantAmount;//附近植物数量
    public GameObject[] AllPlant;//所有植物   
    public float AllPlantAmount;//所有植物数量
    public float MaxEnergy;//最大能量
    public float CurrentEnergy;//现有能量
    void Start()
    {
        GrowColdTimer=GrowCold;
        CurrentEnergy=MaxEnergy;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentEnergy < MaxAmount)//获得能量
        {
            CurrentEnergy += Time.deltaTime * 0.1f;
        }
        if (CurrentEnergy <= 0)//死亡
        {
            Destroy(gameObject);
        }
        AllPlant=GameObject.FindGameObjectsWithTag(tag);
        GrowColdTimer-=Time.deltaTime;    
        FindPlant();
        if(GrowColdTimer < 0 &&PlantAmount<=MaxAmount&&AllPlantAmount<=100)//蔓延
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
        foreach(GameObject plant in AllPlant)
        {
            float distance=Vector3.Distance(transform.position, plant.transform.position);
            AllPlantAmount++;
            if (distance <= CirleDistance)
            {
                PlantAmount++;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Plant : MonoBehaviour
{

    // Start is called before the first frame update
    private Tilemap Tilemap;
    public float GrowCold;//生长冷却
    public float GrowColdTimer;//生长冷却计时器
    public float MaxAmount;//附近最多植物数
    public float CirleDistance;//附近距离
    public float PlantAmount;//附近植物数量
    public GameObject[] AllPlant;//所有植物   
    public float AllPlantAmount;//所有植物数量
    public float MaxEnergy;//最大能量
    public float CurrentEnergy;//现有能量
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
        if (CurrentEnergy < MaxAmount)//获得能量
        {
            CurrentEnergy += Time.deltaTime * 0.1f;
        }
        if (CurrentEnergy <= 0)//死亡
        {
            Destroy(gameObject);
        }       
        GrowColdTimer-=Time.deltaTime;    
        FindPlant();
        if(GrowColdTimer < 0 &&PlantAmount<=MaxAmount&&AllPlantAmount<=300)//蔓延
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
        // 获取预制体的世界坐标
        Vector3 worldPosition = gameObject.transform.position;

        // 将世界坐标转换为瓦片地图的单元格坐标
        Vector3Int cellPosition = Tilemap.WorldToCell(worldPosition);
        if (Tilemap.GetTile(cellPosition) != grassTile)
        {

            Destroy(gameObject);
        }
    }

}

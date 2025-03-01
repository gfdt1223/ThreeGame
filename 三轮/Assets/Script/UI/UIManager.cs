using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UIManager : MonoBehaviour
{
    public Tilemap groundTileMap;
    public GameObject shopPaneel;
    public GameObject genePanel;
    public GameObject creaturePanel;
    public GameObject sheep;

    private void Start()
    {
        shopPaneel.SetActive(true);
        genePanel.SetActive(false);
        creaturePanel.SetActive(false);
    }
    public void ChangeToShop()
    {
        shopPaneel.SetActive(true);
        genePanel.SetActive(false);
        creaturePanel.SetActive(false);
    }
    public void ChangeToGene()
    {
        shopPaneel.SetActive(false );
        genePanel.SetActive(true );
        creaturePanel.SetActive(false);
    } 
    public void ChangeToCreature()
    {
        shopPaneel.SetActive(false );
        genePanel.SetActive(false);
        creaturePanel.SetActive(true );
    }

    public void BuySheep()
    {
        Tilemap groundTileMap = GameObject.Find("Tilemap").GetComponent<Tilemap>();
        GameManager.Instance.gold--;
        int x = Random.Range(0, 40);
        int y = Random.Range(0, 40);
        Vector3Int cell = new Vector3Int(x, y, 0);
        Instantiate(sheep, groundTileMap.GetCellCenterWorld(cell), Quaternion.identity);
    }
}

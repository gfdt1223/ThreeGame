using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 单例实例
    public static GameManager Instance { get; private set; }

    // 游戏数据
    public int creatureCount = 0;
    public int gold = 100; // 初始金币
    public int score = 0;

    private void Awake()
    {
        // 确保只有一个实例存在
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保持对象在场景切换时不销毁
        }
        else
        {
            Destroy(gameObject); // 如果已经存在实例，销毁新的实例
        }
    }

    // 增加生物数量
    public void AddCreature(int amount)
    {
        creatureCount += amount;
        Debug.Log("Creature Count: " + creatureCount);
    }

    // 增加金币
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Gold: " + gold);
    }

    // 增加得分
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    // 减少金币
    public void SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            Debug.Log("Gold Spent. Remaining Gold: " + gold);
        }
        else
        {
            Debug.Log("Not enough gold!");
        }
    }
}
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // ����ʵ��
    public static GameManager Instance { get; private set; }

    // ��Ϸ����
    public int creatureCount = 0;
    public int gold = 100; // ��ʼ���
    public int score = 0;

    private void Awake()
    {
        // ȷ��ֻ��һ��ʵ������
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���ֶ����ڳ����л�ʱ������
        }
        else
        {
            Destroy(gameObject); // ����Ѿ�����ʵ���������µ�ʵ��
        }
    }

    // ������������
    public void AddCreature(int amount)
    {
        creatureCount += amount;
        Debug.Log("Creature Count: " + creatureCount);
    }

    // ���ӽ��
    public void AddGold(int amount)
    {
        gold += amount;
        Debug.Log("Gold: " + gold);
    }

    // ���ӵ÷�
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score: " + score);
    }

    // ���ٽ��
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
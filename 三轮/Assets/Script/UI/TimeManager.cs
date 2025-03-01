using UnityEngine;
using UnityEngine.UI;

public class GameTimeManagerUI : MonoBehaviour
{
    // ����ʵ��
    public static GameTimeManagerUI Instance { get; private set; }

    // UIԪ��
    public Text gameTimeText; // ��ʾ������Ϸʱ���Text���
    public Button pauseButton; // ��ͣ��ť
    public Button speedUpButton; // ���ٰ�ť
    public Button normalSpeedButton; // �����ٶȰ�ť

    // ʱ������
    private float normalTimeScale = 1f; // �����ٶ�
    private float fastTimeScale = 2f; // �����ٶ�

    // ������Ϸʱ��
    private float gameTime = 0f; // ��Ϸ����ʱ�䣨�룩
    private bool isPaused = false; // �Ƿ���ͣ

    private void Awake()
    {
        // ����ģʽ��ʼ��
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

    private void Start()
    {
        // ��ʼ����ť�¼�
        pauseButton.onClick.AddListener(TogglePause);
        speedUpButton.onClick.AddListener(SpeedUpGame);
        normalSpeedButton.onClick.AddListener(SetNormalSpeed);

        // ��ʼ��ʱ����ʾ
        UpdateGameTimeDisplay();
    }

    private void Update()
    {
        // �����Ϸδ��ͣ�����±�����Ϸʱ��
        if (!isPaused)
        {
            gameTime += Time.deltaTime;
            UpdateGameTimeDisplay();
        }
    }

    // ���±�����Ϸʱ����ʾ
    private void UpdateGameTimeDisplay()
    {
        // ������ת��Ϊʱ�����ʽ
        int hours = (int)(gameTime / 3600);
        int minutes = (int)((gameTime % 3600) / 60);
        int seconds = (int)(gameTime % 60);

        // ����UI��ʾ
        gameTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    // �л���ͣ״̬
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : normalTimeScale; // ��ͣʱʱ������Ϊ0������ָ������ٶ�
        Debug.Log(isPaused ? "Game Paused" : "Game Resumed");
    }

    // ������Ϸ
    public void SpeedUpGame()
    {
        Time.timeScale = fastTimeScale;
        isPaused = false; // ȷ����Ϸδ��ͣ
        Debug.Log("Game Speed: Fast (" + fastTimeScale + "x)");
    }

    // �ָ������ٶ�
    public void SetNormalSpeed()
    {
        Time.timeScale = normalTimeScale;
        isPaused = false; // ȷ����Ϸδ��ͣ
        Debug.Log("Game Speed: Normal (" + normalTimeScale + "x)");
    }

    // ��ȡ������Ϸʱ��
    public float GetGameTime()
    {
        return gameTime;
    }

    // ���ñ�����Ϸʱ��
    public void ResetGameTime()
    {
        gameTime = 0f;
        UpdateGameTimeDisplay();
    }
}
using UnityEngine;
using UnityEngine.UI;

public class GameTimeManagerUI : MonoBehaviour
{
    // 单例实例
    public static GameTimeManagerUI Instance { get; private set; }

    // UI元素
    public Text gameTimeText; // 显示本局游戏时间的Text组件
    public Button pauseButton; // 暂停按钮
    public Button speedUpButton; // 加速按钮
    public Button normalSpeedButton; // 正常速度按钮

    // 时间缩放
    private float normalTimeScale = 1f; // 正常速度
    private float fastTimeScale = 2f; // 加速速度

    // 本局游戏时间
    private float gameTime = 0f; // 游戏进行时间（秒）
    private bool isPaused = false; // 是否暂停

    private void Awake()
    {
        // 单例模式初始化
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

    private void Start()
    {
        // 初始化按钮事件
        pauseButton.onClick.AddListener(TogglePause);
        speedUpButton.onClick.AddListener(SpeedUpGame);
        normalSpeedButton.onClick.AddListener(SetNormalSpeed);

        // 初始化时间显示
        UpdateGameTimeDisplay();
    }

    private void Update()
    {
        // 如果游戏未暂停，更新本局游戏时间
        if (!isPaused)
        {
            gameTime += Time.deltaTime;
            UpdateGameTimeDisplay();
        }
    }

    // 更新本局游戏时间显示
    private void UpdateGameTimeDisplay()
    {
        // 将秒数转换为时分秒格式
        int hours = (int)(gameTime / 3600);
        int minutes = (int)((gameTime % 3600) / 60);
        int seconds = (int)(gameTime % 60);

        // 更新UI显示
        gameTimeText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
    }

    // 切换暂停状态
    public void TogglePause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : normalTimeScale; // 暂停时时间缩放为0，否则恢复正常速度
        Debug.Log(isPaused ? "Game Paused" : "Game Resumed");
    }

    // 加速游戏
    public void SpeedUpGame()
    {
        Time.timeScale = fastTimeScale;
        isPaused = false; // 确保游戏未暂停
        Debug.Log("Game Speed: Fast (" + fastTimeScale + "x)");
    }

    // 恢复正常速度
    public void SetNormalSpeed()
    {
        Time.timeScale = normalTimeScale;
        isPaused = false; // 确保游戏未暂停
        Debug.Log("Game Speed: Normal (" + normalTimeScale + "x)");
    }

    // 获取本局游戏时间
    public float GetGameTime()
    {
        return gameTime;
    }

    // 重置本局游戏时间
    public void ResetGameTime()
    {
        gameTime = 0f;
        UpdateGameTimeDisplay();
    }
}
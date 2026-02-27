using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    [Header("Win/Lose")]
    [SerializeField] private Health castleHealth;
    [SerializeField] private int winWave = 5; // buradan deðiþtir

    public int WinWave => winWave;
    public int CurrentWave { get; private set; }
    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
    }

    private void Start()
    {
        if (castleHealth != null)
            castleHealth.onDied.AddListener(Lose);
    }

    public void SetWave(int wave)
    {
        CurrentWave = wave;
    }

    public void Win()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        Debug.Log($"YOU WIN! Cleared wave {CurrentWave}.");
        Time.timeScale = 0f;
    }

    public void Lose()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        Debug.Log("YOU LOSE! Castle destroyed.");
        Time.timeScale = 0f;
    }
}
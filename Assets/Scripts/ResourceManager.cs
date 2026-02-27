using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager I { get; private set; }

    [Header("Starting Values")]
    [SerializeField] private int startingGold = 200;

    [Header("UI")]
    [SerializeField] private TMP_Text goldText;

    public int Gold { get; private set; }

    private void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        // Ýstersen: DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Gold = startingGold;
        RefreshUI();
    }

    public bool CanAfford(int cost) => Gold >= cost;

    public bool TrySpend(int cost)
    {
        if (Gold < cost) return false;
        Gold -= cost;
        RefreshUI();
        return true;
    }

    public void AddGold(int amount)
    {
        Gold += Mathf.Max(0, amount);
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (goldText != null)
            goldText.text = $"Gold: {Gold}";
    }
}
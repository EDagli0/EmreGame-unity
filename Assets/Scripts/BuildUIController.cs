using UnityEngine;

public class BuildUIController : MonoBehaviour
{
    [SerializeField] private UnitFactory factory;

    [Header("Costs")]
    [SerializeField] private int swordsmanCost = 50;
    [SerializeField] private int archerCost = 75;

    public void OnClickSwordsman()
    {
        if (ResourceManager.I != null && ResourceManager.I.TrySpend(swordsmanCost))
            factory.SpawnSwordsman();
    }

    public void OnClickArcher()
    {
        if (ResourceManager.I != null && ResourceManager.I.TrySpend(archerCost))
            factory.SpawnArcher();
    }
}
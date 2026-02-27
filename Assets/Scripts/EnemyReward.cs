using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyReward : MonoBehaviour
{
    [SerializeField] private int goldReward = 10;

    private void Start()
    {
        var h = GetComponent<Health>();
        h.onDied.AddListener(GiveReward);
    }

    private void GiveReward()
    {
        if (ResourceManager.I != null)
            ResourceManager.I.AddGold(goldReward);
    }
}
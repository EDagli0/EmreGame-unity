using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject swordsmanPrefab;
    [SerializeField] private GameObject archerPrefab;

    public void SpawnSwordsman()
    {
        Instantiate(swordsmanPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void SpawnArcher()
    {
        Instantiate(archerPrefab, spawnPoint.position, Quaternion.identity);
    }
}
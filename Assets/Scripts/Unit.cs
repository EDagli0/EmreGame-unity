using UnityEngine;

public class Unit : MonoBehaviour
{
    public Team team = Team.Player;

    private void OnEnable() => UnitRegistry.Register(this);
    private void OnDisable() => UnitRegistry.Unregister(this);
}
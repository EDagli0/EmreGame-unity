using System.Collections.Generic;
using UnityEngine;

public static class UnitRegistry
{
    private static readonly List<Unit> units = new();

    public static void Register(Unit u)
    {
        if (u != null && !units.Contains(u)) units.Add(u);
    }

    public static void Unregister(Unit u)
    {
        if (u != null) units.Remove(u);
    }

    public static bool TryGetNearest(Unit seeker, Team targetTeam, float radius, out Unit nearest)
    {
        nearest = null;
        if (seeker == null) return false;

        float best = float.MaxValue;
        Vector3 pos = seeker.transform.position;
        float r2 = radius * radius;

        for (int i = units.Count - 1; i >= 0; i--)
        {
            Unit u = units[i];
            if (u == null) { units.RemoveAt(i); continue; }

            if (u == seeker) continue;
            if (u.team != targetTeam) continue;

            var h = u.GetComponent<Health>();
            if (h != null && h.CurrentHP <= 0) continue;

            float d2 = (u.transform.position - pos).sqrMagnitude;
            if (d2 > r2) continue;

            if (d2 < best)
            {
                best = d2;
                nearest = u;
            }
        }

        return nearest != null;
    }
}
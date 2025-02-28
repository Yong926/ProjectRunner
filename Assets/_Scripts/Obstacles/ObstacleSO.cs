using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "Date/Obstacle")]
public class ObstacleSO : ScriptableObject
{
    public List<ObstaclePool> pools;
    [AsRange(0, 100)] public Vector2 interval;
}
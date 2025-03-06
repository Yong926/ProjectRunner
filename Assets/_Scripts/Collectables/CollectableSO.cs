using System.Collections.Generic;
using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "Date/Collectable")]
public class CollectableSO : ScriptableObject
{
    public List<CollectablePool> collectablePools;
    public List<LanePatternPool> lanepatternPools;

    [AsRange(0, 100)] public Vector2 interval;
    [AsRange(1, 30)] public Vector2 quota;
}
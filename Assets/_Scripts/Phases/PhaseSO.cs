using UnityEngine;
using CustomInspector;

[CreateAssetMenu(menuName = "Date/Phase")]
public class PhaseSO : ScriptableObject
{
    public string Name;
    [Preview(Size.small)] public Sprite Icon;
    public uint Mileage;

    public float scrollSpeed;



    [Foldout] public ObstacleSO obstacleData;
}
using UnityEngine;
using System.Collections.Generic;

public class Obstacle : MonoBehaviour
{
    [SerializeField] List<GameObject> meshPrefabs;
    [SerializeField] Transform meshRoot;

    void Start()
    {
        int rnd = Random.Range(0, meshPrefabs.Count);
        Instantiate(meshPrefabs[rnd], meshRoot);
    }
}
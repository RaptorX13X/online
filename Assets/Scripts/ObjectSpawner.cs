using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class ObjectSpawner : NetworkBehaviour
{
    [Networked] private TickTimer spawnTimer { get; set; }
    [SerializeField] private float planeMaxX;
    [SerializeField] private float planeMinX;
    [SerializeField] private float planeY;
    [SerializeField] private float planeMaxZ;
    [SerializeField] private float planeMinZ;
    [SerializeField] private GameObject smallPrefab;
    [SerializeField] private GameObject bigPrefab;
    [SerializeField] private int smallPrefabWeight;
    [SerializeField] private int bigPrefabWeight;
    private int randomWeight;
    private int chosenIndex;
    private GameObject chosenObject;

    private void Start() // powinno iść na on player joined czy cos
    {
        Spawn();
    }

    public override void FixedUpdateNetwork()
    {
        if (spawnTimer.Expired(Runner))
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        RandomizeObject();
        Instantiate(chosenObject, new Vector3(Random.Range(planeMinX, planeMaxX), planeY, Random.Range(planeMinZ, planeMaxZ)), quaternion.identity);
        spawnTimer = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }

    private void RandomizeObject()
    {
        randomWeight = smallPrefabWeight + bigPrefabWeight + 1;
        chosenIndex = Random.Range(0, randomWeight);
        if (chosenIndex <= smallPrefabWeight)
        {
            chosenObject = smallPrefab;
        }
        else if (chosenIndex > smallPrefabWeight && chosenIndex <= smallPrefabWeight + bigPrefabWeight)
        {
            chosenObject = bigPrefab;
        }
    }
}

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
    [SerializeField] private GameObject card1Prefab;
    [SerializeField] private GameObject card2Prefab;
    [SerializeField] private int smallPrefabWeight;
    [SerializeField] private int bigPrefabWeight;
    [SerializeField] private int card1PrefabWeight;
    [SerializeField] private int card2PrefabWeight;
    private int randomWeight;
    private int chosenIndex;
    private GameObject chosenObject;

    private void Start() // powinno iść na on player joined czy cos
    {
        spawnTimer = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }

    public override void FixedUpdateNetwork()
    {
        if (spawnTimer.Expired(Runner))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        RandomizeObject();
        Runner.Spawn(chosenObject,
            new Vector3(Random.Range(planeMinX, planeMaxX), planeY, Random.Range(planeMinZ, planeMaxZ)),
            quaternion.identity);
        spawnTimer = TickTimer.CreateFromSeconds(Runner, 5.0f);
    }

    private void RandomizeObject()
    {
        randomWeight = smallPrefabWeight + bigPrefabWeight + card1PrefabWeight + card2PrefabWeight + 1;
        chosenIndex = Random.Range(0, randomWeight);
        int smaller1 = smallPrefabWeight;
        int bigger2 = smallPrefabWeight;
        int smaller2 = smallPrefabWeight + bigPrefabWeight;
        int bigger3 = smallPrefabWeight + bigPrefabWeight;
        int smaller3 = smallPrefabWeight + bigPrefabWeight + card1PrefabWeight;
        int bigger4 = smallPrefabWeight + bigPrefabWeight + card1PrefabWeight;
        int smaller4 = smallPrefabWeight + bigPrefabWeight + card1PrefabWeight + card2PrefabWeight;
        
        if (chosenIndex <= smaller1)
        {
            chosenObject = smallPrefab;
        }
        else if (chosenIndex > bigger2 && chosenIndex <= smaller2)
        {
            chosenObject = bigPrefab;
        }
        else if (chosenIndex > bigger3 && chosenIndex <= smaller3)
        {
            chosenObject = card1Prefab;
        }
        else if (chosenIndex > bigger4 && chosenIndex <= smaller4)
        {
            chosenObject = card2Prefab;
        }
    }
}

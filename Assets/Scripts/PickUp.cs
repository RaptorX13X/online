using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PickUp : NetworkBehaviour
{
    [SerializeField] private int scoreValue;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().AddScore(scoreValue);
            Runner.Despawn(Object); // ?
        }
    }
}

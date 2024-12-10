using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Karta : NetworkBehaviour
{
    [SerializeField] private int cardID;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().AddCard(cardID);
            Runner.Despawn(Object); 
        }
    }
}

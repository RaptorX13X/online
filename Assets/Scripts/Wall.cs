using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Wall : NetworkBehaviour
{
    public Vector3 teleportLocation;
    public bool vertical;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.TryGetComponent(out NetworkCharacterController cc);
            if (vertical)
            {
                teleportLocation.x = other.transform.position.x;
                teleportLocation.y = other.transform.position.y;
                cc.Teleport(teleportLocation);
            }
            else
            {
                teleportLocation.y = other.transform.position.y;
                teleportLocation.z = other.transform.position.z;
                cc.Teleport(teleportLocation);
            }
        }
    }
}

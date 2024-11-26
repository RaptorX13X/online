using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController _cc;
    private float velocity;
    private float angle;
    [SerializeField] private float maxVelocity;
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {   
        if (GetInput(out NetworkInputData data))
        {
            velocity += data.velocity;
            angle += data.angle;
            velocity = Mathf.Clamp(velocity, 0, maxVelocity);
            
            _cc.gameObject.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + angle, transform.rotation.z);
        }
        Debug.Log(velocity);
        if (velocity > 0f)
            _cc.Move(transform.forward * velocity * Runner.DeltaTime);
    }
}

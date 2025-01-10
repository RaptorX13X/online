using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    public const byte EButton = 1;
    public NetworkButtons buttons;
    public float angle;
    public float velocity;
    public const byte PButton = 2;
}

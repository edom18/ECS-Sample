using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public struct Velocity : IComponentData
{
    public float3 Value;

    public Velocity(float3 value)
    {
        Value = value;
    }
}

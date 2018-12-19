using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Collections;

public struct VelocityGroup
{
    public ComponentDataArray<Position> Position;
    public ComponentDataArray<Velocity> Velocity;

    [ReadOnly]
    public SharedComponentDataArray<MeshInstanceRenderer> Renderer;

    public readonly int Length;
}

public class VelocitySystem : ComponentSystem
{
    [Inject]
    private VelocityGroup _velocityGroup;

    private float deltaTime;

    protected override void OnUpdate()
    {
        deltaTime = Time.deltaTime;

        for (int i = 0; i < _velocityGroup.Length; i++)
        {
            Position pos = _velocityGroup.Position[i];
            pos.Value += _velocityGroup.Velocity[i].Value * deltaTime;
            _velocityGroup.Position[i] = pos;
        }
    }
}

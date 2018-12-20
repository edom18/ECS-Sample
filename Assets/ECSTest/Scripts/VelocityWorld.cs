using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

public class VelocityWorld : MonoBehaviour
{
    [SerializeField]
    private Mesh _mesh;

    [SerializeField]
    private Material _material;

    [SerializeField]
    private bool _useJobSystem = false;

    private void Start()
    {
        World.DisposeAllWorlds();

        World.Active = new World("VelocityWorld");

        EntityManager manager = World.Active.CreateManager<EntityManager>();
        World.Active.CreateManager<EndFrameTransformSystem>();
        World.Active.CreateManager<RenderingSystemBootstrap>();

        if (_useJobSystem)
        {
            World.Active.CreateManager<VelocityJobSystem>();
        }
        else
        {
            World.Active.CreateManager<VelocitySystem>();
        }

        EntityArchetype archetype = manager.CreateArchetype(
            typeof(Velocity),
            typeof(Position),
            typeof(MeshInstanceRenderer)
        );
        Entity entity = manager.CreateEntity(archetype);

        manager.SetSharedComponentData(entity, new MeshInstanceRenderer
        {
            mesh = _mesh,
            material = _material,
        });

        manager.SetComponentData(entity, new Velocity { Value = new float3(0, 1f, 0) });
        manager.SetComponentData(entity, new Position { Value = new float3(0, 0, 0) });

        manager.Instantiate(entity);

        ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World.Active);
    }
}

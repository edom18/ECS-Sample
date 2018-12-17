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

    private void Start()
    {
        World.DisposeAllWorlds();

        World.Active = new World("VelocityWorld");

        EntityManager manager = World.Active.CreateManager<EntityManager>();
        World.Active.CreateManager<EndFrameTransformSystem>();
        World.Active.CreateManager<RenderingSystemBootstrap>();

        EntityArchetype archetype = manager.CreateArchetype(typeof(Position), typeof(Rotation), typeof(MeshInstanceRenderer));
        Entity entity = manager.CreateEntity(archetype);

        manager.SetSharedComponentData(entity, new MeshInstanceRenderer
        {
            mesh = _mesh,
            material = _material,
        });

        manager.SetComponentData(entity, new Position { Value = new float3(0, 0, 0) });
        manager.SetComponentData(entity, new Rotation { Value = Quaternion.Euler(0, 0, 0) });

        manager.Instantiate(entity);

        ScriptBehaviourUpdateOrder.UpdatePlayerLoop(World.Active);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

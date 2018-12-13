using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;

public class ECSTest : MonoBehaviour
{
    [SerializeField]
    private Mesh _mesh;

    [SerializeField]
    private Material _material;

    private void Start()
    {
        World world = World.Active;// = new World("hoge");
        EntityManager entityManager = world.GetOrCreateManager<EntityManager>();

        // Prefabを作成
        Entity prefab = entityManager.CreateEntity(
            ComponentType.Create<Position>(), // 位置
            ComponentType.Create<Prefab>() // Prefab（これがついているEntityはSystemから無視される）
        );

        // 描画用のComponentを追加
        entityManager.AddSharedComponentData(prefab, new MeshInstanceRenderer
        {
            castShadows = UnityEngine.Rendering.ShadowCastingMode.On,
            receiveShadows = true,
            material = _material,
            mesh = _mesh
        });

        // Prefabをインスタンス化
        entityManager.Instantiate(prefab);
    }
}

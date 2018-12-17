using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Mathematics;

public class ECSWorldTest : MonoBehaviour
{
    [SerializeField]
    private Mesh _mesh;

    [SerializeField]
    private Material _material;

    private World _gameWorld;

    private void Start()
    {
        // ひとまず、デフォルトのワールドを削除する
        // ※ Player SettingsのDefine Symbolsに「UNITY_DISABLE_AUTOMATIC_SYSTEM_BOOTSTRAP」を指定してもデフォルトワールドの生成を抑えられる
        World.DisposeAllWorlds();

        _gameWorld = new World("GameWorld");
        World.Active = _gameWorld;

        // デフォルトで用意されているTransformSystem
        _gameWorld.CreateManager<EndFrameTransformSystem>();
        _gameWorld.CreateManager<EndFrameBarrier>();

        // デフォルトで用意されている描画を行うためのシステム「MeshInstanceRendererSystem」の補助クラス
        _gameWorld.CreateManager<RenderingSystemBootstrap>();

        // EndFrameTransformSystemなどを先に生成すると、自動的にEntityManagerが生成されるので、GetOrCreateで取得する
        EntityManager entityManager = _gameWorld.GetOrCreateManager<EntityManager>();

        EntityArchetype archetype = entityManager.CreateArchetype(
            // ComponentType.Create<LocalToWorld>(), // Positionなどがあるとデフォルトシステムなら自動で追加してくれる
            ComponentType.Create<Position>(),
            ComponentType.Create<Rotation>(),
            ComponentType.Create<MeshInstanceRenderer>()
        );

        // アーキタイプを元にエンティティを生成する
        Entity entity = entityManager.CreateEntity(archetype);

        // Rendererを設定
        entityManager.SetSharedComponentData(entity, new MeshInstanceRenderer
        {
            mesh = _mesh,
            material = _material
        });

        // Transform関連の情報を設定
        entityManager.SetComponentData(entity, new Position
        {
            Value = new float3(0, 0, 0)
        });

        entityManager.SetComponentData(entity, new Rotation
        {
            Value = Quaternion.Euler(0f, 180f, 0f)
        });

        // インスタンス化
        entityManager.Instantiate(entity);

        // イベントループにワールドを登録する
        ScriptBehaviourUpdateOrder.UpdatePlayerLoop(_gameWorld);
    }

    private void OnDestroy()
    {
        _gameWorld.Dispose();
    }
}

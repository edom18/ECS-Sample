using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Transforms;

public class VelocitySystem : JobComponentSystem
{
    [BurstCompile]
    struct Job : IJobProcessComponentData<Velocity, Position, Rotation>
    {
        readonly float _deltaTime;

        public Job(float deltaTime)
        {
            _deltaTime = deltaTime;
        }

        public void Execute(ref Velocity velocity, ref Position position, ref Rotation rotation)
        {
            position.Value += velocity.Value * _deltaTime;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        Job job = new Job(Time.deltaTime);
        return job.Schedule(this, inputDeps);
    }
}

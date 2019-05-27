using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


[BurstCompile]
public struct MovementJob : IJobForEach<Translation,Rotation,MovementComponent,BulletTag>
{
    public float moveSpeed;
    public float deltaTime;

    public void Execute(ref Translation translation, [ReadOnly] ref Rotation rotation, [ReadOnly] ref MovementComponent movementComponent, [ReadOnly] ref BulletTag bulletTag)
    {
        if (bulletTag.isShot)
        {
            translation.Value += math.mul(rotation.Value, new float3(0, 1, 0)) * movementComponent.moveSpeed * deltaTime;
        }
    }
}

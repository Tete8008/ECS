using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public class MovementSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {



        MovementJob movementJob = new MovementJob
        {
            deltaTime = Time.deltaTime,
            moveSpeed = GameManager.instance.projectileSpeed
        };

        JobHandle moveHandle = movementJob.Schedule(this,inputDeps);

        return moveHandle;


        /*Entities.ForEach((ref MovementComponent movementComponent, ref Translation translation, ref Rotation rotation) =>
        {
            if (movementComponent.isShot)
            {
                translation.Value += math.mul(rotation.Value, new float3(0, 1, 0)) * movementComponent.moveSpeed * Time.deltaTime;
            }
        });
        Debug.Log(Entities.ToEntityQuery().CalculateLength());*/
    }



}

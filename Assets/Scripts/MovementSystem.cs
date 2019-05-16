using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref MovementComponent movementComponent,ref Translation translation, ref Rotation rotation) =>
        {
            if (movementComponent.isShot)
            {
                translation.Value += math.mul(rotation.Value, new float3(0, 1, 0)) * movementComponent.moveSpeed * Time.deltaTime;
            }
        });
        Debug.Log(Entities.ToEntityQuery().CalculateLength());
    }
}

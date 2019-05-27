using Unity.Entities;
using UnityEngine;


public struct LifeTimeJob : IJobForEachWithEntity<BulletTag,LifeTimeComponent>
{
    public EntityCommandBuffer.Concurrent entityCommandBuffer;
    public float deltaTime;


    public void Execute(Entity entity, int index, ref BulletTag bulletTag, ref LifeTimeComponent lifeTimeComponent)
    {
        if (bulletTag.isShot)
        {
            lifeTimeComponent.Value -= deltaTime;
            if (lifeTimeComponent.Value <= 0)
            {

                entityCommandBuffer.DestroyEntity(index, entity);

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

public class LifeTimeSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity,ref LifeTimeComponent lifeTimeComponent,ref MovementComponent movementComponent) =>
        {
            if (movementComponent.isShot)
            {
                lifeTimeComponent.Value -= Time.deltaTime;
                if (lifeTimeComponent.Value <= 0)
                {
                    GameManager.instance.RemoveProjectile(entity, PostUpdateCommands);
                }
            }
            
        });
        
    }

}

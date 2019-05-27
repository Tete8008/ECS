using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Jobs;

public class GameManager : Singleton<GameManager>
{
    public float projectileSpeed;
    public GameObject projectilePrefab;
    public float shootingAngle;

    public float playerSpeed=1f;

    public int projectileCount = 2;

    public float projectileLifeTime=5f;

    public EntityManager entityManager;

    public Mesh projectileMesh;
    public Material projectileMaterial;

    TransformAccessArray transforms;
    MovementJob movementJob;
    JobHandle movementHandle;

    EntityArchetype projectileArchetype;




    private void OnDisable()
    {
        movementHandle.Complete();
        transforms.Dispose();
    }



    private void Start()
    {
        transforms = new TransformAccessArray(0);
        entityManager = World.Active.EntityManager;

        projectileArchetype = entityManager.CreateArchetype(
            typeof(MovementComponent),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(Translation),
            typeof(Rotation),
            typeof(LifeTimeComponent),
            typeof(BulletTag)
        );
    }


    public void SpawnProjectiles(int projectileCount,Vector3 playerRotation,Vector3 projectileOriginPosition)
    {

        movementHandle.Complete();

        transforms.capacity = transforms.length + projectileCount;
        float pc = projectileCount - 1;

        NativeArray<Entity> projectileEntities = new NativeArray<Entity>(projectileCount, Allocator.Temp);
        entityManager.CreateEntity(projectileArchetype, projectileEntities);

        for (int i = 0; i < projectileCount; i++)
        {
           
            float angle = Mathf.Lerp(-shootingAngle / 2, shootingAngle / 2, i / pc)/360*math.PI*2;

            //transforms.Add(Instantiate(projectilePrefab, projectileOriginPosition, Quaternion.Euler(playerRotation.x, playerRotation.y + angle, playerRotation.z)).transform);


            Entity entity =entityManager.CreateEntity(projectileArchetype);
            //on ajoute un renderMesh
            entityManager.AddComponent(entity, typeof(RenderMesh));
            entityManager.SetSharedComponentData(entity, new RenderMesh
            {
                mesh = projectileMesh,
                material = projectileMaterial
            });
            //on set sa position
            entityManager.SetComponentData(entity, new Translation
            {
                Value = new float3(projectileOriginPosition)
            });
            //et sa rotation
            entityManager.SetComponentData(entity, new Rotation
            {
                Value = quaternion.Euler(playerRotation.x/360*math.PI*2+math.PI/2, playerRotation.y/360*math.PI*2 + angle, playerRotation.z/360*math.PI*2)
            });

            entityManager.SetComponentData(entity, new MovementComponent
            {
                moveSpeed = projectileSpeed
            });

            entityManager.SetComponentData(entity, new BulletTag
            {
                isShot = true
            });


            entityManager.SetComponentData(entity, new LifeTimeComponent
            {
                Value=projectileLifeTime
            });
        }
        projectileEntities.Dispose();
    }


}

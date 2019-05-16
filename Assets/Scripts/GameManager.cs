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

    public int projectileCount = 5;

    public float projectileLifeTime=5f;

    public EntityManager entityManager;

    public Mesh projectileMesh;
    public Material projectileMaterial;

    TransformAccessArray transforms;
    MovementJob movementJob;
    JobHandle movementHandle;

    public NativeArray<Entity> projectileEntities;

    public List<Entity> projectilePool;
    public List<Entity> activeProjectiles;

    private void OnDisable()
    {
        movementHandle.Complete();
        transforms.Dispose();
    }



    private void Start()
    {
        transforms = new TransformAccessArray(0);
        entityManager = World.Active.EntityManager;

        EntityArchetype projectileArchetype = entityManager.CreateArchetype(
            typeof(MovementComponent),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(Translation),
            typeof(Rotation),
            typeof(LifeTimeComponent)
        );


        projectileEntities = new NativeArray<Entity>(5000, Allocator.Temp);
        entityManager.CreateEntity(projectileArchetype, projectileEntities);

        projectilePool = new List<Entity>();
        activeProjectiles = new List<Entity>();

        for (int i = 0; i < projectileEntities.Length; i++)
        {
            Entity entity = projectileEntities[i];
            /*entityManager.SetSharedComponentData(entity,  new RenderMesh{
                mesh=projectileMesh,
                material=projectileMaterial
            });
            Color color = entityManager.GetSharedComponentData<RenderMesh>(entity).material.color;
            entityManager.GetSharedComponentData<RenderMesh>(entity).material.color = new Color(color.r, color.g, color.b, 0);
            */
            entityManager.SetComponentData(entity, new MovementComponent
            {
                moveSpeed=projectileSpeed,
                isShot=false
            });

            entityManager.SetComponentData(entity, new Rotation
            {
                Value=quaternion.Euler(math.PI/2,0,0)
            });


            entityManager.SetComponentData(entity, new Translation
            {
                Value = new float3(0, 0.05f, 0)
            });

            entityManager.SetComponentData(entity, new LifeTimeComponent
            {
                Value=projectileLifeTime
            });


            entityManager.RemoveComponent<RenderMesh>(entity);
            projectilePool.Add(entity);
        }
        projectileEntities.Dispose();
    }

    /*private void Update()
    {
        movementHandle.Complete();
        movementJob = new MovementJob()
        {
            moveSpeed = projectileSpeed,
            deltaTime = Time.deltaTime
        };

        movementHandle = movementJob.Schedule(transforms);

        JobHandle.ScheduleBatchedJobs();
    }*/

    public void SpawnProjectiles(int projectileCount,Vector3 playerRotation,Vector3 projectileOriginPosition)
    {

        movementHandle.Complete();
        transforms.capacity = transforms.length + projectileCount;
        float pc = projectileCount - 1;

        if (projectileCount > projectilePool.Count)
        {
            Debug.Log("Plus rien dans la pool");
            return;
        }

        for (int i = 0; i < projectileCount; i++)
        {
           
            float angle = Mathf.Lerp(-shootingAngle / 2, shootingAngle / 2, i / pc)/360*math.PI*2;

            //transforms.Add(Instantiate(projectilePrefab, projectileOriginPosition, Quaternion.Euler(playerRotation.x, playerRotation.y + angle, playerRotation.z)).transform);


            Entity entity = projectilePool[i];
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
                moveSpeed = projectileSpeed,
                isShot = true
            });
            activeProjectiles.Add(entity);
        }

        projectilePool.RemoveRange(0, projectileCount);
        
    }


    public void RemoveProjectile(Entity entity,EntityCommandBuffer entityCommandBuffer)
    {
        //entityManager.RemoveComponent<RenderMesh>(entity);


        entityCommandBuffer.RemoveComponent<RenderMesh>(entity);
        entityManager.SetComponentData(entity, new MovementComponent
        {
            isShot = false
        });
        projectilePool.Add(entity);
        activeProjectiles.Remove(entity);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
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

    TransformAccessArray transforms;
    MovementJob movementJob;
    JobHandle movementHandle;

    private void OnDisable()
    {
        movementHandle.Complete();
        transforms.Dispose();
    }


    private void Start()
    {
        transforms = new TransformAccessArray(0);
    }

    private void Update()
    {
        movementHandle.Complete();
        movementJob = new MovementJob()
        {
            moveSpeed = projectileSpeed,
            deltaTime = Time.deltaTime
        };

        movementHandle = movementJob.Schedule(transforms);

        JobHandle.ScheduleBatchedJobs();
    }

    public void SpawnProjectiles(int projectileCount,Vector3 playerRotation,Vector3 projectileOriginPosition)
    {
        movementHandle.Complete();
        transforms.capacity = transforms.length + projectileCount;
        float pc = projectileCount - 1;
        for (int i = 0; i < projectileCount; i++)
        {
            float angle = Mathf.Lerp(-shootingAngle / 2, shootingAngle / 2, i / pc);

            transforms.Add(Instantiate(projectilePrefab, projectileOriginPosition, Quaternion.Euler(playerRotation.x, playerRotation.y + angle, playerRotation.z)).transform);
        }
    }
}

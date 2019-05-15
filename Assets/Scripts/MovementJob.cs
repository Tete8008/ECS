using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;



public struct MovementJob : IJobParallelForTransform
{
    public float moveSpeed;
    public float deltaTime;
    public void Execute(int index, TransformAccess transform)
    {
        transform.position += transform.rotation*new Vector3(0,0,1f) * GameManager.instance.projectileSpeed * deltaTime;
    }

}

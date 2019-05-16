using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public struct MovementComponent : IComponentData
{
    public float moveSpeed;
    public bool isShot;
}
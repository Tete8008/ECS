using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    #region Players array
    public static List<PlayerBehaviour> players;

    private void Awake()
    {
        if (players == null)
        {
            players = new List<PlayerBehaviour>();
        }
        players.Add(this);
    }

    private void OnDestroy()
    {
        players.Remove(this);
    }

    #endregion

    public Transform self;
    public Transform projectilesOrigin;



    public void Move(float2 velocity)
    {
        self.position += new Vector3(velocity.x,0,velocity.y);

    }


    public void Shoot(int projectileCount,float2 mousePosition)
    {

        Vector3 playerRotation = Quaternion.LookRotation(new Vector3(mousePosition.x-Screen.width/2,0,mousePosition.y-Screen.height/2)).eulerAngles;
        
        GameManager.instance.SpawnProjectiles(projectileCount, playerRotation,projectilesOrigin.position);
        
        
    }

}

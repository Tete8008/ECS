using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public KeyCode left;
    public KeyCode right;
    public KeyCode up;
    public KeyCode down;

    private void Update()
    {
        float2 direction=new float2();

        if (Input.GetKey(left))
        {
            direction.x -= 1;
        }

        if (Input.GetKey(right))
        {
            direction.x += 1;
        }

        if (Input.GetKey(up))
        {
            direction.y += 1;
        }

        if (Input.GetKey(down))
        {
            direction.y-= 1;
        }
        if (direction.x != 0 || direction.y!=0)
        {
            PlayerBehaviour.players[0].Move(direction * GameManager.instance.playerSpeed * Time.deltaTime);

        }

        if (Input.GetMouseButton(0))
        {
            PlayerBehaviour.players[0].Shoot(GameManager.instance.projectileCount,new float2(Input.mousePosition.x,Input.mousePosition.y));
        }

    }




}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBehaviour : Singleton<CameraFollowBehaviour>
{
    public Transform self;

    private void Update()
    {
        Transform player = PlayerBehaviour.players[0].self;
        if (!(Mathf.Abs(self.position.x-player.position.x)<=0.075f && Mathf.Abs(self.position.z - player.position.z) <= 0.075f))
        {
            Vector3 pos = Vector3.Lerp(self.position,player.position,Time.deltaTime*2);
            self.position = new Vector3(pos.x, self.position.y, pos.z);
        }
    }
}

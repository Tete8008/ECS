using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public Transform self;


    private void Update()
    {
        //self.position += self.forward * GameManager.instance.projectileSpeed * Time.deltaTime;
    }

    private void Start()
    {
        StartCoroutine(Die());
    }


    private IEnumerator Die()
    {
        yield return new WaitForSeconds(GameManager.instance.projectileLifeTime);
        Destroy(gameObject);
    }
}

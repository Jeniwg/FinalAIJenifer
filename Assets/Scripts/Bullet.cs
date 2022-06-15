using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5;

    //Add velocity to bullet
    private void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    //if enter in collision with something destroy
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDoor : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer cube;
    [SerializeField]
    private Collider cubeCollider;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 7.9f, gameObject.transform.position.z);
            cube.enabled = false;
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, 4.8f, gameObject.transform.position.z);
            cube.enabled = true;
            cubeCollider.enabled = false;
        }
    }

}

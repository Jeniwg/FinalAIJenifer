using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
 
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3 (player.transform.position.x, 20, player.transform.position.z);
    }
}

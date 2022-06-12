using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject bulletEmitter;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float bulletRate = 0.5f;
    [SerializeField]
    private int playerLife = 20;
    private float lastBullet;
    private Vector3 inputMovement;
    private bool canOpenDoor = true;
    [SerializeField]
    private BlockDoor blockDoor;

    private void Start()
    {
        lastBullet = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));

        inputMovement = new Vector3(Input.GetAxis("Vertical"), 0, Input.GetAxis("Horizontal"));


        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(inputMovement * Time.deltaTime * moveSpeed * 2, Space.World);
        }
        else
        {
            transform.Translate(inputMovement * Time.deltaTime * moveSpeed, Space.World);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if ((Time.time - lastBullet > bulletRate))
            {
                Shoot();
                lastBullet = Time.time;
            }

        }

        if(playerLife <= 0)
        {
            Debug.Log("U DEAD!");
        }

    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        canOpenDoor = blockDoor.CanOpenDoors;
        Debug.DrawRay(transform.position, transform.forward, Color.yellow);
        if (Physics.Raycast(ray, out hit, 2))
        {
            if (hit.transform.tag == "Door" && Input.GetKeyDown(KeyCode.E) && canOpenDoor == true)
            {
                Debug.Log("Open");
                Animator anim = hit.collider.gameObject.GetComponentInParent<Animator>();
                if (anim.GetBool("isDoorOpen") == false)
                {
                    anim.SetBool("isDoorOpen", true);
                }
                else
                {
                    anim.SetBool("isDoorOpen", false);
                }
            }

        }
    }

    private void Shoot()
    {
        GameObject obj = Instantiate(bullet, bulletEmitter.transform.position, transform.rotation);
        obj.tag = "PlayerBullet";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            playerLife -= 2;
        }
    }

}

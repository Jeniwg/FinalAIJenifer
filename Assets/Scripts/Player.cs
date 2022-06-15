using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private float speed = 2;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject bulletEmitter;
    [SerializeField]
    private float bulletRate = 0.5f;
    [SerializeField]
    private int playerLife = 20;
    private float lastBullet;
    private Vector3 inputMovement;
    private bool isGrounded;
    private float groundDistance = 0.05f;
    private bool canOpenDoor = true;
    private bool lookingAtDoor = false;
    private GameObject lastOpenedDoor;
    [SerializeField]
    private BlockDoor blockDoor;
    [SerializeField]
    private GameObject gameOver;
    [SerializeField]
    private GameObject win;

    private void Start()
    {
        lastBullet = Time.time;
    }

    void Update()
    {
        //Rotation of camera
        //Look where mouse is
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.LookAt(new Vector3(mousePos.x, transform.position.y, mousePos.z));

        //Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(z, 0, -x);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            controller.Move(move * speed * 2 * Time.deltaTime);
        }
        else
        {
            controller.Move(move * speed * Time.deltaTime);
        }

        //Shoot and cooldown
        if (Input.GetMouseButtonDown(0))
        {
            if ((Time.time - lastBullet > bulletRate))
            {
                Shoot();
                lastBullet = Time.time;
            }

        }

        //Check if grounded if not move in y until its grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (!isGrounded)
        {
            Vector3 y = transform.position;
            y.y = y.y - Time.deltaTime;
            transform.position = y;
        }

        //RayCast
        Ray();

        //If E and not looking door close last opened door
        if (Input.GetKeyDown(KeyCode.E) && lookingAtDoor == false && lastOpenedDoor != null)
        {
            Animator anim = lastOpenedDoor.GetComponentInParent<Animator>();
            anim.SetBool("isDoorOpen", false);
        }

        //GameOver
        if (playerLife <= 0)
        {
            gameOver.SetActive(true);
            controller.enabled = false;
        }

    }

    //Send Raycast. If player can OpenDoor open
    private void Ray()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit = new RaycastHit();
        canOpenDoor = blockDoor.CanOpenDoors;
        Debug.DrawRay(transform.position, transform.forward, Color.yellow);
        if (Physics.Raycast(ray, out hit, 3))
        {
            if (hit.transform.tag == "Door")
            {
                lookingAtDoor = true;
                if (Input.GetKeyDown(KeyCode.E) && canOpenDoor == true)
                {
                    Animator anim = hit.collider.gameObject.GetComponentInParent<Animator>();
                    if (anim.GetBool("isDoorOpen") == false)
                    {
                        anim.SetBool("isDoorOpen", true);
                        lastOpenedDoor = hit.collider.gameObject;
                    }
                }
            }
            else
            {
                lookingAtDoor = false;
            }

        }
    }

    //Instantiate bullet with PlayerBullet tag
    private void Shoot()
    {
        GameObject obj = Instantiate(bullet, bulletEmitter.transform.position, transform.rotation);
        obj.tag = "PlayerBullet";
    }

    //Get a hit
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Enemy"))
        {
            playerLife -= 2;
        }
    }

    //If in win area and enemy not in alarm
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("WIN") && canOpenDoor == true)
        {
            win.SetActive(true);
        }
    }
}

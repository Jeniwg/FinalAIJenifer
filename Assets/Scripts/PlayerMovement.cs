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

        inputMovement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));


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

    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Debug.DrawRay(transform.position, transform.forward, Color.yellow);
        if (Physics.Raycast(ray, out hit, 1))
        {
            if (hit.transform.tag == "Door" && Input.GetKeyDown(KeyCode.E))
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
        Instantiate(bullet, bulletEmitter.transform.position, transform.rotation);
    }

}

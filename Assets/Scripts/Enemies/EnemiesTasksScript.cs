using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Panda;
using UnityEngine.AI;

public class EnemiesTasksScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private int roamRadius = 10;
    [SerializeField]
    private bool alertOverr = false;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private GameObject bulletEmitter;
    [SerializeField]
    private float bulletRate = 0.3f;
    [SerializeField]
    private float life = 10;
    [SerializeField]
    private BlockDoor door;
    private float lastBullet;
    private Vector3 InicialPos;
    private RaycastHit hit;
    private Vector3 playerLastPosition;
    private bool Aware = false;


    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        InicialPos = agent.transform.position;
        lastBullet = Time.time;
    }

    private void Update()
    {
        if (life <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void OnEnable()
    {
        EventManager.alarm += GetPlayer;
    }

    void OnDisable()
    {
        EventManager.alarm -= GetPlayer;
    }

    private void GetPlayer()
    {
        alertOverr = true;
        playerLastPosition = player.transform.position;
        this.Aware = true;
    }

    [Task]
    private bool SeePlayer()
    {
        Vector3 rayDirection = player.transform.position - gameObject.transform.position;

        if (Physics.Raycast(gameObject.transform.position, rayDirection, out hit) && hit.collider.CompareTag("Player"))
        {
            playerLastPosition = player.transform.position;
            Aware = true;
            return true;
        }
        return false;
    }

    [Task]
    private void AlertOverride()
    {
        if (alertOverr)
        {
            alertOverr = false;
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    private void AwareOfPlayer()
    {
        if (Aware)
        {
            agent.destination = playerLastPosition;
            if (Vector3.Distance(agent.transform.position, playerLastPosition) <= 0.5)
            {
                alertOverr = false;
                Aware = false;
                Task.current.Succeed();
            }
        }
        else
        {
            Aware = false;
            Task.current.Fail();
        }

    }

    [Task]
    private void MoveRandom()
    {
        Vector3 randomPosition = RandomNavSphere(agent.transform.position, 15f, -1);
        agent.destination = randomPosition;
        Task.current.Succeed();
    }

    [Task]
    private void Chase()
    {
        Vector3 aux1 = player.transform.position;
        Vector3 aux2 = transform.position;
        aux1.y = 0;
        aux2.y = 0;
        transform.forward = aux1 - aux2;
        agent.destination = playerLastPosition;

        if ((Time.time - lastBullet > bulletRate))
        {
            Shoot();
            lastBullet = Time.time;
        }

        Task.current.Succeed();
    }

    [Task]
    private void InAlert()
    {
        Vector3 aux1 = player.transform.position;
        Vector3 aux2 = transform.position;
        aux1.y = 0;
        aux2.y = 0;
        transform.forward = aux1 - aux2;
        agent.destination = playerLastPosition;
        EventManager.go = true;
        if ((Time.time - lastBullet > bulletRate))
        {
            Shoot();
            lastBullet = Time.time;
        }
        Task.current.Succeed();
    }

    [Task]
    private void Panic()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += playerLastPosition;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
        Vector3 finalPosition = hit.position;
        agent.destination = finalPosition;
    }

    [Task]
    private void BeAlert()
    {
        agent.destination = InicialPos;
        door.CanOpenDoors = true;
        if (SeePlayer())
        {
            EventManager.go = true;
            Task.current.Succeed();
        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        randomDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        return navHit.position;
    }

    private void Shoot()
    {
        GameObject obj = Instantiate(bullet, bulletEmitter.transform.position, transform.rotation);
        obj.tag = "Enemy";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerBullet")
        {
            life -= 2;
        }
    }

}

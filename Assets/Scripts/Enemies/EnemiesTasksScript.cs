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
    private int roamRadius = 5;
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
    private Vector3 randomPosition;
    private bool aware = false;
    private bool canChange = true;
    private Vector3 oldPos;
    private float positionBlockTimer = 2f;



    private void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        InicialPos = agent.transform.position;
        lastBullet = Time.time;
        oldPos = agent.transform.position;
    }

    private void Update()
    {
        // Debug.Log(alertOverr);
        if (life <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void OnEnable()
    {
        EventManager.alarm += GetPlayer;
        EventManager.forget += ForgetPlayer;
    }

    void OnDisable()
    {
        EventManager.alarm -= GetPlayer;
        EventManager.forget -= ForgetPlayer;
    }

    private void GetPlayer()
    {
        alertOverr = true;
        playerLastPosition = player.transform.position;
        this.aware = true;
    }

    private void ForgetPlayer()
    {
        //Debug.Log("FORGETPLAYER");
        alertOverr = false;
    }

    [Task]
    private bool SeePlayer()
    {
        Vector3 rayDirection = player.transform.position - gameObject.transform.position;

        if (Physics.Raycast(gameObject.transform.position, rayDirection, out hit) && hit.collider.CompareTag("Player"))
        {
            playerLastPosition = player.transform.position;
            StopCoroutine(Change());
            aware = true;
            return true;
        }
        return false;
    }

    [Task]
    private bool AlertOverride()
    {
        if (alertOverr)
        {
            //alertOverr = false;
            return true;
        }
        return false;
    }

    [Task]
    private bool Aware()
    {
        if (aware)
        {
            change = true;
            return true;
        }

        return false;

    }

    [Task]
    private void MoveRandom()
    {
        aware = false;
        RandomPos();
        agent.destination = randomPosition;
        if (Vector3.Distance(agent.transform.position, randomPosition) <= 1f)
        {
            canChange = true;
        }

        if (agent.transform.position == oldPos)
        {
            positionBlockTimer -= Time.deltaTime;
            if (positionBlockTimer <= 0)
            {
                canChange = true;
                positionBlockTimer = 2f;
            }
        }
        oldPos = agent.transform.position;
        Task.current.Succeed();
    }

    private void RandomPos()
    {
        if (canChange)
        {
            randomPosition = RandomNavSphere(agent.transform.position, 8f, -1);
            if (randomPosition == new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity))
            {
                RandomPos();
            }
            canChange = false;
        }
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
        Debug.Log("ININALERT");
        EventManager.go = true;
        if ((Time.time - lastBullet > bulletRate))
        {
            Shoot();
            lastBullet = Time.time;
        }
        Task.current.Succeed();
    }

    private bool change = false;
    private bool check = false;
    [Task]
    private void Panic()
    {
        agent.destination = playerLastPosition;
        if (Vector3.Distance(agent.transform.position, playerLastPosition) <= 1f)
        {
            check = true;
            change = true;
        }

        if (check)
        {
            Vector3 randomDirection = Random.insideUnitSphere * roamRadius + playerLastPosition;
            agent.destination = randomDirection;
        }


        if (change)
        {
            change = false;
            StartCoroutine("Change");
        }
    }
    private IEnumerator Change()
    {
        yield return new WaitForSeconds(15);

        check = false;
        aware = false;
    }

    [Task]
    private void BeAlert()
    {
        agent.destination = InicialPos;
        //EventManager.forgetIt = true;
        alertOverr = false;
        if (SeePlayer())
        {
            Debug.Log("INBEALERT");
            Task.current.Succeed();
        }
        Task.current.Fail();
    }

    [Task]
    private void TurnOffAlarm()
    {
        EventManager.unlock = true;
        aware = false;
    }

    [Task]
    private void GoBack()
    {
        agent.destination = InicialPos;
        EventManager.forgetIt = true;
        Task.current.Succeed();
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

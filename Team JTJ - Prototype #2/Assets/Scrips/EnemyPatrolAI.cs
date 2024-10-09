using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolAI : MonoBehaviour
{
    private GameObject player;
    NavMeshAgent agent;
    private Animator anim;

    public bool isMoving;    

    [SerializeField] LayerMask groundLayer, playerLayer;

    //patrol 
    Vector3 destPoint;
    bool walkedToPoint;
    bool playerInSight;
    bool playerInCoughtRange;

    public int distToChange;

    [SerializeField] float range;   
    [SerializeField] float playerRange;
    [SerializeField] float catchRange;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player");
        anim = GetComponent<Animator>();

        isMoving = false;
        //anim.SetBool("isWalking",true);
    }

    // Update is called once per frame
    void Update()
    {
        playerInSight = Physics.CheckSphere(transform.position, playerRange, playerLayer);
        playerInCoughtRange = Physics.CheckSphere(transform.position, catchRange, playerLayer);

        if(!playerInSight && !playerInCoughtRange) Patrol();
        if (playerInSight && !playerInCoughtRange) Chase();
        if (playerInSight && playerInCoughtRange) Catch();
    }

    private void Chase()
    {
        agent.SetDestination(player.transform.position);
    }

    private void Catch()
    {

    }

    private void Patrol()
    {
        if (!walkedToPoint)
        {
            SearchForDest();
        }

        if (walkedToPoint)
        {
            agent.SetDestination(destPoint);
            isMoving = true;

            if (isMoving == true)
            {
                anim.SetBool("isWalking", true);
            }
        }

        if (Vector3.Distance(transform.position, destPoint) < distToChange)
        {
            walkedToPoint = false;
        }
    }

    private void SearchForDest()
    {
        float z = Random.Range(-range, range);
        float x = Random.Range(-range, range);
        //anim.SetBool("isWalking", true);

        destPoint = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
              
        
        if (Physics.Raycast(destPoint, Vector3.down, groundLayer))
        {
            walkedToPoint = true;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.CompareTag("obstacle"))
    //    {
    //        walkedToPoint = false ;            
    //    }
    //}
}

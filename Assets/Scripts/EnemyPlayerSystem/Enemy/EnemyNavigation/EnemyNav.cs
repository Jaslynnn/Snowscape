using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNav : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks = 4;
    bool alreadyAttacked;

    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Linked scripts
    public PlayerClass playerClass;
    public PlayerAttack playerAttack;
    public GameController gameController;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        if (gameController.State == GameController.GameStateEnums.Tutorial || gameController.State == GameController.GameStateEnums.Started)
        {
            if(playerClass.PlayerHealth > 0)
            {


            playerInSightRange = Physics.CheckSphere(transform.position, sightRange , whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        UnityEngine.AI.NavMeshAgent Agent = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (Agent.enabled == true )
        {
            
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();

                    if (playerAttack.ActionState == PlayerAttack.PlayerActionStates.Attack)
                    {
                        if (playerInAttackRange && playerInSightRange) AttackPlayer();

                    }
        }


            }
            //Check for sight and attack range
        }
    }

    //Makes the Fiends move around the place randomly
    private void Patroling()
    {
        agent.gameObject.GetComponent<Renderer>().material.color = Color.green;
        //Debug.Log("Patrolling");
        if (!walkPointSet) SearchWalkPoint();
        if(walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if( distanceToWalkPoint.magnitude <1f)
            walkPointSet = false;
    }


    private void SearchWalkPoint()
    {

        float randomZ = Random.Range( -walkPointRange, walkPointRange );
        float randomX = Random.Range( -walkPointRange, walkPointRange );

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            //Debug.Log("WalkPOint" +  walkPoint);
            walkPointSet = true;

    }

    private void ChasePlayer()
    {
        if (agent == enabled)
        {
            agent.SetDestination(player.position);
            agent.gameObject.GetComponent<Renderer>().material.color = Color.blue;
        }
    }


    private void AttackPlayer() 
    {
        if(agent == enabled)
        {
        agent.SetDestination(player.position);
        transform.LookAt(player);

        }
        if (!alreadyAttacked)
        {
            
            //Deduct the health of the player
            //Knocks the player backwards, 3 consecutive strikes will set the player flying
            //if the player hits something at a certain velocity, they destroy the object/ object will fracture

            //Place the attack code in here
            //Debug.Log("Attacking rn");
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        agent.gameObject.GetComponent<Renderer>().material.color = Color.red;

    }
    //Grab fiend to use as shield


    private void ResetAttack()
    {

        alreadyAttacked = false;
        playerClass.PlayerHealth -= 10;
        Debug.Log(playerClass.PlayerHealth);

    }




}

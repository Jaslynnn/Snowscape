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
    public UIManager uiManager;
    [SerializeField] EnemyAnimation enemyAnimation;
    public CameraShakeTrigger cameraShakeTrigger;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        /*if (enemyAnimation != null)
        {
            Debug.Log("enemyAnimation found");
        }*/
    }

    private void Update()
    {

        if (gameController.State == GameController.GameStateEnums.Tutorial || gameController.State == GameController.GameStateEnums.Started)
        {
            if(playerClass.playerHealth > 0)
            {


            playerInSightRange = Physics.CheckSphere(transform.position, sightRange , whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        UnityEngine.AI.NavMeshAgent Agent = transform.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (Agent.enabled == true )
        {
            
            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();


            if (playerInAttackRange && playerInSightRange) AttackPlayer();

                   
        }


            }
            //Check for sight and attack range
        }
        // Ensure the GameObject's rotation on the X-axis stays at 0
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        // update blend tree parameter 
        float currentSpeed = agent.velocity.magnitude;
        // Only update the animator if not attacking. (Assuming enemyAnimation has an isAttacking flag accessible)
        if (!enemyAnimation.isAttacking)
        {
            enemyAnimation.UpdateMovementSpeed(currentSpeed);
        }
        //Debug.Log("Enemy nav speed set to: " + currentSpeed);





    }

    //Makes the Fiends move around the place randomly
    private void Patroling()
    {
        //enemyAnimation.PlayFiendWalk();

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
            //Camera Shake
            
            cameraShakeTrigger.GenerateImpulseSource();
            enemyAnimation.PlayFiendAttack();
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        agent.gameObject.GetComponent<Renderer>().material.color = Color.red;

    }
    //Grab fiend to use as shield


    private void ResetAttack()
    {

        alreadyAttacked = false;
        playerClass.playerHealth -= 10;
        uiManager.UpdatePlayerHealthBar();
        Debug.Log(playerClass.playerHealth);

    }
    
}

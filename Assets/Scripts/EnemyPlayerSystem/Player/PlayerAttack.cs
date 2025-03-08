using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.AI.Navigation;
using UnityEngine.Serialization;

/// <summary>
/// This script 
/// Select the closest item/ enemy to the player 
/// if the player collides into smth, add it to list
/// if player presses mouse click to attack,
/// if tag == fiend , foreach item in fiend list, search and deduct health points
/// else
/// create a class for it based on its tag and add it to the list of objects
/// then
/// deduct health points using the damage value of the weapon class(player current damage value)
/// if health of the object = 0
/// Destroy object 
/// if object.tag = fiend, add to enemyDefeatedCounter
/// Add to enem
/// 
/// </summary>
public class PlayerAttack : MonoBehaviour
{

    [FormerlySerializedAs("Destructables")] public LayerMask destructables;
    public GameObject playerMesh;
    //public GameObject currentEnemy;
    //public GameObject currentGrabObject;
    public GameObject objectToolTipUI;
    public GameObject playerGrabPoint;
    public NavMeshSurface navMeshSurface;

    public bool selectedObject;

    public float hitDistance = 2f;
    [SerializeField] LayerMask layermask;
    public int layer;


    public List<GameObject> collidedItems = new List<GameObject>();
    public List<GameObject> rayCastedItems = new List<GameObject>();
    [FormerlySerializedAs("DestructionPieces")] public GameObject[] destructionPieces;
    public GameObject itemParent;
    public bool attackedEnemy;
    public bool grabbedObject;

    //Lists for enemies/Things attacked
    //public List<EnemyTranslationTableEntry> enemies = new List<EnemyTranslationTableEntry>();

    [FormerlySerializedAs("EnemyTranslationType")]
    [Header("Linked scripts")]
    //Attached Scripts
    [SerializeField] BaseEnemyTranslation enemyTranslationType;
    BaseEnemyTranslation _enemyTranslation;
    public EnemyTracker enemyTracker;
    public PlayerClass playerClass;
    public ThirdPersonMovement thirdPersonMovement;
    public Coroutine AttackEnemyCoroutine;
    public GameController gameController;
    public CurrentEnemyClass currentEnemyClass;
    public EnemyHealthBar enemyHealthBar;
    public enum PlayerActionStates
    {
        Null,
        Attack, 
        Defense
    }

    [FormerlySerializedAs("ActionState")] public PlayerActionStates actionState;


    private void Awake()
    {
        _enemyTranslation = ScriptableObject.Instantiate(enemyTranslationType);
        
      
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (gameController.State == GameController.GameStateEnums.Tutorial || gameController.State == GameController.GameStateEnums.Started)
        {


            //CollisionItemParent = collision.transform.parent.gameObject;


            collidedItems.Add(collision.gameObject);



        if (destructables == (destructables | (1 << collision.gameObject.layer)))
        {
            //gameOver


        }



        }

    }
    public void OnTriggerStay(Collider collision)
    {
        if (gameController.State == GameController.GameStateEnums.Tutorial || gameController.State == GameController.GameStateEnums.Started)
        {



            //Debug.Log("This is the layer number" + layer);
            //for each item that is collided to, add to list make red and add a raycast, then take the raycast distance that is the shortest and make dark red
            for (int i = 0; i < collidedItems.Count; i++)
        {
            GameObject item = collidedItems[i];



            //collidedItems.RemoveAll(s => s == null);
            if (item && item.layer == 6)
            {

                Ray ray = new Ray(playerMesh.transform.position, -(playerMesh.transform.position - item.transform.position).normalized);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, hitDistance, layermask, QueryTriggerInteraction.Ignore))
                {
            
                    //rayCastedItems.Add(hit.transform.gameObject);
                    //if (!iList.Contains(value)) iList.Add(value)
                    rayCastedItems.RemoveAll(s => s == null);
                    if (!rayCastedItems.Contains(hit.transform.gameObject))
                    {
                        rayCastedItems.Add(hit.transform.gameObject);
                        SelectClosestEnemy();
                // ShowActionTooltipUI(currentEnemyClass.currentEnemy);
                        //ShowDestroyTooltipUI(hit.transform.gameObject);
                        //OldColor = lastHighlighted.GetComponent<Renderer>().material.color;
                        //hit.transform.GetComponent<Renderer>().material.color = Color.red;
                    }
                    //lastHighlighted.GetComponent<Renderer>().material.color = OldColor;
                    //myVector1 - myVector2).normalized
                    //lastHighlighted.GetComponent<Renderer>().material.color = Color.blue;

                    //Debug.DrawRay(playerMesh.transform.position, -(playerMesh.transform.position - item.transform.position).normalized * hit.distance, Color.red);
                    if(hit.distance < hitDistance)
                    {
                        //print(hit.distance);
                    Debug.DrawRay(playerMesh.transform.position, -(playerMesh.transform.position - item.transform.position).normalized * hit.distance, Color.blue);

                    }


                }
                else
                {
                    //lastHighlighted.GetComponent<Renderer>().material.color = OldColor;
                    //item.GetComponent<Renderer>().material.color = Color.blue;
                    rayCastedItems.Remove(item);
                }



            }



        }

        ChangeActionState();

        }


    }



    public void ChangeActionState()
    {
        switch (actionState)
        {
            case PlayerActionStates.Null:

                //%%TestingLIne only
                actionState = PlayerActionStates.Attack;
                //If game started, then change to the attack state;

                break;

            case PlayerActionStates.Attack:
                thirdPersonMovement.speed = 6f;
                if (Input.GetMouseButtonDown(0))
                {
                    //Change the state here
                    //Debug.Log("Attacking" + currentEnemy);
                    attackedEnemy = true;
                    AttackEnemyCoroutine = StartCoroutine(AttackCurrentEnemy());
                }
                if (Input.GetMouseButtonDown(1))
                {
                    StartCoroutine(GrabCurrentEnemy()); 
                    

                }
                    break;

            case PlayerActionStates.Defense:
                //Reduce the speed of the player and prevent it from recieving damage from the enemy
                thirdPersonMovement.speed = 3f;
         if (Input.GetMouseButtonDown(1))
                {
                    StartCoroutine(ReleaseCurrentEnemy());

                }

                
                break;
        }
    }



    

    //Attack the first item in the raycasted list
    //if the item already existed in the attacked enemies, check the type of the item, if fiend then etc ....
    //then grab the health and do damage to the health using the already created function in the base enemies translation table
    //else Create a class by detecting the tag of that item, add it to a list of the enemy classes
    //Make each item take damage, use the already created function , turn it into a health bar
    public void SelectClosestEnemy()
    {
        if (rayCastedItems.Count > 0 )
        {

            currentEnemyClass.currentEnemy = rayCastedItems[rayCastedItems.Count - 1];
        
            selectedObject = true;
            
        }

     


    }

    //Make this an ienumerator so that there is a cool down when attacking so that the player cannot spam the attack button

    public IEnumerator AttackCurrentEnemy()
    {
        //Play player attacking animation
        //GRab enemy prefab and find the enemy UI bar , then set active and assign that item's value to the 
            yield return new WaitForSeconds(.2f);
        if (attackedEnemy)
        {
            currentEnemyClass.attackedEnemy = currentEnemyClass.currentEnemy;
            //currentEnemyClass.currentEnemy.GetComponent<Renderer>().material.color = Color.white;
            enemyTracker.TakeDamage(currentEnemyClass.currentEnemy.gameObject, playerClass.playerDamageValue, currentEnemyClass.currentEnemy.tag);
            if (!currentEnemyClass.attackedEnemy.CompareTag("Fiend") )
            {
                if(currentEnemyClass.attackedEnemyHealth <= 0)
                {
                if(playerClass.playerHealth <= 100)
                    {
                        playerClass.playerHealth += 10;
                    }

            Fragmentation(currentEnemyClass.attackedEnemy);

                }
            }
            attackedEnemy = false;
        }
    }

    public IEnumerator GrabCurrentEnemy()
    {
        yield return new WaitForSeconds(.2f);
        
        if (!grabbedObject)
        {
            playerClass.currentGrabbedObject = currentEnemyClass.currentEnemy.gameObject;
            var playerOffset = new Vector3(playerGrabPoint.transform.position.x, playerGrabPoint.transform.position.y , playerMesh.transform.position.z + (playerClass.currentGrabbedObject.transform.localScale.z / 2) + (playerGrabPoint.transform.localScale.z / 2));
            Rigidbody grabbedRb = playerClass.currentGrabbedObject.GetComponent<Rigidbody>();
            if (!grabbedRb)
            {
                grabbedRb = playerClass.currentGrabbedObject.AddComponent<Rigidbody>();
            }
            grabbedRb.useGravity = false;
            grabbedRb.constraints = RigidbodyConstraints.FreezeAll;
            playerClass.currentGrabbedObject.transform.position = playerOffset;
            BakeNavMesh();

            playerClass.currentGrabbedObject.transform.parent = playerMesh.transform;
            playerClass.currentGrabbedObject.transform.rotation = playerMesh.transform.rotation;
           
          actionState = PlayerActionStates.Defense;
            grabbedObject = true;
            
            //*** If tag == fiend Play the current Enemy struggling animation here

            if (playerClass.currentGrabbedObject.CompareTag("Fiend")) 
            {
                UnityEngine.AI.NavMeshAgent agent = playerClass.currentGrabbedObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
                agent.enabled = false;
            
            }
        }

    }

    public IEnumerator ReleaseCurrentEnemy()
    {
            playerClass.currentGrabbedObject.transform.parent = null;
        yield return new WaitForSeconds(.1f);
        if (grabbedObject)
        {
          

            //Adds force when releasing the item
            Rigidbody grabbedRb = playerClass.currentGrabbedObject.GetComponent<Rigidbody>();
            if (!grabbedRb)
            {
                /*
                Rigidbody pieceRbNew = piece.AddComponent<Rigidbody>();
                Vector3 direction = pieceRbNew.transform.position - transform.position;
                */
                grabbedRb = playerClass.currentGrabbedObject.AddComponent<Rigidbody>();
            }
            grabbedRb.isKinematic = false;
            grabbedRb.useGravity = true;
            grabbedRb.constraints = RigidbodyConstraints.None;
            // Calculate the direction and force to apply
            Vector3 direction = grabbedRb.transform.position - transform.position;
            direction.Normalize();  // Make sure the direction is normalized

            // Add force in the direction
            grabbedRb.AddForce(direction * 1f, ForceMode.Impulse);  // Adjust the force magnitude as needed
            enemyTracker.TakeDamage(playerClass.currentGrabbedObject.gameObject, playerClass.playerThrowDamageValue, currentEnemyClass.currentEnemy.tag);

            Debug.Log($"Force applied with magnitude: {direction.magnitude * 1f}");
            if (playerClass.currentGrabbedObject.CompareTag("Fiend"))
            {
                UnityEngine.AI.NavMeshAgent agent = playerClass.currentGrabbedObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
                agent.enabled = true;

            }

            Debug.Log("huh");
            playerClass.currentGrabbedObject = null;
        grabbedObject = false;
            actionState = PlayerActionStates.Attack;
            BakeNavMesh();

        }
    }

    public void BakeNavMesh()

    {

        if (navMeshSurface != null)

        {

            navMeshSurface.BuildNavMesh();
        }

    }


    public void ShowActionTooltipUI(GameObject item)
    {
        //Parent it under the gaemObject item
        if (selectedObject )
        {
        var aboveItem = new Vector3(item.transform.position.x, item.transform.position.y + (item.transform.position.y / 2) + 2.5f, item.transform.position.z);
        var selectedItemUI = Instantiate(objectToolTipUI, aboveItem, transform.rotation);
            selectedItemUI.transform.parent = item.transform;
            selectedObject = false;
            if (currentEnemyClass.currentEnemy != null)
            {
            Destroy(selectedItemUI, 2f);
            }

        }
        
        //display the tooltip

    }


    public void OnTriggerExit(Collider collision)
    {
        if (destructables == (destructables | (1 << collision.gameObject.layer)))
        {

            //lastHighlighted.GetComponent<Renderer>().material.color = Color.blue;
        }

        rayCastedItems.RemoveAll(s => s == null);
        collidedItems.Remove(collision.gameObject);

    }




    public void Fragmentation(GameObject item)
    {
        //CinemachineShake.Instance.ShakeCamera(5f, .1f);
        //FindObjectOfType<AudioManager>().Play("Breaking");
        itemParent = item.transform.parent.gameObject;

        //XPTracker.AddXP(ItemsTranslationType.AddXP(item.tag));
        Debug.Log("I am adding points");

        //uiManager.ShowAddedPoints(ItemsTranslationType.AddXP(item.tag));

        //Debug.Log("PlayerHitScript : " + playerClass.xpPoints);
        //XPTracker.RefreshDisplays();

        //StartCoroutine(SpawnDestroyVFX(item.transform.position));
        

        //inRangeItem.SetActive(false);

        collidedItems.RemoveAll(s => s == null);

        //Get the parent of the gameobject,
        //for each child in the parent we want to assign a rigidbody to it
        //Add all the childObjects into a list

        destructionPieces = new GameObject[itemParent.transform.childCount];
        for (int i = 0; i < destructionPieces.Length; i++)
        {
            destructionPieces[i] = itemParent.transform.GetChild(i).gameObject;
        }

        foreach (GameObject piece in destructionPieces)
        {
            Rigidbody pieceRb = piece.GetComponent<Rigidbody>();
            if (!pieceRb)
            {
                // Vector3 direction = pieceRb.transform.position - transform.position;
                //pieceRb.AddForceAtPosition(direction.normalized, transform.position* 100f);

                Rigidbody pieceRbNew = piece.AddComponent<Rigidbody>();
                Vector3 direction = pieceRbNew.transform.position - transform.position;
                pieceRbNew.AddForceAtPosition(direction.normalized, transform.position * 100f);
                piece.layer = 0;

                rayCastedItems.Remove(piece);

                //change the layer of the item into default so that the raycast cannot detect it 
                //Add the points for the thing here
            }



            // cannot add the points
            //Assign a rigidbody to each piece
            //Rigidbody pieceRb = pieceGO.getComponent<Rigidbody>();

        }
        //Add some force into the middle of the item to make the fragments fly out



    }



}
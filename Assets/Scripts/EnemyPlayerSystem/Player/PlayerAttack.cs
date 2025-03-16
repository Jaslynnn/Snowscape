using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
    public GameObject explosionPrefab;
    public GameObject snowBombDroppedPrefab;
    private Quaternion targetRotation; // Store target rotation
    public float rotationSpeed = 5f;
    
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

    [SerializeField] YettyAnimation yettyAnimation;
    
    public enum PlayerActionStates
    {
        Null,
        Attack, 
        Defense
    }

    public enum PlayerWeaponState
    {
        Hit,
        Grab,
        Bomb
    }

    [FormerlySerializedAs("ActionState")] public PlayerActionStates actionState;

    public PlayerWeaponState weaponState;


    private void Awake()
    {
        _enemyTranslation = ScriptableObject.Instantiate(enemyTranslationType);
        
   }
    
    private void FixedUpdate()
    {
        if (actionState == PlayerActionStates.Attack)
        {
            if (playerMesh)
            {
                playerMesh.transform.rotation = Quaternion.Slerp(
                    playerMesh.transform.rotation,
                    targetRotation,
                    Time.deltaTime * rotationSpeed
                );
            }
        }
  
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

                    Debug.DrawRay(playerMesh.transform.position, -(playerMesh.transform.position - item.transform.position).normalized * hit.distance, Color.red);
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

//Weapon and attack is checked in the gameController script update instead of here
              

                break;

            case PlayerActionStates.Attack:
                break;

            case PlayerActionStates.Defense:
             
                
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
        //GRab enemy prefab and find the enemy UI bar , then set active and assign that item's value to the 
            yield return new WaitForSeconds(.1f);
            yettyAnimation.PlayYettyAttackStick();

            actionState = PlayerActionStates.Attack;
            if (weaponState == PlayerWeaponState.Hit)
            {
                StartCoroutine(HitCurrentEnemy());
            }

            if (weaponState == PlayerWeaponState.Grab)
            {
                GrabCurrentEnemy();
            }


            if (weaponState == PlayerWeaponState.Bomb)
            {
                if (playerClass.noOfBombs > 0)
                {
                    //Put function for dropping bomb here
                    //release the bomb transform parent , 
                    StartCoroutine(DropBomb());
                }
                else
                {
                    yield return new WaitForSeconds(3f);
                    weaponState = PlayerWeaponState.Hit;
                }

            }

            yield return new WaitForSeconds(1f);
            actionState = PlayerActionStates.Null;
    }

    public IEnumerator DropBomb()
    {
        snowBombDroppedPrefab.SetActive(true);
        snowBombDroppedPrefab.transform.parent = null;
        playerClass.noOfBombs -= 1;
        yield return new WaitForSeconds(5f);
        snowBombDroppedPrefab.transform.parent = playerGrabPoint.transform;
        snowBombDroppedPrefab.SetActive(false);
    }

    public IEnumerator HitCurrentEnemy()
    {
        if (attackedEnemy)
        {
            
            currentEnemyClass.attackedEnemy = currentEnemyClass.currentEnemy;
            
            if (!currentEnemyClass.attackedEnemy)
            {
                Debug.Log("Nothing to Attack");
                targetRotation = transform.rotation;
            }
            else
            {
                Vector3 targetPosition = currentEnemyClass.attackedEnemy.transform.position;

                
                targetPosition.y = playerMesh.transform.position.y; // Keep the Y-axis unchanged
                targetRotation = Quaternion.LookRotation(targetPosition - playerMesh.transform.position);

                //currentEnemyClass.currentEnemy.GetComponent<Renderer>().material.color = Color.white;
                enemyTracker.TakeDamage(currentEnemyClass.currentEnemy.gameObject, playerClass.playerDamageValue,
                    currentEnemyClass.currentEnemy.tag);
                if (!currentEnemyClass.attackedEnemy.CompareTag("Fiend"))
                {
                    if (currentEnemyClass.attackedEnemyHealth <= 0)
                    {
                        if (playerClass.playerHealth <= 100)
                        {
                            playerClass.playerHealth += 10;
                        }

                        // Fragmentation(currentEnemyClass.attackedEnemy);

                    }
                }
 
                attackedEnemy = false;
                yield return new WaitForSeconds(.5f);
                GameObject explosion = Instantiate(explosionPrefab, targetPosition, Quaternion.identity);
                Destroy(explosion, 1f); // Destroy the explosion effect after 1 second
                
            }

        }  
    }

    public void GrabCurrentEnemy()
    {
        actionState = PlayerActionStates.Defense;
        yettyAnimation.PlayYettyGrab();
        if (!grabbedObject && currentEnemyClass.currentEnemy)
        {
            actionState = PlayerActionStates.Attack;
            playerClass.currentGrabbedObject = currentEnemyClass.currentEnemy.gameObject;
            Rigidbody grabbedRb = playerClass.currentGrabbedObject.GetComponent<Rigidbody>() ?? playerClass.currentGrabbedObject.AddComponent<Rigidbody>();
            grabbedRb.useGravity = false;
            grabbedRb.constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine(MoveToGrabPoint(playerClass.currentGrabbedObject));
            BakeNavMesh();
            playerClass.currentGrabbedObject.transform.parent = playerMesh.transform;
            playerClass.currentGrabbedObject.transform.rotation = playerMesh.transform.rotation;
            actionState = PlayerActionStates.Defense;
            grabbedObject = true;
            if (playerClass.currentGrabbedObject.CompareTag("Fiend"))
            {
                
                UnityEngine.AI.NavMeshAgent agent = playerClass.currentGrabbedObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
                if (agent != null)
                {
                    agent.enabled = false;
                }
                
            }
        }
        else { Debug.Log("Nothing to Grab"); }
    }

    private IEnumerator MoveToGrabPoint(GameObject grabbedObject)
    {
        float duration = 1.5f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            grabbedObject.transform.position = Vector3.Lerp(grabbedObject.transform.position, playerGrabPoint.transform.position, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        grabbedObject.transform.position = playerGrabPoint.transform.position;
    }

    public IEnumerator ReleaseCurrentEnemy()
    {
            playerClass.currentGrabbedObject.transform.parent = null;
      
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

            grabbedRb.useGravity = true;
            grabbedRb.constraints = RigidbodyConstraints.None;
            grabbedRb.isKinematic = false;
            // Calculate the direction and force to apply
            Vector3 direction = playerMesh.transform.forward; 
            direction.Normalize();  // Make sure the direction is normalized

            // Add force in the direction
            grabbedRb.AddForce(direction * 11f, ForceMode.Impulse);  // Adjust the force magnitude needed
            // enemyTracker.TakeDamage(playerClass.currentGrabbedObject.gameObject, playerClass.playerThrowDamageValue, currentEnemyClass.currentEnemy.tag);

            Debug.Log($"Force applied with magnitude: {direction.magnitude * 1f}");
            if (playerClass.currentGrabbedObject.CompareTag("Fiend"))
            {
                yield return new WaitForSeconds(2f);
                UnityEngine.AI.NavMeshAgent agent = playerClass.currentGrabbedObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
                agent.enabled = true;
               

            }

            grabbedRb.isKinematic = true; 
            Debug.Log("huh");
            playerClass.currentGrabbedObject = null;
        grabbedObject = false;
            actionState = PlayerActionStates.Attack;
            BakeNavMesh();
            yield return new WaitForSeconds(1f);
            actionState = PlayerActionStates.Null; 
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
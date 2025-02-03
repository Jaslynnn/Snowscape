using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cinemachine;
/// <summary>
/// This script 
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

    public LayerMask Destructables;
    public GameObject playerMesh;

    public float hitDistance = 2f;
    [SerializeField] LayerMask layermask;
    public int layer;
    private GameObject lastHighlighted = null;


    public List<GameObject> collidedItems = new List<GameObject>();
    public List<GameObject> rayCastedItems = new List<GameObject>();
    public GameObject[] DestructionPieces;
    public GameObject itemParent;

    public void OnTriggerEnter(Collider collision)
    {

        //CollisionItemParent = collision.transform.parent.gameObject;


        collidedItems.Add(collision.gameObject);



        if (Destructables == (Destructables | (1 << collision.gameObject.layer)))
        {
            lastHighlighted = collision.gameObject;
            //gameOver


        }


    }
    public void OnTriggerStay(Collider collision)
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
                    lastHighlighted = hit.transform.gameObject;
                    if (!rayCastedItems.Contains(hit.transform.gameObject))
                    {
                        rayCastedItems.Add(hit.transform.gameObject);
                        //ShowDestroyTooltipUI(hit.transform.gameObject);
                        //OldColor = lastHighlighted.GetComponent<Renderer>().material.color;
                        hit.transform.GetComponent<Renderer>().material.color = Color.red;
                    }
                    //lastHighlighted.GetComponent<Renderer>().material.color = OldColor;
                    //myVector1 - myVector2).normalized
                    //lastHighlighted.GetComponent<Renderer>().material.color = Color.blue;

                    //Debug.DrawRay(playerMesh.transform.position, -(playerMesh.transform.position - item.transform.position).normalized * hit.distance, Color.red);
                    if(hit.distance < hitDistance)
                    {
                        print(hit.distance);
                    Debug.DrawRay(playerMesh.transform.position, -(playerMesh.transform.position - item.transform.position).normalized * hit.distance, Color.blue);

                    }


                }
                else
                {
                    //lastHighlighted.GetComponent<Renderer>().material.color = OldColor;
                    item.GetComponent<Renderer>().material.color = Color.blue;
                    rayCastedItems.Remove(item);
                }



            }



        }
    }

    public void OnTriggerExit(Collider collision)
    {
        if (Destructables == (Destructables | (1 << collision.gameObject.layer)))
        {

            lastHighlighted.GetComponent<Renderer>().material.color = Color.blue;
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

        DestructionPieces = new GameObject[itemParent.transform.childCount];
        for (int i = 0; i < DestructionPieces.Length; i++)
        {
            DestructionPieces[i] = itemParent.transform.GetChild(i).gameObject;
        }

        foreach (GameObject piece in DestructionPieces)
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
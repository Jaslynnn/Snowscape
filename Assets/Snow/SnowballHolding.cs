using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// THis script controls the making, picking up and putting down of the snowball
/// duration can be modified to make the making of the snowball faster or slower
/// </summary>
public class SnowballHolding : MonoBehaviour
{
    //Reference the snowball class script
    public SnowballClass snowballClass;
    public GameObject snowballPrefab;
    public GameObject snowballCurrent;
    public GameObject snowballSpawnMarker;
    public Transform snowballParent;
    public float maxSnowballSize;
    public float duration;
    public float timePassed;
    public Coroutine makeSnowballCoroutine;

    //Testing
    private void Update()
    {
        MakeSnowball();
        DropSnowball(snowballCurrent);
        //StopMakingSnowball();
    }

    public void MakeSnowball()
    {
        if (Input.GetKeyDown("e"))
        {
            
            if (snowballClass.snowBallHeld == false) {
                print("spawn snowball");
                GameObject snowballObj = Instantiate(snowballPrefab, snowballSpawnMarker.transform.position, snowballSpawnMarker.transform.rotation);
            snowballObj.transform.parent = snowballParent;
            snowballClass.snowBallHeld = true;
            snowballCurrent = snowballObj;
                
            if (snowballClass.snowBallHeld == true && snowballClass.snowBallSize < maxSnowballSize)
            {
                   Coroutine makeSnowballCoroutine = StartCoroutine(LerpSnowballSize(snowballCurrent, duration));
            }


            }
            if (snowballClass.snowBallHeld == true && snowballClass.snowBallSize < maxSnowballSize)
            {
                Coroutine makeSnowballCoroutine = StartCoroutine(LerpSnowballSize(snowballCurrent, duration));
            }





        }

      
    }


    IEnumerator LerpSnowballSize(GameObject snowball , float duration)
    {
        print("making snowball bigger");
        var startSize = snowball.transform.localScale;
        var endSize = startSize * maxSnowballSize;
        for (timePassed = 0f; timePassed < duration; timePassed += Time.deltaTime)
        {
            var factor = timePassed / duration;
            // [optional] add ease-in and -out
            factor = Mathf.SmoothStep(0, 1, factor);

            snowball.transform.localScale = Vector3.Lerp(startSize, endSize, factor);

            snowballClass.snowBallSize = snowball.transform.localScale.x;
            //Stop making the snowball
            if (Input.GetKeyUp("e"))
            {
                break;
            }
                yield return null;
        }
        //snowball.transform.localScale = endSize;
        //snowballObj.transform.localScale = snowballObj.transform.localScale * maxSnowballSize;

    }


    public void DropSnowball( GameObject currentSnowball)
    {
        if (currentSnowball != null)
        {

        if (Input.GetKeyDown("w"))
        {
        print("Dropped snowball");
                currentSnowball.transform.parent = null;

        }

        }
    }

}

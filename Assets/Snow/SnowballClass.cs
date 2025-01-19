using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This script has the properties of the snowball size, snowball boolean of true or false, the name of the snowball's parent 
/// *Player script link for later: if the size of the snowball is bigger, the speed of the player is slower
/// </summary>

public class SnowballClass : MonoBehaviour
{
    public string snowBallName;
    public float snowBallSize;
    public bool snowBallHeld;
    public string snowBallParent;

    public SnowballClass(string snowBallName, float snowBallSize, bool snowBallHeld , string snowBallParent)
    {
        this.snowBallName = snowBallName;
        this.snowBallSize = snowBallSize;
        this.snowBallHeld = snowBallHeld;
        this.snowBallParent = snowBallParent;
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public Sprite icon; 
    public string name; 
    public int score;   

    public PlayerScore(Sprite icon, string name, int score)
    {
        this.icon = icon;
        this.name = name;
        this.score = score;
    }
}

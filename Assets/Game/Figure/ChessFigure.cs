using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChessFigure : MonoBehaviour
{
    public int CurrentX { get; set; }
    public int CurrentY { get; set; }
    public bool isRed;

    public void SetPosition(int x, int y)
    {
        CurrentX = x;
        CurrentY = y;
    }

    public virtual bool isEight(int x, int y)
    {
        return true;
    }

    public virtual bool[,] PossibleMove()
    {
        return new bool[5, 5];
    }
}
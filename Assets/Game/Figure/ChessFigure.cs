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
        //bool eight = true;
        //switch (y % 2)
        //{
        //    case 0:
        //        if ((x % 2) == 0) eight = true;
        //        else eight = false;
        //        break;
        //    case 1:
        //        if ((x % 2) == 0) eight = false;
        //        else eight = true;
        //        break;
        //    default:
        //        break;
        //}
        return true;
    }

    public virtual bool[,] PossibleMove()
    {
        return new bool[5, 5];
    }
}
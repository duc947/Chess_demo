using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chesspiece : ChessFigure
{
    public override bool isEight(int x, int y)
    {
        bool eight = true;
        switch (y % 2)
        {
            case 0:
                if ((x % 2) == 0) eight = true;
                else eight = false;
                break;
            case 1:
                if ((x % 2) == 0) eight = false;
                else eight = true;
                break;
            default:
                break;
        }
        return eight;
    }

    public override bool[,] PossibleMove()
    {
        int i, j;
        bool[,] r = new bool[5, 5];
        ChessFigure c, c1;
        //ChessFigure c = BoardManager.Instance.ChessFigurePositions[CurrentX, CurrentY];


        //Left
        i = CurrentX - 1;
        if (i >= 0)
        {
            c = BoardManager.Instance.ChessFigurePositions[i, CurrentY];
            if (c == null) r[i, CurrentY] = true;
            else
            {
                j = CurrentX - 2;
                if (j >= 0)
                {
                    c1 = BoardManager.Instance.ChessFigurePositions[j, CurrentY];
                    if ((c.isRed != isRed) && (c1 == null)) r[j, CurrentY] = true;
                }
            }
        }

        // Right
        i = CurrentX + 1;
        if (i <= 4)
        {
            c = BoardManager.Instance.ChessFigurePositions[i, CurrentY];
            if (c == null) r[i, CurrentY] = true;
            else
            {
                j = CurrentX + 2;
                if (j <= 4)
                {
                    c1 = BoardManager.Instance.ChessFigurePositions[j, CurrentY];
                    if ((c.isRed != isRed) && (c1 == null)) r[j, CurrentY] = true;
                }
            }
        }

        //Up
        i = CurrentY + 1;
        if (i <= 4)
        {
            c = BoardManager.Instance.ChessFigurePositions[CurrentX, i];
            if (c == null) r[CurrentX, i] = true;
            else
            {
                j = CurrentY + 2;
                if (j <= 4)
                {
                    c1 = BoardManager.Instance.ChessFigurePositions[CurrentX, j];
                    if ((c.isRed != isRed) && (c1 == null)) r[CurrentX, j] = true;
                }
            }
        }

        // Down
        i = CurrentY - 1;
        if (i >= 0)
        {
            c = BoardManager.Instance.ChessFigurePositions[CurrentX, i];
            if (c == null) r[CurrentX, i] = true;
            else
            {
                j = CurrentY - 2;
                if (j >= 0)
                {
                    c1 = BoardManager.Instance.ChessFigurePositions[CurrentX, j];
                    if ((c.isRed != isRed) && (c1 == null)) r[CurrentX, j] = true;
                }
            }
        }

        //Eight direction
        if (isEight(CurrentX, CurrentY))
        {
            // Diagonal Left Up
            if (CurrentX != 0 && CurrentY != 4)
            {
                c = BoardManager.Instance.ChessFigurePositions[CurrentX - 1, CurrentY + 1];
                if (c == null) r[CurrentX - 1, CurrentY + 1] = true;
                else
                {
                    i = CurrentX - 2;
                    j = CurrentY + 2;
                    if((i >= 0) && (j <= 4))
                    {
                        c1 = BoardManager.Instance.ChessFigurePositions[i, j];
                        if ((c.isRed != isRed) && (c1 == null)) r[i, j] = true;

                    }
                }
            }

            // Diagonal Left Down
            if (CurrentX != 0 && CurrentY != 0)
            {
                c = BoardManager.Instance.ChessFigurePositions[CurrentX - 1, CurrentY - 1];
                if (c == null) r[CurrentX - 1, CurrentY - 1] = true;
                else
                {
                    i = CurrentX - 2;
                    j = CurrentY - 2;
                    if ((i >= 0) && (j >= 0))
                    {
                        c1 = BoardManager.Instance.ChessFigurePositions[i, j];
                        if ((c.isRed != isRed) && (c1 == null)) r[i, j] = true;

                    }
                }
            }

            // Diagonal Right Up    
            if (CurrentX != 4 && CurrentY != 4)
            {
                c = BoardManager.Instance.ChessFigurePositions[CurrentX + 1, CurrentY + 1];
                if (c == null) r[CurrentX + 1, CurrentY + 1] = true;
                else
                {
                    i = CurrentX + 2;
                    j = CurrentY + 2;
                    if ((i <= 4) && (j <= 4))
                    {
                        c1 = BoardManager.Instance.ChessFigurePositions[i, j];
                        if ((c.isRed != isRed) && (c1 == null)) r[i, j] = true;

                    }
                }
            }

            // Diagonal Right Down
            if (CurrentX != 4 && CurrentY != 0)
            {
                c = BoardManager.Instance.ChessFigurePositions[CurrentX + 1, CurrentY - 1];
                if (c == null) r[CurrentX + 1, CurrentY - 1] = true;
                else
                {
                    i = CurrentX + 2;
                    j = CurrentY - 2;
                    if ((i <= 4) && (j >= 0))
                    {
                        c1 = BoardManager.Instance.ChessFigurePositions[i, j];
                        if ((c.isRed != isRed) && (c1 == null)) r[i, j] = true;

                    }
                }
            }
        }
        return r;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; set; }
    private bool[,] allowedMoves { get; set; }

    public ChessFigure[,] ChessFigurePositions { get; set; }
    private ChessFigure selectedFigure;

    public GameObject Winner_Panel;
    public Text Winner_txt;
    

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessFigures;
    private List<GameObject> activeFigures = new List<GameObject>();

    private ChessAI ai;

    int red = 8, blue = 8;
    public bool isRedTurn = true;

    void Start()
    {
        Instance = this;

        Winner_Panel.SetActive(false);

        ai = new ChessAI();

        ChessFigurePositions = new ChessFigure[5, 5];

        SpawnAllChessFigures();
    }

    void Update()
    {
        //DrawChessBoard();
        UpdateSelection();

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedFigure == null)
                {
                    // Select Figure
                    SelectChessFigure(selectionX, selectionY);
                }
                else
                {
                    // Move Figure
                    MoveChessFigure(selectionX, selectionY);
                }
            }
        }

        //AI should be blue player
        if (!isRedTurn)
        {
            Vector2 aiMove = new Vector2();
            Debug.Log(String.Format("Initial Values: x = {0}, y = {1}", aiMove.x, aiMove.y));
            //selectedFigure = ai.SelectChessFigure();
            //aiMove = ai.MakeMove(selectedFigure);
            //Debug.Log("SelectedFigure" + String.Format("{0} - {1}", selectedFigure.CurrentX, selectedFigure.CurrentY));

            do
            {
                selectedFigure = ai.SelectChessFigure();
                aiMove = ai.MakeMove(selectedFigure);
                Debug.Log("selectedFigure" + String.Format("{0} - {1}", selectedFigure.CurrentX, selectedFigure.CurrentY));
                //Debug.Log(String.Format("{0} - {1}", aiMove.x, aiMove.y));
            } while (aiMove.x < 0 && aiMove.y < 0);

            SelectChessFigure(selectedFigure.CurrentX, selectedFigure.CurrentY);
            Debug.Log("Selected :" + String.Format("{0} - {1}", selectedFigure.CurrentX, selectedFigure.CurrentY));
            MoveChessFigure((int)Math.Round(aiMove.x), (int)Math.Round(aiMove.y));
            Debug.Log("Move to :" + String.Format("{0} - {1}", aiMove.x, aiMove.y));
            //Debug.Log("Red turn: " + isRedTurn);
        }
    }

    private void SelectChessFigure(int x, int y)
    {
        if (ChessFigurePositions[x, y] == null) return;
        if (ChessFigurePositions[x, y].isRed != isRedTurn) return;
        
        allowedMoves = ChessFigurePositions[x, y].PossibleMove();

        bool hasAtLeastOneMove = false;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (allowedMoves[i, j])
                {
                    hasAtLeastOneMove = true;

                    // break outer loop
                    i = 4;

                    // break inner loop
                    break;
                }
            }
        }
        if (!hasAtLeastOneMove) return;

        selectedFigure = ChessFigurePositions[x, y];
        BoardHighlighting.Instance.HighlightAllowedMoves(allowedMoves);
    }

    private void MoveChessFigure(int x, int y)
    {
        //Debug.Log("Red turn: " + isRedTurn);
        if (allowedMoves[x, y])
        //if (selectedFigure.PossibleMove(x,y))
        {
            //Debug.Log("Red turn1: " + isRedTurn);
            ChessFigure c, c1, c2, c3, c4, c5, c6, c7, c8, c9;
            int a = 0, b = 0;
            bool flag = false;
            if (Mathf.Abs(selectedFigure.CurrentX - x) == 2 || Mathf.Abs(selectedFigure.CurrentY - y) == 2)
            {
                flag = true;
                if (selectedFigure.CurrentX - x < 0)
                {
                    if (selectedFigure.CurrentY - y < 0)
                    {
                        a = x - 1;
                        b = y - 1;
                    }
                    else
                    {
                        if (selectedFigure.CurrentY - y > 0)
                        {
                            a = x - 1;
                            b = y + 1;
                        }
                        else
                        {
                            a = x - 1;
                            b = y;
                        }
                    }
                }
                else
                {
                    if (selectedFigure.CurrentX - x > 0)
                    {
                        if (selectedFigure.CurrentY - y < 0)
                        {
                            a = x + 1;
                            b = y - 1;
                        }
                        else
                        {
                            if (selectedFigure.CurrentY - y > 0)
                            {
                                a = x + 1;
                                b = y + 1;
                            }
                            else
                            {
                                a = x + 1;
                                b = y;
                            }
                        }
                    }
                    else
                    {
                        if (selectedFigure.CurrentY - y < 0)
                        {
                            a = x;
                            b = y - 1;
                        }
                        else
                        {
                            if (selectedFigure.CurrentY - y > 0)
                            {
                                a = x;
                                b = y + 1;
                            }
                        }
                    }
                }
                //Debug.Log("selectedFigure.CurrentX : " + selectedFigure.CurrentX);
                //Debug.Log("selectedFigure.CurrentY : " + selectedFigure.CurrentY);
                //Debug.Log("x : " + x);
                //Debug.Log("y : " + y);
                //Debug.Log("a : " + a);
                //Debug.Log("b : " + b);
                c9 = ChessFigurePositions[a, b];
                updateStatus(c9);
                activeFigures.Remove(c9.gameObject);
                Destroy(c9.gameObject);
            }

            if (flag)
            {
                c = ChessFigurePositions[a, b];   //5

                //1 vs 9
                if (x + 1 <= 4 && x - 1 >= 0 && y - 1 >= 0 && y + 1 <= 4)
                {
                    c5 = ChessFigurePositions[x + 1, y - 1];
                    c7 = ChessFigurePositions[x - 1, y + 1];
                    if (c5 != c && c7 != c)
                        if (c7 != null && c5 != null && c5.isRed == c7.isRed && c5.isRed != isRedTurn)
                        {
                            updateStatus(c5);
                            activeFigures.Remove(c5.gameObject);
                            Destroy(c5.gameObject);
                            updateStatus(c7);
                            activeFigures.Remove(c7.gameObject);
                            Destroy(c7.gameObject);
                        }
                }

                //2 vs 8
                if (y - 1 >= 0 && y + 1 <= 4)
                {
                    c1 = ChessFigurePositions[x, y + 1];
                    c2 = ChessFigurePositions[x, y - 1];
                    if (c1 != c && c2 != c)
                        if (c1 != null && c2 != null && c1.isRed == c2.isRed && c1.isRed != isRedTurn)
                        {
                            updateStatus(c1);
                            activeFigures.Remove(c1.gameObject);
                            Destroy(c1.gameObject);
                            updateStatus(c2);
                            activeFigures.Remove(c2.gameObject);
                            Destroy(c2.gameObject);
                        }
                }

                //3 vs 7
                if (x + 1 <= 4 && x - 1 >= 0 && y - 1 >= 0 && y + 1 <= 4)
                {
                    c4 = ChessFigurePositions[x + 1, y + 1];
                    c8 = ChessFigurePositions[x - 1, y - 1];
                    if (c4 != c && c8 != c)
                        if (c4 != null && c8 != null && c4.isRed == c8.isRed && c4.isRed != isRedTurn)
                        {
                            updateStatus(c4);
                            activeFigures.Remove(c4.gameObject);
                            Destroy(c4.gameObject);
                            updateStatus(c8);
                            activeFigures.Remove(c8.gameObject);
                            Destroy(c8.gameObject);
                        }
                }

                //6 vs 4
                if (x + 1 <= 4 && x - 1 >= 0)
                {
                    c3 = ChessFigurePositions[x + 1, y];
                    c6 = ChessFigurePositions[x - 1, y];
                    if (c3 != c && c6 != c)
                        if (c3 != null && c6 != null && c3.isRed == c6.isRed && c3.isRed != isRedTurn)
                        {
                            updateStatus(c3);
                            activeFigures.Remove(c3.gameObject);
                            Destroy(c3.gameObject);
                            updateStatus(c6);
                            activeFigures.Remove(c6.gameObject);
                            Destroy(c6.gameObject);
                        }
                }

            }
            else
            {
                //1 vs 9
                if (x + 1 <= 4 && x - 1 >= 0 && y - 1 >= 0 && y + 1 <= 4)
                {
                    c5 = ChessFigurePositions[x + 1, y - 1];
                    c7 = ChessFigurePositions[x - 1, y + 1];
                    if (c7 != null && c5 != null && c5.isRed == c7.isRed && c5.isRed != isRedTurn)
                    {
                        updateStatus(c5);
                        activeFigures.Remove(c5.gameObject);
                        Destroy(c5.gameObject);
                        updateStatus(c7);
                        activeFigures.Remove(c7.gameObject);
                        Destroy(c7.gameObject);
                    }
                }

                //2 vs 8
                if (y - 1 >= 0 && y + 1 <= 4)
                {
                    c1 = ChessFigurePositions[x, y + 1];
                    c2 = ChessFigurePositions[x, y - 1];
                    if (c1 != null && c2 != null && c1.isRed == c2.isRed && c1.isRed != isRedTurn)
                    {
                        updateStatus(c1);
                        activeFigures.Remove(c1.gameObject);
                        Destroy(c1.gameObject);
                        updateStatus(c2);
                        activeFigures.Remove(c2.gameObject);
                        Destroy(c2.gameObject);
                    }
                }

                //3 vs 7
                if (x + 1 <= 4 && x - 1 >= 0 && y - 1 >= 0 && y + 1 <= 4)
                {
                    c4 = ChessFigurePositions[x + 1, y + 1];
                    c8 = ChessFigurePositions[x - 1, y - 1];
                    if (c4 != null && c8 != null && c4.isRed == c8.isRed && c4.isRed != isRedTurn)
                    {
                        updateStatus(c4);
                        activeFigures.Remove(c4.gameObject);
                        Destroy(c4.gameObject);
                        updateStatus(c8);
                        activeFigures.Remove(c8.gameObject);
                        Destroy(c8.gameObject);
                    }
                }

                //6 vs 4
                if (x + 1 <= 4 && x - 1 >= 0)
                {
                    c3 = ChessFigurePositions[x + 1, y];
                    c6 = ChessFigurePositions[x - 1, y];
                    if (c3 != null && c6 != null && c3.isRed == c6.isRed && c3.isRed != isRedTurn)
                    {
                        updateStatus(c3);
                        activeFigures.Remove(c3.gameObject);
                        Destroy(c3.gameObject);
                        updateStatus(c6);
                        activeFigures.Remove(c6.gameObject);
                        Destroy(c6.gameObject);
                    }
                }
            }

            //Debug.Log("Red : " + red);
            //Debug.Log("Blue : " + blue);
            if (red == 0 || blue == 0)
            {
                EndGame();
                return;
            }

            ChessFigurePositions[selectedFigure.CurrentX, selectedFigure.CurrentY] = null;
            selectedFigure.transform.position = GetTileCenter(x, y);
            selectedFigure.SetPosition(x, y);
            ChessFigurePositions[x, y] = selectedFigure;
            isRedTurn = !isRedTurn;
        }

        BoardHighlighting.Instance.HideHighlights();
        selectedFigure = null;
    }

    private void DrawChessBoard()
    {
        Vector3 widthLine = Vector3.right * 5;
        Vector3 heightLine = Vector3.forward * 5;

        // Draw Chessboard
        for (int i = 0; i <= 5; i++)
        {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j <= 5; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        // Draw Selection
        if (selectionX >= 0 && selectionY >= 0)
        {
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * selectionX,
                Vector3.forward * (selectionY + 1) + Vector3.right * (selectionX + 1));
            Debug.DrawLine(Vector3.forward * selectionY + Vector3.right * (selectionX + 1),
               Vector3.forward * (selectionY + 1) + Vector3.right * selectionX);
        }
    }

    private void UpdateSelection()
    {
        if (!Camera.main) return;

        RaycastHit hit;
        float raycastDistance = 25.0f;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, raycastDistance, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void SpawnChessFigure(int index, int x, int y)
    {
        GameObject go = Instantiate(chessFigures[index], GetTileCenter(x, y), chessFigures[index].transform.rotation) as GameObject;
        go.transform.SetParent(transform);
        ChessFigurePositions[x, y] = go.GetComponent<ChessFigure>();
        ChessFigurePositions[x, y].SetPosition(x, y);
        activeFigures.Add(go);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        origin.y = 0.5f;
        return origin;
    }
    
    private void SpawnAllChessFigures()
    {
        // Red  
        SpawnChessFigure(0, 0, 0);
        SpawnChessFigure(0, 0, 1);
        SpawnChessFigure(0, 1, 0);
        SpawnChessFigure(0, 2, 0);
        SpawnChessFigure(0, 3, 0);
        SpawnChessFigure(0, 4, 0);
        SpawnChessFigure(0, 4, 1);
        SpawnChessFigure(0, 4, 2);

        // Blue
        SpawnChessFigure(1, 4, 4);
        SpawnChessFigure(1, 4, 3);
        SpawnChessFigure(1, 3, 4);
        SpawnChessFigure(1, 2, 4);
        SpawnChessFigure(1, 1, 4);
        SpawnChessFigure(1, 0, 4);
        SpawnChessFigure(1, 0, 3);
        SpawnChessFigure(1, 0, 2);
    }

    private void EndGame()
    {
        if (isRedTurn)
        {
            Winner_Panel.SetActive(true);
            Winner_txt.text = "Red team Wins!";
            Debug.Log("Red team won!");
        }
        else
        {
            Winner_Panel.SetActive(true);
            Winner_txt.text = "Blue team Wins!"; 
            Debug.Log("Blue team won!");
        }

        foreach (GameObject go in activeFigures)
            Destroy(go);

        isRedTurn = true;
        //red = 8;
        //blue = 8;
        //isRedTurn = true;
        //BoardHighlighting.Instance.HideHighlights();
        //SpawnAllChessFigures();
        //Debug.Log("Red turn: " + isRedTurn);
    }

    public void updateStatus(ChessFigure c)
    {
        if (c.isRed)
            red--;
        else
            blue--;
    }

    public List<GameObject> GetAllActiveFigures()
    {
        List<GameObject> activeFiguresR = new List<GameObject>();
        foreach (GameObject go in activeFigures)
        {
            float a = go.transform.position.x - TILE_OFFSET;
            int x = (int)a;
            float b = go.transform.position.z - TILE_OFFSET;
            int y = (int)b;
            ChessFigure c = ChessFigurePositions[x, y];
            if (!c.isRed) activeFiguresR.Add(go);
        }
        return activeFiguresR;
    }
}

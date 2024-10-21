using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using System;

public class GameOfLifeOneDimension : MonoBehaviour
{
    public OneDCell[,] theGrid;
    public Tilemap tilemap;
    public Tilemap userTilemap;
    public Tile drawedTile;
    public int rows = 31;
    public int columns = 16;
    private bool simulationIsRunning;
    public Tile deadTile;
    /// //////////
    public Button startGameOfLife, b_steppedMode,b_runAllMode, b_stopMode;
    public TMP_InputField xRowInput, yColumnsInput, ruleNumber, generationsNumber;
    public bool bRandomActivaded, bSteppedModeActivated;
    public Toggle randomMode, steppedMode;
    private int[,] cellsGrid;
    private int rule;
    private int ruleToBinary;
    private int currentXSize, currentYSize;

    public int numGenerations = 20;

    //Variables para los patrones (las partes de arriba que no cambian nunca)
    //lista de patrones (pero tengo que ver si hay alguna manera mas optima)
    List<(List<char>, bool)> patternsList = new List<(List<char>, bool)>() {
        (new List<char>() {'1','1','1' },false),
        (new List < char >() { '1', '1', '0' }, false),
        (new List < char >() { '1', '0', '1' }, false),
        (new List < char >() { '1', '0', '0' }, false),
        (new List < char >() { '0', '1', '1' }, false),
        (new List < char >() { '0', '1', '0' }, false),
        (new List < char >() { '0', '0', '1' }, false),
        (new List < char >() { '0', '0', '0' }, false),
    };

void Start()
    {
        theGrid = new OneDCell[rows, columns];
        //initializeFirstRow();
        numGenerations = 10;
        xRowInput.onEndEdit.AddListener(selectRows);
        yColumnsInput.onEndEdit.AddListener(selectColumns);
        ruleNumber.onEndEdit.AddListener(selectRule);
        generationsNumber.onEndEdit.AddListener(selectGenerations);      
    }
    private void Update() {
        
    }
    public void selectRows(string rowsTxt) {
        rows = int.Parse(rowsTxt);
        Debug.Log(rows);
    }

    public void selectColumns(string columnsTxt) {
        columns = int.Parse(columnsTxt);
        theGrid = new OneDCell[rows,columns];
        Debug.Log("columssssssss:  " + columns);
        createGrid();
        Debug.Log(columns);
    }

    public void selectRule(string ruleTxt) {
        rule = int.Parse(ruleTxt);
        ruleNumberToBinary();
        Debug.Log("ruleeeeeeeeeeeeee:  " + rule);
    }

    public void selectGenerations(string generationNumTxt) {
        numGenerations = int.Parse(generationNumTxt);
        Debug.Log(columns);
    }
    //aqui tengo que empezar la logica para los patrones(los que nunca cambian )
    int patterns(List<int> neighsList) {
        //me gustaria meter un foreach donde tome los tres cell actuales y cheque los vecinos
        //y entonces cada caso  debe tener su propio 0 o 1 lo que hara (descartado por el momento)
        //////////////////////////////////////////////////////////////////
        //bool matchFound;
        Debug.LogWarning("Verificando vecinos: " + string.Join(", ", neighsList));
        for (int i = 0;i < patternsList.Count;i++) {
            bool matchFound = true;
            for(int n=0; n< neighsList.Count;n++) {
                if (patternsList[i].Item1[n] != neighsList[n].ToString()[0]){
                    matchFound = false;
                    break;
                }

            }
            //aaqui deberiaa devolver mi indice del patron con el cual hubo match
            if (matchFound) {            
                return i;
            }
        }
        Debug.LogWarning("No se encontró ningún patrón");
        return -1;
    }

    void initializeFirstRow() {
        if (theGrid == null || theGrid.Length == 0) {
            Debug.LogError("theGrid no esta bieeeeeeeeeen");
            return;
        }
        int midGridColum = columns / 2;
        theGrid[0,midGridColum].bIsAlive = true;
        Vector3Int tilePos = new Vector3Int(midGridColum, 0, 0);
        tilemap.SetTile(tilePos, drawedTile);
        Debug.LogWarning("Aqui va first roooooooooooooowwwwwwwwww");
        Debug.LogWarning("tile possssss: " + tilePos+ "esta vivo");
    }

    //function para checar vecinos de tres en tres(left, current, right)
    List<int> checkNeighsOneD(int x, int y) {
        List<int> neighsList = new List<int>();
        int[,] neighborsCoords = new int[,] {
        {x - 1, y}, {x, y}, {x + 1, y}
        };
        for(int i = 0;i < neighborsCoords.GetLength(0); i++) {
            int neighx = neighborsCoords[i, 0];
            int neighy = neighborsCoords[i, 1];
            if(neighx >= 0 && neighx < rows && neighy >= 0 && neighy < columns) {
                if(theGrid[neighx, neighy].bIsAlive) {
                    neighsList.Add(1);
                } else {
                    neighsList.Add(0);
                }
            } else {
                neighsList.Add(0);  
            }
        }
        return neighsList;
    }

    public void runGameOfLife() {
        int currentIteration = 0;
        Debug.LogWarning("iniciaaaaaaaaa: " + numGenerations);
        while (currentIteration < numGenerations) {
            OneDCell[,] tempGrid = new OneDCell[rows, columns];
            for (int i = 0;i < rows; i++) {
                for (int c = 0; c < columns; c++) {
                    tempGrid[i, c] = new OneDCell();

                }
            }
            for (int i = 0; i < rows; i++) {
                for (int c = 0; c < columns; c++) {
                    if (currentIteration == 0) {
                        tempGrid[i, c].bIsAlive = theGrid[i, c].bIsAlive;
                    } else {
                        List<int> getNeighsList = checkNeighsOneD(i, c);
                        int tempIndexPattern = patterns(getNeighsList);
                        if (tempIndexPattern != -1) {
                            tempGrid[i, c].bIsAlive = patternsList[tempIndexPattern].Item2;
                        } else {
                            //tempGrid[i, c].bIsAlive = false;
                            tempGrid[i, c].bIsAlive = theGrid[i, c].bIsAlive;
                        }
                    }
                }
            }
                theGrid = tempGrid;
                updateVisualGrid(currentIteration);
                currentIteration++;
        }
        Debug.LogWarning("Finalización del ciclo de generaciones");
    }
        IEnumerator generationsInterval() {
            while (simulationIsRunning) {
                yield return new WaitForSeconds(5f);
                //gameOflifePast();
                runGameOfLife();
                //updateVisualGrid();
            }
        }

    public void startGameOfLifeOneD() {
        simulationIsRunning = true;
        StartCoroutine(generationsInterval());

    }

    

    private void updateVisualGrid(int iterationNumber) {
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c <columns; c++) {
                Vector3Int currentGridPos = new Vector3Int(c,i,0);
                if (theGrid[i,c].bIsAlive) {
                    Debug.Log("tile vivoooooooo");
                    tilemap.SetTile(currentGridPos, drawedTile);
                } else {
                    Debug.Log("tile muertooooooooo");
                    tilemap.SetTile(currentGridPos, deadTile);
                }
            }
        }
    }

    //aqui solo creo toda la grid por lo que todas deben estar muertas
    void createGrid() {
        //for(int i = 0;i < currentXSize; i++) {
        theGrid = new OneDCell[rows, columns];
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                theGrid[i, c] = new OneDCell(false);
            }
        }
        initializeFirstRow();
        //}
    }
    public void ruleNumberToBinary() {
        //preguntar como hacer esto de lo binario
        int numberToConvert = int.Parse(ruleNumber.text);
        string binaryString = Convert.ToString(numberToConvert,2).PadLeft(8,'0');
        Debug.Log("Número en binario: " + binaryString);
        char[] binaryChars = binaryString.ToCharArray();
        for(int i = 0;i < binaryChars.Length ; i++) {
            patternsList[i] = (patternsList[i].Item1, binaryChars[i] =='1');
        }
    }

    //funcion para crear todo de una 
    void runFullGameOfLife() {
        for(int i= 0;i < currentXSize; i++) {

        }
    }

    IEnumerator steppedModeDelay() {
        for(int i = 0;i < currentYSize ; i++) {
            yield return new WaitForSeconds(0.4f);
        }
    }
}

    //reference to inputsField
    //https://youtu.be/guelZvubWFY
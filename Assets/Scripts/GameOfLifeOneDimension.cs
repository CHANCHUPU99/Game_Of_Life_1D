using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;
using System;

public class GameOfLifeOneDimension : MonoBehaviour
{
    public OneDCell[] theGrid;
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
    private int ruleToBinari;
    private int currentXSize, currentYSize;

    private int numGenerations;

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
        xRowInput.onEndEdit.AddListener(selectRows);
        yColumnsInput.onEndEdit.AddListener(selectColumns);
        ruleNumber.onEndEdit.AddListener(selectRule);
        generationsNumber.onEndEdit.AddListener(selectGenerations);
        //startGameOfLife.onClick.AddListener(runGameOfLife);
        //theGrid = new OneDCell[columns];
        ////for (int i = 0; i < rows; i++) {
        //    for (int c = 0; c < columns; c++) {
        //        theGrid[c] = new OneDCell(false);
        //        Debug.LogError("theGrid no esta bien");
        //    }
        //    initializeFirstRow();
        ////}
    }

    private void Update() {
        
    }
    public void selectRows(string rowsTxt) {
        rows = int.Parse(rowsTxt);
        Debug.Log(rows);
    }

    public void selectColumns(string columnsTxt) {
        columns = int.Parse(columnsTxt);
        theGrid = new OneDCell[columns];
        Debug.Log("columssssssss:  " + columns);
        createGrid();
        Debug.Log(columns);
    }

    public void selectRule(string ruleTxt) {
        rule = int.Parse(ruleTxt);
        ruleNumberToBinary();
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
       for(int i = 0;i < patternsList.Count;i++) {
            bool matchFound = true;
            for(int n=0; n< neighsList.Count;n++) {
                if (patternsList[i].Item1[n] != neighsList[n].ToString()[0]){
                    matchFound = false;
                    return 0;
                }

            }
            //aaqui deberiaa devolver mi indice del patron con el cual hubo match
            if (matchFound) {
                return i;
            }
        }
        return -1;
    }

    void initializeFirstRow() {
        if (theGrid == null || theGrid.Length == 0) {
            Debug.LogError("theGrid no esta bieeeeeeeeeen");
            return;
        }
        int midGridColum = columns / 2;
        theGrid[midGridColum].bIsAlive = true;
        Vector3Int tilePos = new Vector3Int(midGridColum, 0, 0);
        tilemap.SetTile(tilePos, drawedTile);
        Debug.Log("tile possssss: " + tilePos);
    }

    //function para checar vecinos de tres en tres(left, current, right)
    List<int> checkNeighsOneD(int currentCellPos) {
        List<int> neighsList = new List<int>();
        if (currentCellPos < 0 || currentCellPos >= theGrid.Length) {
            Debug.LogError("currentCellPos fuera del grid: " + currentCellPos);
            return new List<int>(); 
        }
        if (currentCellPos == 0) {
            if (theGrid[theGrid.Length - 1].bIsAlive) {
                neighsList.Add(1);
            } else {
                neighsList.Add(0);
            }
        } else {
            if (theGrid[currentCellPos - 1].bIsAlive) {
                neighsList.Add(1);
            } else {
                neighsList.Add(0);
            }
        }
        if (theGrid[currentCellPos].bIsAlive) {
            neighsList.Add(1);
        } else {
            neighsList.Add(0);
        }
        if (currentCellPos == theGrid.Length - 1) {
            if (theGrid[0].bIsAlive) {
                neighsList.Add(1);
            } else {
                neighsList.Add(0);
            }
        } else {
            if (theGrid[currentCellPos + 1].bIsAlive) {
                neighsList.Add(1);
            } else {
                neighsList.Add(0);
            }
        }
        return neighsList;
    }

    public void runGameOfLife() {
        int currentIteration = 0;
        Debug.LogWarning("iniciaaaaaaaaa");
            OneDCell[] tempGrid = new OneDCell[columns];
            for(int i = 0; i < columns; i++) {
                tempGrid[i] = new OneDCell();
            }
        while (currentIteration < numGenerations) {
            for(int i = 0;i < columns;i++) {
                if(currentIteration ==  0) {
                    tempGrid[i].bIsAlive = theGrid[i].bIsAlive;
                } else {
                    List<int> getNeighsList = checkNeighsOneD(i);
                    int tempIndexPatter = patterns(getNeighsList);
                    if(tempIndexPatter != -1) {
                        tempGrid[i].bIsAlive = patternsList[tempIndexPatter].Item2;
                } else {
                        tempGrid[i].bIsAlive = theGrid[i].bIsAlive;
                    }
                }
            }
            
            theGrid = tempGrid;
            updateVisualGrid(currentIteration);
            Debug.Log("itttt " + currentIteration);
            currentIteration++;
        }


    }
        IEnumerator generationsInterval() {
            while (simulationIsRunning) {
                yield return new WaitForSeconds(2f);
                //gameOflifePast();
                runGameOfLife();
                //updateVisualGrid();
            }
        }

    public void startGameOfLifeOneD() {
        simulationIsRunning = true;
        StartCoroutine(generationsInterval());

    }


    private void updateGrid(Vector3Int tilePos) {
        int currentPos = tilePos.x;
        TileBase currentTilemap = tilemap.GetTile(tilePos);
        if (currentTilemap == null) {
            tilemap.SetTile(tilePos, drawedTile);
            userTilemap.SetTile(tilePos, drawedTile);
            theGrid[tilePos.x].bIsAlive = true;
            Debug.Log("viveeeeeee");
        } else {
            tilemap.SetTile(tilePos, null);
            userTilemap.SetTile(tilePos, null);
            theGrid[tilePos.x].bIsAlive = false;
            Debug.Log("MUEREEEEEE");
        }
    }

    private void updateVisualGrid(int iterationNumber) {
        //for (int i = 0; i < rows; i++) {
            for (int c = 0; c < theGrid.Length; c++) {
                Vector3Int currentGridPos = new Vector3Int(c,iterationNumber,0);
                if (theGrid[c].bIsAlive) {
                    Debug.Log($"Generación {iterationNumber}: Celda {c} viva, dibujando en {currentGridPos}");
                    //Debug.Log("tile vivoooooooo");
                    tilemap.SetTile(currentGridPos, drawedTile);
                } else {
                    Debug.Log($"Generación {iterationNumber}: Celda {c} muertaaaaaaa, dibujando en {currentGridPos}");
                    //Debug.Log("tile muertooooooooo");
                    tilemap.SetTile(currentGridPos, deadTile);
                }
            }
        //}
    }

    //aqui solo creo toda la grid por lo que todas deben estar muertas
    void createGrid() {
        //for(int i = 0;i < currentXSize; i++) {
        theGrid = new OneDCell[columns];
            for(int c = 0;c < columns; c++) {
            theGrid[c] = new OneDCell(false);
            Debug.Log($"Creando celda en columna {c}, viva: {theGrid[c].bIsAlive}");
            //updateVisualGrid(c);
        }
            initializeFirstRow();
        //}
    }
    public void ruleNumberToBinary() {
        //preguntar como hacer esto de lo binario
        int numberToConvert = int.Parse(ruleNumber.text);
        string binaryString = Convert.ToString(numberToConvert,2).PadLeft(8,'0');
        char[] binaryChars = binaryString.ToCharArray();

        for(int i = 0;i < binaryChars.Length ; i++) {
            patternsList[i] = (patternsList[i].Item1, binaryChars[i] =='1');
            Debug.Log($"Patrón {patternsList[i].Item1} asignado a: {(binaryChars[i] == '1' ? "Vivo" : "Muerto")}");
        }
    }

    //funcion para crear todo de una 
    void runFullGameOfLife() {
        for(int i= 0;i < currentXSize; i++) {

        }
    }
    //funcion para randomizar el grid(falta preguntar)
    /*void randomGrid() {
        for(int i = 0;i < columns; i++) {
            bool bIsAlive = Random.Range(0, 2) == 1;
            theGrid[i].bIsAlive = bIsAlive;
            if (bIsAlive) {
                tilemap.SetTile(new Vector3Int(i, 0, 0), drawedTile);
            }
        }
    }*/

    //funcion para la corrutina del steppedMode
    IEnumerator steppedModeDelay() {
        for(int i = 0;i < currentYSize ; i++) {
            yield return new WaitForSeconds(0.4f);
        }
    }
    //reference to inputsField
    //https://youtu.be/guelZvubWFY
}

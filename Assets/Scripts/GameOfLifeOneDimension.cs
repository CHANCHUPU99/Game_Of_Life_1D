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
        startGameOfLife.onClick.AddListener(runGameOfLife);
        theGrid = new OneDCell[columns];
        //for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                theGrid[c] = new OneDCell(false);
            }
        //}
    }
    
    public void selectRows(string rowsTxt) {
        rows = int.Parse(rowsTxt);
        Debug.Log(rows);
    }

    public void selectColumns(string columnsTxt) {
        columns = int.Parse(columnsTxt);
        Debug.Log(columns);
    }

    public void selectRule(string ruleTxt) {
        rule = int.Parse(ruleTxt);
    }
    //aqui tengo que empezar la logica para los patrones(los que nunca cambian )
    int patterns(List<int> neighsList) {
        //me gustaria meter un foreach donde tome los tres cell actuales y cheque los vecinos
        //y entonces cada caso  debe tener su propio 0 o 1 lo que hara (descartado por el momento)
        //////////////////////////////////////////////////////////////////
        bool matchFound;
       for(int i = 0;i < patternsList.Count;i++) {
            matchFound = true;
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
        return 0;
    }

    //function para checar vecinos de tres en tres(left, current, right)
    List<int> checkNeighsOneD(int currentCellPos) {
        List<int> neighsList = new List<int>();
        if (currentCellPos > 0 && theGrid[currentCellPos - 1].bIsAlive) {
            //vivo a la izq
            neighsList.Add(1);
        } else {
            //no hay vivo a la izq
            neighsList.Add(0);
        }

        //curreeeeeeeeeeent
        if (theGrid[currentCellPos].bIsAlive) {
            neighsList.Add(1);
        } else {
            neighsList.Add(0);
        }

        //riiight
        if (currentCellPos < theGrid.Length - 1 &&theGrid[currentCellPos++].bIsAlive) {
            neighsList.Add(1);
        } else {
            neighsList.Add(0);
        }
        return neighsList;
    }

    public void runGameOfLife() {
        /*currentXSize = int.Parse(xRowInput.text);
        currentYSize = int.Parse(yColumnsInput.text);
        bRandomActivaded = randomMode.isOn;
        bSteppedModeActivated = steppedMode.isOn;
        cellsGrid = new int[currentXSize, currentYSize];
        if(bRandomActivaded) {
            randomGrid();
            //aqui tendria que poner mi funcion de randomizar 
            
        } else {
            createGrid();
        }
        // tengo que hacer lo contrario , por si se activa steppedMode, para la corrutina
        if(bSteppedModeActivated) {
            StartCoroutine(steppedModeDelay());
        }*/

        int currentIteration = 0;
        ruleNumberToBinary();
        OneDCell[] tempGrid = new OneDCell[columns];
        for(int i = 0;i < columns; i++) {
            tempGrid[i] = new OneDCell();
        }
        for(int i = 0; i < columns; i++) {
            List<int> getNeighsList = checkNeighsOneD(i);
            int tempIndexPatter = patterns(getNeighsList);
            //aqui tendria que 
            if(tempIndexPatter != -1) {
                tempGrid[i].bIsAlive = patternsList[tempIndexPatter].Item2;
            }
        }
        theGrid = tempGrid;
        updateVisualGrid(currentIteration);

       

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
            for (int c = 0; c < columns; c++) {
                Vector3Int currentGridPos = new Vector3Int(c,iterationNumber,0);
                if (theGrid[c].bIsAlive) {
                    Debug.Log("deberia ser 1");
                    tilemap.SetTile(currentGridPos, drawedTile);
                } else {
                    tilemap.SetTile(currentGridPos, null);
                }
            }
        //}
    }

    //aqui solo creo toda la grid por lo que todas deben estar muertas
    void createGrid() {
        for(int i = 0;i < currentXSize; i++) {
            for(int c = 0;c < currentYSize; c++) {
                cellsGrid[i, c] = 0;   

            }
        }
    }
    public void ruleNumberToBinary() {
        //preguntar como hacer esto de lo binario
        int numberToConvert = int.Parse(ruleNumber.text);
        string binaryString = Convert.ToString(numberToConvert,2).PadLeft(8,'0');
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
    //funcion para randomizar el grid(falta preguntar)
    void randomGrid() {
        for(int i = 0;i < currentXSize;i++) {
            for(int c = 0; c < currentYSize;c++) {
               // cellsGrid[i, c] = Random.Range(0,1);
            }
        }
    }

    //funcion para la corrutina del steppedMode
    IEnumerator steppedModeDelay() {
        for(int i = 0;i < currentYSize ; i++) {
            yield return new WaitForSeconds(0.4f);
        }
    }
    //reference to inputsField
    //https://youtu.be/guelZvubWFY
}

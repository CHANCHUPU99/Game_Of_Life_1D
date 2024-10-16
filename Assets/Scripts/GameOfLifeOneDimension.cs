using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;

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
    private int ruleToBinari;
    private int currentXSize, currentYSize;

    //Variables para los patrones (las partes de arriba que no cambian nunca)
    //lista de patrones (pero tengo que ver si hay alguna manera mas optima)
    List<List<int>> patternsList = new List<List<int>>() {
        new List<int>() {1,1,1 },
        new List<int>() { 1, 1, 0 },
        new List<int>() { 1, 0, 1 },
        new List<int>() { 1, 0, 0 },
        new List<int>() { 0, 1, 1 },
        new List<int>() { 0, 1, 0 },
        new List<int>() { 0, 0, 1 },
        new List<int>() { 0, 0, 0 },
    };

void Start()
    {
        /*theGrid = new OneDCell[rows, columns];
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                theGrid[i, c] = new OneDCell(false);
            }
        }
        startGameOfLife.onClick.AddListener(runGameOfLife);*/
    }

    //aqui tengo que empezar la logica para los patrones(los que nunca cambian )
    int petterns(List<int> neighsList) {
        //me gustaria meter un foreach donde tome los tres cell actuales y cheque los vecinos
        //y entonces cada caso  debe tener su propio 0 o 1 lo que hara (descartado por el momento)
        //////////////////////////////////////////////////////////////////
        bool matchFound;
       for(int i = 0;i < patternsList.Count;i++) {
            matchFound = true;
            for(int n=0; n< neighsList.Count;n++) {
                if (patternsList[i][n] != neighsList[n]){
                    matchFound = false;
                    return 0;
                }

            }
            //aaqui deberiaa devolver mi indice del patron con el cual hubo match
            return i;
        }
        return 0;
    }

    //function para checar vecinos de tres en tres(left, current, right)
    List<int> checkNeighsOneD(int currentCellPos) {
        List<int> neighsList = new List<int>();
        if (theGrid[currentCellPos - 1].bIsAlive) {
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
        if (theGrid[currentCellPos++].bIsAlive) {
            neighsList.Add(1);
        } else {
            neighsList.Add(0);
        }
        return neighsList;
    }



    void runGameOfLife() {
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

        OneDCell[] tempGrid = new OneDCell[columns];
        for(int i = 0;i < columns; i++) {
            tempGrid[i] = new OneDCell();
        }
        for(int i = 0; i < columns; i++) {
            List<int> getNeighsList = checkNeighsOneD(i);
            //aqui tendria que 

        }
        theGrid = tempGrid;
        //updateVisualGrid();

    }

    //aqui solo creo toda la grid por lo que todas deben estar muertas
    void createGrid() {
        for(int i = 0;i < currentXSize; i++) {
            for(int c = 0;c < currentYSize; c++) {
                cellsGrid[i, c] = 0;   

            }
        }
    }
    public void gameOfLifeRules() {
        //preguntar como hacer esto de lo binario
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
                cellsGrid[i, c] = Random.Range(0,1);
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

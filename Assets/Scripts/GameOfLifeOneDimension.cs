using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Tilemaps;

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
    private int ruleToBinari;
    private int currentXSize, currentYSize;

    //Variables para los patrones (las partes de arriba que no cambian nunca)


    void Start()
    {
        theGrid = new OneDCell[rows, columns];
        for (int i = 0; i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                theGrid[i, c] = new OneDCell(false);
            }
        }
        startGameOfLife.onClick.AddListener(runGameOfLife);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //aqui tengo que empezar la logica para los patrones(los que nunca cambian )
    void petterns() {
        //me gustaria meter un foreach donde tome los tres cell actuales y cheque los vecinos
        //y entonces cada caso  debe tener su propio 0 o 1 lo que hara 
    }
    //aqui solo creo toda la grid por lo que todas deben estar muertas
    void createGrid() {
        for(int i = 0;i < currentXSize; i++) {
            for(int c = 0;c < currentYSize; c++) {
                cellsGrid[i, c] = 0;   

                }
            }
        }

    void runGameOfLife() {
        currentXSize = int.Parse(xRowInput.text);
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

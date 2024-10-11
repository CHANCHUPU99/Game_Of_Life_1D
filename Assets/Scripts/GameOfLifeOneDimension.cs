using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOfLifeOneDimension : MonoBehaviour
{
    public Button startGameOfLife;
    public InputField xRowInput, yColumnsInput, ruleNumber;
    public bool bRandomActivaded, bSteppedModeActivated;
    public Toggle randomMode, steppedMode;
    private int[,] cellsGrid;
    private int ruleToBinari;
    private int currentXSize, currentYSize;


    void Start()
    {
        startGameOfLife.onClick.AddListener(runGameOfLife);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //aqui solo creo toda la grid por lo que todas deberian estar muertas
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

    }

    //funcion para crear todo de una 
    void runFullGameOfLife() {
        for(int i= 0;i < currentXSize; i++) {

        }
    }
    //funcion para randomizar
    void randomGrid() {
        for(int i = 0;i < currentXSize;i++) {
            for(int c = 0;c < currentYSize;c++) {
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class OtroPerroTest : MonoBehaviour
{
    public Tilemap tilemap;
    public Tile aliveTile;
    public Tile deadTile;
    public Button startBU; 
    public TMP_InputField rowsInput, columnsInput, ruleInput;
    public int rows;
    public int columns;
    public int rule; 
    private int[,] thegrid;
    private int[,] newGrid;

    void Start() {
        rowsInput.onEndEdit.AddListener(setRows);
        columnsInput.onEndEdit.AddListener(setColumns);
        ruleInput.onEndEdit.AddListener(setRule);
        startBU.onClick.AddListener(runGameOfLIfeeeeee);
        //initializeGrid();
        //applyRule();
        //updateVisualGrid();
    }
    public void runGameOfLIfeeeeee() {
        initializeGrid();
        applyRule();
        updateVisualGrid();
    }
    IEnumerator generationsInterval() {
        //gameOflifePast();
        for(int i = 0; i < rows; i++) {
            applyRule();
            updateVisualGrid();
            yield return new WaitForSeconds(2f);

        }
            //updateVisualGrid();
    }
    //buttons functionssss
    public void startGameOfLifeOneD() {
        //simulationIsRunning = true;
        StartCoroutine(generationsInterval());

    }

    void setRows(string rowsTxt) {
        rows = int.Parse(rowsTxt);
        initializeGrid();
    }

    void setColumns(string columnsTxt) {
        columns = int.Parse(columnsTxt);
        initializeGrid();
    }

    void setRule(string ruleTxt) {
        rule = int.Parse(ruleTxt);
    }

    void initializeGrid() {
        thegrid = new int[rows, columns];
        newGrid = new int[rows, columns];
        for (int i = 0; i < rows; i++) {
            for (int c = 0;c < columns; c++) {
                thegrid[i, c] = 0; 
            }
        }
        thegrid[0, columns / 2] = 1; 
    }

    void applyRule() {
        string binaryRule = Convert.ToString(rule, 2).PadLeft(8, '0');
        for(int i = 1;i < rows; i++) {
            for (int c = 0; c < columns; c++) {
                int leftNeigh, current, rightNeigh;
                if (c > 0) {
                    leftNeigh = thegrid[i - 1, c - 1];
                } else {                   
                    leftNeigh = 0;
                }
                current = thegrid[i - 1, c];
                if (c < columns - 1) {
                    rightNeigh = thegrid[i - 1, c + 1];
                } else {
                    rightNeigh = 0;
                }
                int patteeeern = (leftNeigh * 4) + (current * 2) + rightNeigh;
                if(binaryRule[7 - patteeeern] == '1') {
                    newGrid[i, c] = 1; 
                }else {
                    newGrid[i, c] = 0; 
                }
            }
        }
        //Array.Copy(newGrid, thegrid, newGrid.Length);
    }

    void updateVisualGrid() {
        //tilemap.ClearAllTiles();
        for(int i = 0;i < rows; i++) {
            for(int c = 0; c < columns; c++) {
                Vector3Int position = new Vector3Int(c, -i, 0);
                if (newGrid[i,c] == 1) {
                    tilemap.SetTile(position, aliveTile);
                } else {
                    tilemap.SetTile(position, deadTile);
                }
            }
        }
        Array.Copy(newGrid, thegrid,newGrid.Length);
    }
}

//reference to inputsField
//https://youtu.be/guelZvubWFY

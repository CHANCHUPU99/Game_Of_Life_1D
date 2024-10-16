using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OneDCell 
{
    public bool bIsAlive;

    public OneDCell() {
        bIsAlive = true;
    }

    public OneDCell(bool isAlive) {
        this.bIsAlive = isAlive;
    }

}

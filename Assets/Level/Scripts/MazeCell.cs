using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public bool Wall1, Wall2, Wall3, Wall4;

    void Update() {
        transform.GetChild(0).gameObject.SetActive(Wall1);
        transform.GetChild(1).gameObject.SetActive(Wall2);
        transform.GetChild(2).gameObject.SetActive(Wall3);
        transform.GetChild(3).gameObject.SetActive(Wall4);
    }
}

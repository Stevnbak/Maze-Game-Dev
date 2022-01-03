using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTrap : MonoBehaviour
{
    public MazeCell trapCell;
    public float wallNumber;
    public float time;
    public bool isTriggered;
    float t;

    private void Update()
    {
        if (isTriggered)
        {
            t += Time.deltaTime;
            if (t >= time) close();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isTriggered = true;
            t = 0;
        }
    }

    void close()
    {
        Debug.Log("Closed entrance");
        if(wallNumber == 1) trapCell.Wall1 = true;
        if(wallNumber == 2) trapCell.Wall2 = true;
        if(wallNumber == 3) trapCell.Wall3 = true;
        if(wallNumber == 4) trapCell.Wall4 = true;
        Destroy(gameObject);
    }
}

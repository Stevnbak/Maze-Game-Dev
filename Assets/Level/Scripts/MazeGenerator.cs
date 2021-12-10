using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public Transform parent;
    public int size;
	public GameObject cellPrefab;
	private GameObject[,] cells;

	void Start()
    {

    }
	public void GenerateMaze()
	{
		size = 20;

		cells = new GameObject[size, size];
		int x = Random.Range(0, size);
		int z = Random.Range(0, size);
		GenerateRoute(x, z);
	}


	public void GenerateRoute(int startX, int startZ) {
		int x = startX;
		int z = startZ;
		CreateCell(x, z, true, true, true, true);
		while (parent.childCount < size * size + 1) {
			//The old tiles X coordinates
			int oldX = x;
			int oldZ = z;

			//Choose a random direction and move here
			int r = Random.Range(1,5);
			if(r == 1) x += 1;
			if(r == 2) x -= 1;
			if(r == 3) z += 1;
			if(r == 4) z -= 1;
			//Make sure maze doesn't go outside of the max size
			x = Mathf.Clamp(x, 0, size - 1);
			z = Mathf.Clamp(z, 0, size - 1);

			//Check if a cell already exists on this location
			if(GameObject.Find("Maze Cell (" + x + ", " + z + ")") == null) {
				//Decide which walls the cell should have
				bool w1 = false, w2 = false, w3 = false, w4 = false;
				if(GameObject.Find("Maze Cell (" + (x + 1) + ", " + (z) + ")") == null) w3 = true;
				if(GameObject.Find("Maze Cell (" + (x - 1) + ", " + (z) + ")") == null) w4 = true;
				if(GameObject.Find("Maze Cell (" + (x) + ", " + (z + 1) + ")") == null) w1 = true;
				if(GameObject.Find("Maze Cell (" + (x) + ", " + (z - 1) + ")") == null) w2 = true;
				
				if(r == 1) GameObject.Find("Maze Cell (" + oldX + ", " + oldZ + ")").GetComponent<MazeCell>().Wall3 = false;
				if(r == 2) GameObject.Find("Maze Cell (" + oldX + ", " + oldZ + ")").GetComponent<MazeCell>().Wall4 = false;
				if(r == 3) GameObject.Find("Maze Cell (" + oldX + ", " + oldZ + ")").GetComponent<MazeCell>().Wall1 = false;
				if(r == 4) GameObject.Find("Maze Cell (" + oldX + ", " + oldZ + ")").GetComponent<MazeCell>().Wall2 = false;
				//Create the cell
				CreateCell(x, z, w1, w2, w3, w4);
			}
		}
		
	}

	private void CreateCell (int x, int z, bool w1, bool w2, bool w3, bool w4) {
		GameObject newCell = Instantiate(cellPrefab, new Vector3((x * 5 - size * 2.5f) + 2.5f, 0f, (z * 5 - size * 2.5f) + 2.5f), Quaternion.identity, parent);
		cells[x, z] = newCell;
		newCell.name = "Maze Cell (" + x + ", " + z + ")";
		newCell.transform.parent = parent;

		newCell.GetComponent<MazeCell>().Wall1 = w1;
		newCell.GetComponent<MazeCell>().Wall2 = w2;
		newCell.GetComponent<MazeCell>().Wall3 = w3;
		newCell.GetComponent<MazeCell>().Wall4 = w4;
	}
	

    
}

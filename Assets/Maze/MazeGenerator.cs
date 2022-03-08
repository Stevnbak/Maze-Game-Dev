using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public Transform parent;
	public Camera mapCam;
    public int size;
	public GameObject cellPrefab;
	private GameObject[,] cells;

	public void GenerateMaze(int difficulty)
	{
		size = difficulty == 1 ? 10 : difficulty == 2 ? 16 : difficulty == 3 ? 32 : difficulty == 4 ? 32 : 0;
		mapCam.orthographicSize = size * 2.5f;

		if (size == 0)
        {
			Debug.Log("Difficulty was not one of the preset difficulties (1, 2, 3, 4) maze generation stopped");
			return;
        }
		cells = new GameObject[size, size];
		GenerateRoute();
		Debug.Log("Maze generated with size: " + size);
	}
	public void GenerateRoute()
	{
		//Create maze cells
		float x = 0;
		float z = 0;
		while (parent.childCount < size * size)
		{
			CreateCell(Mathf.RoundToInt(x), Mathf.RoundToInt(z), false, false, false, false);
			if (x == size - 1) cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall3 = true;
			if (x == 0) cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall4 = true;
			if (z == size - 1) cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall2 = true;
			if (z == 0) cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall1 = true;
			x += 1;
			if (x == size) { z += 1; x = 0; }
		}

		//Generate maze
		x = 0;
		z = 0;
		int divide = 2;
		int divideMax = 1;
		float oldX = size;
		float oldZ = size;
		for (int i = 0; i < Mathf.Log(size, 2) * 2 - 1; i++)
		{
			float zStart = 0;
			float xStart = 0;
			int row = 1;
			for (int t = 1; t <= Mathf.Pow(2, i); t++)
			{
				bool lineExists = false;
				//Debug.Log("divide: " + divide + "row: " + row + "maxDivide: " + divideMax);
				if (i % 2 == 0)
				{
					x = oldZ / 2 * (row * 2 - 1);
					float zEnd = oldZ * (t - ((row - 1) * Mathf.Pow(2, i) / (divide / 2)));
					oldX = zEnd - zStart;
					//Debug.Log((i + 1) + "." + t + ": x: " + x + " z start: " + zStart + " z end: " + zEnd + " row: " + row);
					//Debug.Log((i + 1) + "." + t + ": x: " + Mathf.RoundToInt(x) + " z start: " + Mathf.RoundToInt(zStart) + " z end: " + Mathf.RoundToInt(zEnd) + " row: " + row);
					for (z = Mathf.RoundToInt(zStart); z < Mathf.RoundToInt(zEnd); z++)
					{
						if (!cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall4 && !lineExists)
						{
							cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall4 = true;
						} else lineExists = true;
					}
					if(!lineExists)
					for (int v = 0; v < Mathf.Clamp((zEnd - zStart) / 4f, 1, size); v++)
					{
						int r = Random.Range(0, Mathf.RoundToInt(zEnd - zStart));
						if (x != 0) cells[Mathf.RoundToInt(x), Mathf.RoundToInt(zStart + r)].GetComponent<MazeCell>().Wall4 = false;
					}
					zStart = zEnd;
				}
				if (i % 2 == 1)
				{
					z = oldX / 2 * (row * 2 - 1);
					float xEnd = oldX / 2 * (t - ((row - 1) * Mathf.Pow(2, i) / (divide / 2)));
					oldZ = xEnd - xStart;
					//Debug.Log((i + 1) + "." + t + ": z: " + z + " x start: " + xStart + " x end: " + xEnd + " row: " + row);
					//Debug.Log((i + 1) + "." + t + ": z: " + Mathf.RoundToInt(z) + " x start: " + Mathf.RoundToInt(xStart) + " x end: " + Mathf.RoundToInt(xEnd) + " row: " + row);
					for (x = Mathf.RoundToInt(xStart); x < Mathf.RoundToInt(xEnd); x++)
					{
						if (!cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall1 && !lineExists)
						{
							cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall1 = true;
						}
						else lineExists = true;
					}
					if (!lineExists)
						for (int v = 0; v < Mathf.Clamp((xEnd - xStart) / 4f, 1, size); v++)
					{
						int r = Random.Range(0, Mathf.RoundToInt(xEnd - xStart));
						if (z != 0) cells[Mathf.RoundToInt(xStart + r), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall1 = false;
					}
					xStart = xEnd;
				}
				if (t / row >= (Mathf.Pow(2, i) / (divide / 2))) { 
					row += 1;
					xStart = 0;
					zStart = 0;
				}
			}
			if (i % 2 == 0) divideMax *= 2;
			if (i % 2 == 1) divide *= 2;
		}
	}

	private void CreateCell (int x, int z, bool w1, bool w2, bool w3, bool w4) {
		GameObject newCell = Instantiate(cellPrefab, new Vector3((x * 5 - size * 2.5f) + 2.5f, -0.5f, (z * 5 - size * 2.5f) + 2.5f), Quaternion.identity, parent);
		cells[x, z] = newCell;
		newCell.name = "Maze Cell (" + x + ", " + z + ")";
		newCell.transform.parent = parent;

		newCell.GetComponent<MazeCell>().x = x;
		newCell.GetComponent<MazeCell>().z = z;


		newCell.GetComponent<MazeCell>().Wall1 = w1;
		newCell.GetComponent<MazeCell>().Wall2 = w2;
		newCell.GetComponent<MazeCell>().Wall3 = w3;
		newCell.GetComponent<MazeCell>().Wall4 = w4;
	}
}

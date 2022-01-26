using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

	public Transform parent;
	public Camera cam;
	public int size;
	public GameObject cellPrefab;
	private GameObject[,] emptyCells;
	private GameObject[,] cells;
	void Start()
	{
		cells = new GameObject[size, size];
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
		GenerateMaze();
	}

	public void GenerateMaze()
	{
		cam.orthographicSize = size * 2.5f + 10;
		StartCoroutine(GenerateRoute());
	}

	public IEnumerator GenerateRoute()
	{
		float x = 0;
		float z = 0;

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
							if (x == size - 1) cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall3 = true;
							if (x == 0) cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall4 = true;
							if (z == size - 1) cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall2 = true;
							if (z == 0) cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall1 = true;
							cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall4 = true;
						}
						else lineExists = true;
					}
					if (!lineExists)
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
							if (x == size - 1) cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall3 = true;
							if (x == 0) cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall4 = true;
							if (z == size - 1) cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall2 = true;
							if (z == 0) cells[Mathf.RoundToInt(x), Mathf.RoundToInt(z)].GetComponent<MazeCell>().Wall1 = true;
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
				if (t / row >= (Mathf.Pow(2, i) / (divide / 2)))
				{
					row += 1;
					xStart = 0;
					zStart = 0;
				}
				yield return new WaitForSeconds(0.05f);
			}
			if (i % 2 == 0) divideMax *= 2;
			if (i % 2 == 1) divide *= 2;
		}
		for (int i = 0; i < parent.childCount; i++)
		{
			MazeCell cell = parent.GetChild(i).GetComponent<MazeCell>();
			cell.Wall1 = false;
			cell.Wall2 = false;
			cell.Wall3 = false;
			cell.Wall4 = false;
			if (cell.x == size - 1) cell.Wall3 = true;
			if (cell.x == 0) cell.Wall4 = true;
			if (cell.z == size - 1) cell.Wall2 = true;
			if (cell.z == 0) cell.Wall1 = true;
		}
		GenerateMaze();
	}

	private void CreateCell(int x, int z, bool w1, bool w2, bool w3, bool w4)
	{
		GameObject newCell = Instantiate(cellPrefab, new Vector3((x * 5 - size * 2.5f) + 2.5f, 0f, (z * 5 - size * 2.5f) + 2.5f), Quaternion.identity, parent);
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

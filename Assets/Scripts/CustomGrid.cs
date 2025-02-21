using UnityEngine;

public class CustomGrid
{
    private int width;
    private int height;
    private int cellSize;
    private Vector3 originPositon;
    private int[,] gridArray;

    public CustomGrid(int width, int height, int cellSize, Vector3 originPositon)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPositon = originPositon;
        gridArray = new int[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridArray.GetLength(1); z++)
            {
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x + 1, z), Color.white, 1000f);
                Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z + 1), Color.white, 1000f);

            }
        }
    }

    public int GetCellSize()
    {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z) * GetCellSize() + originPositon;
    }

    private int[] GetXY(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition - originPositon).x / GetCellSize());
        int z = Mathf.FloorToInt((worldPosition - originPositon).y / GetCellSize());

        return new int[] { x, z };
    }
}

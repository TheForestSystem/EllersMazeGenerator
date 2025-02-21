using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EllersMazeGenerator : MonoBehaviour
{
    [SerializeField] GridSpawner gridSpawner;

    [Range(0, 100)]
    [SerializeField] int R_WallSpawnPersentage = 0;

    [Range(0, 100)]
    [SerializeField] int B_WallSpawnPersentage = 0;

    [SerializeField] GameObject wall;

    [SerializeField] int wall_yvalue;

    private CustomGrid gridXZ;
    private int maxSetValue = 1;

    private void Start()
    {
        gridXZ = gridSpawner.grid;

        SpawnEllersMaze();
    }

    private void SpawnEllersMaze()
    {
        // Step (1) + (2):
        List <(int[], int)> firstRow = new List<(int[], int)> ();

        for (int x = 0; x < gridSpawner.width; x++)
        {
            int[] coords = new int[] { x, 0 };
            var cellTup = (coords, maxSetValue);

            firstRow.Add(cellTup);

            maxSetValue++;
        }

        PlaceWalls_SameDirection(firstRow, Vector3.right);

        // Maze Generation Loop:

        List<(int[], int)> row = firstRow;
        List<int[]> sameSetWalls = new List<int[]>();
        for (int z = 0; z < gridSpawner.height; z++)
        {
            // Step (5) Finalize Row:
            if (z == gridSpawner.height -1)
            {
                PlaceWalls_SameDirection(IncreaseAxisRow(1, row), Vector3.right);
                FinalRow(row, sameSetWalls);
                continue;
            }

            // Step (3), Vertical Walls & Joining:
            var joinedRow = JoinRowVerticalWalls(row, sameSetWalls);
            // WorldTextRow(joinedRow, Color.red, 1);

            // Step (4), Bottom Walls:
            BottomWalls(joinedRow, out List<int[]> bWallLocation);

            // Step (5+2), Add New Row:
            row = IncreaseAxisRow(1, EmptyCellsNewRow(joinedRow, bWallLocation));


        }
    }

    private List<(int[], int)> JoinRowVerticalWalls(List<(int[], int)> row, List<int[]> sameSetWalls)
    {
        var rowCopy = new List<(int[], int)>(row);

        PlaceWall_XZ(Vector3.forward, rowCopy[0].Item1[0], rowCopy[0].Item1[1]);

        for (int i = 0; i < rowCopy.Count - 1; i++)
        {
            if (i + 1 < rowCopy.Count)
            {
                var currentCell = rowCopy[i];
                var nextCell = rowCopy[i + 1];
                bool join = false;
                bool sameSet = false;

                if (currentCell.Item2 == nextCell.Item2)
                {
                    join = true;
                    sameSet = true;
                }
                else
                {
                    int rand = UnityEngine.Random.Range(0, 101);

                    if (inRangeInclsuive(rand, 0, R_WallSpawnPersentage))
                    {
                        join = false;
                    }
                    else
                    {
                        join = true;
                    }
                }

                // Wall:
                if (!join)
                {
                    var coord = nextCell.Item1;

                    PlaceWall_XZ(Vector3.forward, coord[0], coord[1]);

                    if (sameSet)
                        sameSetWalls.Add(new int[] { coord[0], coord[1] + 1 });
                }
                // Join:
                else if (join)
                {
                    var newCellTup = (nextCell.Item1, currentCell.Item2);
                    rowCopy[i + 1] = newCellTup;
                }
            }
        }

        PlaceWall_XZ(Vector3.forward, rowCopy[rowCopy.Count - 1].Item1[0] + 1, rowCopy[rowCopy.Count - 1].Item1[1]);

        return rowCopy;
    }

    private void BottomWalls(List<(int[], int)> row, out List<int[]> bottomWallLocations)
    {
        var rowCopy = new List<(int[], int)>(row);

        var noBottomWallCells = new List<(int[], int)>();
        var bottomWallCells = new List<(int[], int)>();

        foreach (var setList in RowSortedBySet(rowCopy))
        {
            var shuffledSets = setList.OrderBy(a => rng.Next()).ToList();

            foreach (var (i, cell) in shuffledSets.Enumerate())
            {
                if (i == 0)
                {
                    noBottomWallCells.Add(cell);
                    continue;
                }

                int rand = UnityEngine.Random.Range(0, 101);

                if (inRangeInclsuive(rand, 0, B_WallSpawnPersentage)) bottomWallCells.Add(cell);
                else noBottomWallCells.Add(cell);

            }
        }

        List<int[]> bWalls = new List<int[]>();
        foreach (var cell in bottomWallCells)
        {
            bWalls.Add(cell.Item1);
        }
        bottomWallLocations = bWalls;

        foreach (var cellTup in IncreaseAxisRow(1, bottomWallCells))
        {
            var coord = cellTup.Item1;
            PlaceWall_XZ(Vector3.right, coord[0], coord[1]);
        }
    }

    private List<(int[], int)> EmptyCellsNewRow(List<(int[], int)> row, List<int[]> emptyCells)
    {
        var newRow = new List<(int[], int)>(row);
        foreach (var (i, cell) in row.Enumerate())
        {
            int cellX = cell.Item1[0];
            int cellZ = cell.Item1[1];

            foreach (var coord in emptyCells)
            {
                int eCellX = coord[0];
                int eCellZ = coord[1];

                if (cellX == eCellX && cellZ == eCellZ)
                {
                    var newCellTup = (cell.Item1, maxSetValue);
                    newRow[i] = newCellTup;
                    maxSetValue++;
                }
            }
        }

        return newRow;
    }

    private void FinalRow(List<(int[], int)> row, List<int[]> sameSetWalls)
    {
        PlaceWall_XZ(Vector3.forward, row[0].Item1[0], row[0].Item1[1]);

        foreach (var position in sameSetWalls)
            PlaceWall_XZ(Vector3.forward, position[0], position[1]);

        PlaceWall_XZ(Vector3.forward, row[row.Count - 1].Item1[0] + 1, row[row.Count - 1].Item1[1]);
    }

    #region HelperFunctions

    private List<(int[], int)> IncreaseAxisRow(int addAmount, List<(int[], int)> row)
    {
        var newRow = new List<(int[], int)>();
        foreach (var cellTup in row)
        {
            var coords = cellTup.Item1;
            var set = cellTup.Item2;
            
            int x = coords[0];
            int z = coords[1] + addAmount;

            int[] newCoords = new int[] { x, z };

            var newCellTup = (newCoords, set);

            newRow.Add(newCellTup);
        }

        return newRow;
    }

    private static System.Random rng = new System.Random();

    private List<List<(int[], int)>> RowSortedBySet(List<(int[], int)> row)
    {
        return row
            .Select((x) => new { Value = x})
            .GroupBy(x => x.Value.Item2)
            .Select(x => x.Select(v => v.Value).ToList())
            .ToList();
    }

    private void PlaceWalls_SameDirection(List<(int[], int)> row, Vector3 direction)
    {
        foreach (var cellTup in row)
        {
            var coords = cellTup.Item1;

            PlaceWall_XZ(direction, coords[0], coords[1]);
        }
    }

    private void PlaceWall_XZ(Vector3 direction, int x, int z)
    {
        Vector3 position = gridXZ.GetWorldPosition(x, z);
        position.y = wall_yvalue;

        GameObject.Instantiate(wall, position, Quaternion.LookRotation(direction));
    }

    private bool inRangeInclsuive(int num, int minRange, int maxRange)
    {
        return num >= minRange && num <= maxRange;
    }

    private void WorldTextRow(List<(int[], int)> row, Color textColour, int yVal)
    {
        GameObject rowObj = new GameObject();
        foreach (var cellTup in row)
        {
            var coords = cellTup.Item1;
            var set = cellTup.Item2;

            // Create Text GO:
            var text = new GameObject();
            var mesh = text.AddComponent<TextMesh>();
            mesh.text = set.ToString();
            mesh.color = textColour;

            // Place testGO:
            Vector3 gridPos = gridXZ.GetWorldPosition(coords[0], coords[1]);
            gridPos = new Vector3(gridPos.x + gridXZ.GetCellSize() / 2, yVal, gridPos.z + gridXZ.GetCellSize() / 2);
            text.transform.position = gridPos;
            text.transform.parent = rowObj.transform;
        }
    }

    #endregion
}

using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public int width;
    public int height;
    public int cellSize;
    public GameObject gridSpawnPOS;
    [HideInInspector] public CustomGrid grid;

    private void Awake()
    {
        grid = new CustomGrid(width, height, cellSize, gridSpawnPOS.transform.position);
    }
}

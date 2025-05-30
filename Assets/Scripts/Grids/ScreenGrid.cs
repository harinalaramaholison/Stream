using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenGrid : MonoBehaviour
{
    private int height = 3;
    private int width = 3;
    private float gridSpaceSize = 3f;
    [SerializeField] GameObject planePrefabs;
    private GameObject[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new GameObject[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x,y] = Instantiate(planePrefabs, new Vector3(x*gridSpaceSize, y*gridSpaceSize), Quaternion.identity);
                grid[x, y].transform.parent = transform;
                //grid[x,y].GetComponent<GridCell>().SetPosition(x, y);
                grid[x, y].transform.localScale = new Vector3(.3f,.3f,.3f);
                grid[x, y].transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
                
            }
        }
    }

    public Vector2Int GetGridPosFromXorld(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt(worldPosition.x / gridSpaceSize);
        int y = Mathf.FloorToInt(worldPosition.y / gridSpaceSize);

        x = Mathf.Clamp(x, 0, width);
        y = Mathf.Clamp(y, 0, height);

        return new Vector2Int(x, y);
    }

    public Vector3 GetWorldPosFromGridPos(Vector2Int gridPos)
    {
        float x = gridPos.x * gridSpaceSize;
        float y = gridPos.y * gridSpaceSize;

        return new Vector3(x, 0, y);
    }
}

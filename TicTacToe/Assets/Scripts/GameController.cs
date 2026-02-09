using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject xPrefab;
    public GameObject oPrefab;

    private int[,] grid = new int[3, 3]; // 0 vide, 1 X, 2 O
    private int currentPlayer = 1;
    private bool gameOver = false;

    void Start()
    {
        NewGame();
    }

    public void Play(Cell cell)
    {
        if (gameOver) return;
        if (cell.IsOccupied()) return;

        GameObject prefabToSpawn = currentPlayer == 1 ? xPrefab : oPrefab;

        Instantiate(
            prefabToSpawn,
            cell.transform.position,
            Quaternion.identity,
            cell.transform
        );

        grid[cell.x, cell.y] = currentPlayer;
        cell.SetOccupied();

        if (CheckVictory())
        {
            Debug.Log($"Joueur {(currentPlayer == 1 ? "X" : "O")} gagne !");
            gameOver = true;
            return;
        }

        currentPlayer = currentPlayer == 1 ? 2 : 1;
    }

    void NewGame()
    {
        grid = new int[3, 3];
        currentPlayer = 1;
        gameOver = false;
    }

    bool CheckVictory()
    {
        for (int i = 0; i < 3; i++)
        {
            if (grid[i, 0] == currentPlayer &&
                grid[i, 1] == currentPlayer &&
                grid[i, 2] == currentPlayer)
                return true;

            if (grid[0, i] == currentPlayer &&
                grid[1, i] == currentPlayer &&
                grid[2, i] == currentPlayer)
                return true;
        }

        if (grid[0, 0] == currentPlayer &&
            grid[1, 1] == currentPlayer &&
            grid[2, 2] == currentPlayer)
            return true;

        if (grid[0, 2] == currentPlayer &&
            grid[1, 1] == currentPlayer &&
            grid[2, 0] == currentPlayer)
            return true;

        return false;
    }
}

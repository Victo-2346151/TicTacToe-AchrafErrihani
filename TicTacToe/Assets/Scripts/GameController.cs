using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject xPrefab;
    public GameObject oPrefab;

    [Header("UI")]
    public TextMeshProUGUI currentPlayerText;
    public TextMeshProUGUI instructionsText;
    public GameObject gameOverPanel;
    public TextMeshProUGUI winnerText;

    [Header("AR")]
    public ARPlacement arPlacement;

    private int[,] grid = new int[3, 3];
    private int currentPlayer = 1;
    private bool gameOver = false;
    private int movesCount = 0;

    private Cell[] allCells;

    void Start()
    {
        allCells = FindObjectsOfType<Cell>();

        gameOverPanel.SetActive(false);
        instructionsText.text = "Scannez une surface...";
        UpdateCurrentPlayerText();
    }

    public void Play(Cell cell)
    {
        if (arPlacement != null && !arPlacement.IsPlaced()) return;

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
        movesCount++;

        if (CheckVictory())
        {
            gameOver = true;
            winnerText.text = (currentPlayer == 1 ? "X a gagné!" : "O a gagné!");
            gameOverPanel.SetActive(true);
            instructionsText.text = "Partie terminée";
            return;
        }

        if (movesCount >= 9)
        {
            gameOver = true;
            winnerText.text = "Match nul!";
            gameOverPanel.SetActive(true);
            instructionsText.text = "Partie terminée";
            return;
        }

        currentPlayer = currentPlayer == 1 ? 2 : 1;
        instructionsText.text = "Touchez une case pour jouer";
        UpdateCurrentPlayerText();
    }

    public void NewGame()
    {
        Debug.Log("UI: NewGame() clicked");
        grid = new int[3, 3];
        currentPlayer = 1;
        gameOver = false;
        movesCount = 0;

        gameOverPanel.SetActive(false);

        // Si la grille n'est pas placée
        if (arPlacement != null && !arPlacement.IsPlaced())
            instructionsText.text = "Scannez une surface...";
        else
            instructionsText.text = "Touchez une case pour jouer";

        UpdateCurrentPlayerText();

        // Reset cases
        if (allCells == null || allCells.Length == 0)
            allCells = FindObjectsByType<Cell>(FindObjectsSortMode.None);


        foreach (var c in allCells)
        {
            for (int i = c.transform.childCount - 1; i >= 0; i--)
                Destroy(c.transform.GetChild(i).gameObject);

            c.ResetCell();
        }
    }

    public void ResetPlacement()
    {
        Debug.Log("UI: ResetPlacement() clicked");
        //nouvelle partie
        NewGame();

        // supprime la grille et permet de la replacer
        if (arPlacement != null)
            arPlacement.ResetPlacement();

        instructionsText.text = "Scannez une surface...";
    }

    private void UpdateCurrentPlayerText()
    {
        currentPlayerText.text = "Tour de " + (currentPlayer == 1 ? "X" : "O");
    }

    private bool CheckVictory()
    {
        for (int i = 0; i < 3; i++)
        {
            if (grid[i, 0] == currentPlayer && grid[i, 1] == currentPlayer && grid[i, 2] == currentPlayer)
                return true;

            if (grid[0, i] == currentPlayer && grid[1, i] == currentPlayer && grid[2, i] == currentPlayer)
                return true;
        }

        if (grid[0, 0] == currentPlayer && grid[1, 1] == currentPlayer && grid[2, 2] == currentPlayer)
            return true;

        if (grid[0, 2] == currentPlayer && grid[1, 1] == currentPlayer && grid[2, 0] == currentPlayer)
            return true;

        return false;
    }
}

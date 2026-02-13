using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x;
    public int y;

    private bool occupied = false;
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void OnMouseDown()
    {
        // Sur Android, OnMouseDown fonctionne avec un touch
        if (gameController != null)
            gameController.Play(this);
    }

    public bool IsOccupied() => occupied;
    public void SetOccupied() => occupied = true;
    public void ResetCell() => occupied = false;
}

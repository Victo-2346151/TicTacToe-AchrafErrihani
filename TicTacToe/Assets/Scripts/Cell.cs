using UnityEngine;

public class Cell : MonoBehaviour
{
    //stocke la position logique (x, y)
    public int x;
    public int y;

    private bool occupied = false;

    //empeche de jouer deux fois sur la meme case
    public void SetOccupied()
    {
        occupied = true;
    }
    public bool IsOccupied()
    {
        return occupied;
    }
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void OnMouseDown()
    {
        if (gameController != null)
            gameController.Play(this);
    }

}

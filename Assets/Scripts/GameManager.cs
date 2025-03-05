using UnityEngine;

// This class manages the state of the running game .
// It records if the player has died, if the game is over and if the game has to restart.
public class GameManager : MonoBehaviour
{
    bool IsGameOver { get; set; }
    bool isPlayerDead { get; set; }

    private void Start()
    {
        IsGameOver = false;
        isPlayerDead = false;
    }
}

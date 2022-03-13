using UnityEngine;

public class GameStateManager : MonoBehaviour
{

    public enum GameState
    {
        InGame, InInventory, InPauseMenu, InMenu, InChest
    }

    //Current Game State
    public GameState CurrentGameState;
    //Player
    public PlayerBrain playerBrain;
    //Hud UI Manager
    public HudManager hudManager;

    // Start is called before the first frame update
    void Start()
    {
        ChangeGameState(GameState.InGame);
    }

    public void ChangeGameState(GameState newState)
    {
        CurrentGameState = newState;

        switch (newState)
        {
            case GameState.InGame:

                //UI
                hudManager.charInvCanvas.enabled = false;
                hudManager.abilityBarCanvas.enabled = true;
                hudManager.chestCanvas.enabled = false;
                break;
            case GameState.InInventory:

                //UI
                hudManager.charInvCanvas.enabled = true;
                hudManager.abilityBarCanvas.enabled = true;

                break;
            case GameState.InPauseMenu:

                //UI
                hudManager.charInvCanvas.enabled = false;
                hudManager.abilityBarCanvas.enabled = false;

                break;
            case GameState.InMenu:
                break;
            case GameState.InChest:
                hudManager.charInvCanvas.enabled = true;
                hudManager.abilityBarCanvas.enabled = true;
                hudManager.chestCanvas.enabled = true;
                break;
            default:
                break;
        }
    }
}

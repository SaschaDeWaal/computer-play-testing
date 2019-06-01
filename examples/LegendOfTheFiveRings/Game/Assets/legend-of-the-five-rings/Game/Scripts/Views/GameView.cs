using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour {
        
    [SerializeField] private PlayerStartValues player1StartValues;
    [SerializeField] private PlayerStartValues player2StartValues;

    public Game Game;
    
    private void Start() {
        CreateGame();
    }

    public void CreateGame() {
        Game = new Game();
        Game.PrepareGame(player1StartValues, player2StartValues, this);
        Game.StartGame();
    }

    public void ReplaceGame(Game newGame) {
        Game = null;
        Game = newGame;
        
        Game.ApplyChanges(new ChangeEvent());
        
        Game.PhaseManager.CurrentPhase.OnResumePhase(Game);
    }

}

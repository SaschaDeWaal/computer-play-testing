using System;
using System.Collections.Generic;
using System.Linq;
using ComputerPlayTesting;

public class MyPlayTester : PlayTester {
    
    public override List<PlayerAction> GetTestableGameActions(GameStatus gameStatus, int playerIndex) {
        Game game = ((MyGameStatus) gameStatus).Game;
        List<PlayerAction> playerActions = new List<PlayerAction>();

        // Loop thought all objects.
        // If action is possible to execute, add them to the list
        for (int i = 0; i < game.Players[playerIndex].WeaponsInHand.Count; i++) {
            PlayWeaponPlayerAction action = new PlayWeaponPlayerAction(i);

            if (action.IsExecutable(gameStatus, playerIndex)) {
                playerActions.Add(action);
            }
        }
        
        // Shuffle for randomness
        Random rnd = new Random();
        playerActions = playerActions.OrderBy(o => rnd.Next()).ToList();

        return playerActions;
    }
    
}

using ComputerPlayTesting;
using UnityEngine;

public class FiveRingsPlayTestObserver : PlayTestObserver {


	public FiveRingsPlayTestObserver() : base() {
		
	}

	public override void ObserveAfterExecutedAction(GameStatus prevGameStatus, GameStatus currentGameStatus, PlayerAction lastAppliedAction, int playerIndex) {
		base.ObserveAfterExecutedAction(prevGameStatus, currentGameStatus, lastAppliedAction, playerIndex);

		if (lastAppliedAction is FiveRingsPlayCharacterFromProvinceAction) {
			Player player = (currentGameStatus as FiveRingsGameStatus).Game.GetPlayer(playerIndex);
			
			if (player.PlayArea.Count > 0) {
				AddTracker(currentGameStatus, new FiveRingsCharacterInPlayTracker(playerIndex, player.PlayArea.Count - 1));
			}

		}

		
	}
}

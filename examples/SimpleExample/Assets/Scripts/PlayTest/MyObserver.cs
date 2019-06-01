using ComputerPlayTesting;
using UnityEngine;


public class MyObserver : PlayTestObserver {


	public override void ObserveAfterExecutedAction(GameStatus prevGameStatus, GameStatus currentGameStatus, PlayerAction lastAppliedAction, int playerIndex) {
		base.ObserveAfterExecutedAction(prevGameStatus, currentGameStatus, lastAppliedAction, playerIndex);
		
		// If last action is PlayWeaponPlayerAction, we will add a tracker for this item
		if (lastAppliedAction is PlayWeaponPlayerAction) {
			PlayWeaponPlayerAction weaponAction = (PlayWeaponPlayerAction) lastAppliedAction;
			AddTracker(currentGameStatus, new MyObjectTracker(playerIndex, weaponAction.WeaponName));
		}

	}
	
}

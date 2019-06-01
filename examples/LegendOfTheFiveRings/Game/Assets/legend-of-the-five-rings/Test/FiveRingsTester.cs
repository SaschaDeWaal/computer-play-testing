using System.Collections.Generic;
using ComputerPlayTesting;
using UnityEngine;

public class FiveRingsTester : PlayTester {
    
	public override void OnStartTest(GameStatus gameStatus, int playerIndex) {

	}
	
	public override List<PlayerAction> GetTestableGameActions(GameStatus gameStatus, int playerIndex) {

		FiveRingsGameStatus FiveRingsGameStatus = (FiveRingsGameStatus)gameStatus;
		
		Game game = FiveRingsGameStatus.Game;
		Player player = game.GetPlayer(playerIndex);

		if (game.CurrentGameStatus != Game.GameStatus.Playing) {
			return new List<PlayerAction>();
		}

		if (game.TurnIndex != playerIndex && game.TurnIndex >= 0) {
			return new List<PlayerAction>();
		}
		
		List<PlayerAction> actions = new List<PlayerAction>();

		// generic phase
		if (game.PhaseManager.CurrentGamePhase != PhaseManager.GamePhase.Conflict && (new FiveRingsPassAction()).IsExecutable(FiveRingsGameStatus, playerIndex)) {
			actions.Add(new FiveRingsPassAction());
		}
		
		// DynastyPhase
		if (game.PhaseManager.CurrentGamePhase == PhaseManager.GamePhase.Dynasty) {
			for (int i = 0; i < 5; i++) {
				FiveRingsPlayCharacterFromProvinceAction action = new FiveRingsPlayCharacterFromProvinceAction(i);
				if (action.IsExecutable(FiveRingsGameStatus, playerIndex)) {
					actions.Add(action);
				}
			}
			
		}
		

		// Draw phase
		if (game.PhaseManager.CurrentGamePhase == PhaseManager.GamePhase.Draw) {
			if ((new FiveRingsDialAction(3)).IsExecutable(FiveRingsGameStatus, playerIndex)) {
				actions.Add(new FiveRingsDialAction(3));
			}
		}

		// Conflict phase
		if (game.PhaseManager.CurrentGamePhase == PhaseManager.GamePhase.Conflict) {

			bool canDefend = false;
			bool canFight = false;
			

			for (int province = 0; province < 5; province++) {
				for (int characters = 0;
					characters < game.GetPlayer(playerIndex).PlayArea.Count;
					characters++) {

					FiveRingsDeclareConflict action = (new FiveRingsDeclareConflict((Random.value > 0.5f) ? ConflictType.Military : ConflictType.Political, (ElementType) 1, new[] {characters}, province));
							
					if (action.IsExecutable(FiveRingsGameStatus, playerIndex)) {
						actions.Add(action);
						canFight = true;
					}
				}
			}
			
			for (int characters = 0; characters < player.PlayArea.Count; characters++) {
				FiveRingsDeclareDefence action = new FiveRingsDeclareDefence(new []{ characters });

				if (action.IsExecutable(FiveRingsGameStatus, playerIndex)) {
					actions.Add(action);
					canDefend = true;
				}
			}

			for (int characters = 0; characters < player.PlayArea.Count; characters++) {
				for (int handIndex = 0; handIndex < player.Hand.Count; handIndex++) {
					if (player.Hand[handIndex].Is<Attachment>()) {
						FiveRingsAddAttachment action = new FiveRingsAddAttachment(characters, handIndex);
						
						
						if (action.IsExecutable(FiveRingsGameStatus, playerIndex)) {
							actions.Add(action);
						}
					}
				}
			}

			for (int handIndex = 0; handIndex < player.Hand.Count; handIndex++) {
				if (player.Hand[handIndex].Is<Character>()) {
					for (int fate = -1; fate < player.FatePool; fate++) {
						for (int placeInBattle = 0; placeInBattle < 2; placeInBattle++) {
							FiveRingsPlayCharacterFromHand action = new FiveRingsPlayCharacterFromHand(handIndex, placeInBattle == 0, fate);
						}
					}
				}

			}


			// generic phase
			if (!canDefend && !canFight && (new FiveRingsPassAction()).IsExecutable(FiveRingsGameStatus, playerIndex)) {
				actions.Add(new FiveRingsPassAction());
			}
			
		}

		return BestByWeight(actions, 2, true);
	}

}

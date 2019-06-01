using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ComputerPlayTesting;
using UnityEngine;

[DataContract]
public class FiveRingsPlayCharacterFromHand : PlayerAction {

	private int _handIndex;
	private bool _placeInBattle;
	private int _addFaePoints;

	public FiveRingsPlayCharacterFromHand(int handIndex, bool placeInBattle, int addFatePoints) {
		_handIndex = handIndex;
		_placeInBattle = placeInBattle;
		_addFaePoints = addFatePoints;
	}
	

	
	public override void Execute(GameStatus PlayTestData, int playerIndex) {
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		Player player = data.Game.GetPlayer(playerIndex);

		Controllers.Run(new PlayCharacterFromHandController(player, player.Hand[_handIndex] as Character, _placeInBattle));

		for (int i = 0; i < _addFaePoints; i++) {
			Controllers.Run(new PlaceFateOnCharacterFromHandController(player));
		}
	}
	
	public override int GetWeight() {
		return 40;
	}


	public override bool IsExecutable(GameStatus PlayTestData, int playerIndex) {
		
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		Player player = data.Game.GetPlayer(playerIndex);

		return (new PlayCharacterFromHandController(player, player.Hand[_handIndex] as Character, _placeInBattle)).CanBeExecuted();
	}
}

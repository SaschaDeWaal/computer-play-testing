using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ComputerPlayTesting;
using UnityEngine;

public class FiveRingsPlaceFateOnCharacter : PlayerAction {

	
	public override void Execute(GameStatus PlayTestData, int playerIndex) {
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		if (Controllers.Run(new PlaceFateOnCharacterFromProvince(playerIndex)) == false) {
			return;
		}
	}


	public override bool IsExecutable(GameStatus PlayTestData, int playerIndex) {
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		return (new PlaceFateOnCharacterFromProvince(playerIndex)).CanBeExecuted();
	}

}

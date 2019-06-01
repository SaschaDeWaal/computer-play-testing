using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ComputerPlayTesting;
using UnityEngine;

[DataContract]
public class FiveRingsPassAction : PlayerAction {


	public FiveRingsPassAction() {
		
	}

	public override void Execute(GameStatus PlayTestData, int playerIndex) {
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		if (Controllers.Run(new PlayerReadyTurn(data.Game.GetPlayer(playerIndex))) == false) {
			Debug.LogWarning("Can't pass");
		}
	}

	public override int GetWeight() {
		return 100;
	}
	
	public override bool IsExecutable(GameStatus PlayTestData, int playerIndex) {
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		return (new PlayerReadyTurn(data.Game.GetPlayer(playerIndex))).CanBeExecuted();
	}
}

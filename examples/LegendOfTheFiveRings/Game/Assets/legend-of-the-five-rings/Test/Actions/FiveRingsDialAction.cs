using ComputerPlayTesting;
using UnityEngine;

public class FiveRingsDialAction: PlayerAction {

	private readonly int _dialNumber;

	public FiveRingsDialAction(int dialNumber) {
		_dialNumber = dialNumber;
	}
	
	public override void Execute(GameStatus PlayTestData, int playerIndex) {
		
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		if (Controllers.Run(new SelectNumberOnDialController(data.Game.GetPlayer(playerIndex), _dialNumber)) == false) {
			Debug.LogWarning("Can't select dial");
		}
	}
	
	public override int GetWeight() {
		return 100;
	}

	public override bool IsExecutable(GameStatus PlayTestData, int playerIndex) {
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		return (new SelectNumberOnDialController(data.Game.GetPlayer(playerIndex), _dialNumber)).CanBeExecuted();
	}
}

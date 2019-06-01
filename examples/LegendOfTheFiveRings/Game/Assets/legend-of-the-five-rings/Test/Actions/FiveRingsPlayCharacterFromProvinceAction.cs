using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ComputerPlayTesting;
using UnityEngine;

[DataContract]
public class FiveRingsPlayCharacterFromProvinceAction : PlayerAction {

	private readonly int _provinceIndex;
	private Character _card;

	public FiveRingsPlayCharacterFromProvinceAction(int provinceIndex) {
		_provinceIndex = provinceIndex;
	
	}
	
	public Character GetCar() {
		return _card;
	}
	
	public override void Execute(GameStatus PlayTestData, int playerIndex) {
				
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		Player player = data.Game.GetPlayer(playerIndex);
		_card = (player.Provinces[_provinceIndex].DynastyCard as Character);
		
		if (Controllers.Run(new PlayCharacterFromProvinces(_provinceIndex, playerIndex)) == false) {
			Debug.LogWarning("Can't play character from provinces");
		}
	}
	
	public override int GetWeight() {
		return 80;
	}
	
	public override bool IsExecutable(GameStatus PlayTestData, int playerIndex) {		
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		return (new PlayCharacterFromProvinces(_provinceIndex, playerIndex)).CanBeExecuted();
	}
	
}

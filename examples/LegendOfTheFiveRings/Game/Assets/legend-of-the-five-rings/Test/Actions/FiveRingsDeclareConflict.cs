using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ComputerPlayTesting;
using UnityEngine;

[DataContract]
public class FiveRingsDeclareConflict : FiveRingsCardAction {

	public ConflictType ConflictType { get; private set; }
	private ElementType _elementType;
	public int[] Characters { get; private set; }
	private int _province;
	
	public FiveRingsDeclareConflict(ConflictType conflictType, ElementType elementType, int[] characters, int province) {
		ConflictType = conflictType;
		_elementType = elementType;
		Characters = characters;
		_province = province;
	}
	
	public override string GetCardId(FiveRingsGameStatus PlayTestData, int playerIndex) {
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		Player player = data.Game.GetPlayer(playerIndex);
		Character[] characters = Characters.ToList().Select((i) => player.PlayArea[i] as Character).ToArray();
		
		return (characters[0]).ToString();
	}

	public override void Execute(GameStatus PlayTestData, int playerIndex) {
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		Player player = data.Game.GetPlayer(playerIndex);
		Player otherPlayer = data.Game.GetPlayer((playerIndex == 0) ? 1 : 0);
		Character[] characters = Characters.ToList().Select((i) => player.PlayArea[i] as Character).ToArray();
		
		if (Controllers.Run(new DeclareConflictController(ConflictType, _elementType, characters, player,
			otherPlayer.Provinces[_province])) == false) {
			Debug.LogWarning("failed");
		}
	}
	
	public override int GetWeight() {
		return 80;
	}


	public override bool IsExecutable(GameStatus PlayTestData, int playerIndex) {
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		Player player = data.Game.GetPlayer(playerIndex);
		Player otherPlayer = data.Game.GetPlayer((playerIndex == 0) ? 1 : 0);
		Character[] characters = Characters.ToList().Select((i) => player.PlayArea[i] as Character).ToArray();
		
		return (new DeclareConflictController(ConflictType, _elementType, characters, player, otherPlayer.Provinces[_province])).CanBeExecuted();
	}
	
	public override bool IsRelevantForCard(int character) {
		return Characters.Contains(character);
	}

}
		
	

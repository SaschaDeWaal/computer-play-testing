using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ComputerPlayTesting;
using UnityEngine;

[DataContract]

public class FiveRingsDeclareDefence : FiveRingsCardAction {

	public int[] Characters { get; private set; }

	public FiveRingsDeclareDefence(int[] characters) {
		Characters = characters;
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
		Character[] characters = Characters.ToList().Select((i) => player.PlayArea[i] as Character).ToArray();
		
		Controllers.Run(new DeclareDefenceController(player, characters));
	}

	public override int GetWeight() {
		return 1000;
	}

	public override bool IsExecutable(GameStatus PlayTestData, int playerIndex) {
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		Player player = data.Game.GetPlayer(playerIndex);
		Character[] characters = Characters.ToList().Select((i) => player.PlayArea[i] as Character).ToArray();
		
		return (new DeclareDefenceController(player, characters)).CanBeExecuted();
	}
	
	public override bool IsRelevantForCard(int character) {
		return Characters.Contains(character);
	}
}
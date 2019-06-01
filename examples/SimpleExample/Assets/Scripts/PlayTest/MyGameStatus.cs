using System.Runtime.Serialization;
using ComputerPlayTesting;

[DataContract] //<-- We are going to copy game status. Instead of creating a deep copy, we will use Serialization
public class MyGameStatus : GameStatus {
    
	// Contains our game
	[DataMember] public Game Game;

	public MyGameStatus(int madeActions = 0) : base(madeActions) {
		
	}

	// This will be called when we jump to this status
	public override void OnWillJumpToThis() {
		
		// We will replace the current game object with the game from this status
		Game.Instance = Game;
	}


	// This will be called when we jump away from this point
	public override void OnWillJumpAway() {
		// In this example, we don't need this
		// But when you work with saving system, you should save the file here.
	}

}

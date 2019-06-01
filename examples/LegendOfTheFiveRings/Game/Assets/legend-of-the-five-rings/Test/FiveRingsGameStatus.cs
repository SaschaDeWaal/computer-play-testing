using System.Runtime.Serialization;
using ComputerPlayTesting;

[DataContract]
public class FiveRingsGameStatus : GameStatus {

	[DataMember] public Game Game;
	
	public FiveRingsGameStatus(int madeActions = 0) : base(madeActions) {
		Game = Game.Instance;
		Game.AnimationEnabled = false;
	}
	
	
	public override void OnWillJumpToThis() {
		Game.GameViewInstance.ReplaceGame(Game);
	}

	public override void OnWillJumpAway() {
		Game.PhaseManager.CurrentPhase.OnEndPhase();
	}

	public override void OnDestroy() {
		Game.PhaseManager.CurrentPhase.OnEndPhase();
	}

}

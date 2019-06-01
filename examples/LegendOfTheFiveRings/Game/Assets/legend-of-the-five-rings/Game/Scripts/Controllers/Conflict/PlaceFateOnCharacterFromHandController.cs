

public class PlaceFateOnCharacterFromHandController : PhaseDependedController<ConflictPhase> {

	private Player _player;
	
	public PlaceFateOnCharacterFromHandController(Player player) {
		_player = player;
	}

	public override bool Execute() {
		if (!CanBeExecuted()) {
			CurGame.EventText = "Can't place fate on character";
			return false;
		}

		Character Character = CurPhase.LastPlayedCharacter.As<Character>();
		Character.FatePoints++;
		_player.FatePool--;
		
		Game.Instance.ApplyChanges(ChangeEvent.Create(EventType.FateTokens));

		return true;

	}

	protected override bool CanBeExecutedWithCorrectPhase() {
		return CurPhase.LastPlayedCharacter != null &&
		       IsTurn(_player) &&
		       _player.FatePool > 0;
	}
}
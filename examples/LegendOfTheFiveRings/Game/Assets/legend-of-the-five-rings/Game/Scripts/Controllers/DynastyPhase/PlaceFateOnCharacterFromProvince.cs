
using UnityEngine;

public class PlaceFateOnCharacterFromProvince : PhaseDependedController<DynastyPhase> {

	private int _playerIndex;

	public PlaceFateOnCharacterFromProvince(int player) {
		_playerIndex = player;
	}

	public override bool Execute() {
		if (!CanBeExecuted()) {
			CurGame.EventText = "Can't place fate on character";
			return false;
		}

		Character Character = CurPhase.LastPlayedCard.As<Character>();
		Player player = CurGame.GetPlayer(_playerIndex);
		Character.FatePoints++;
		player.FatePool--;
		
		Game.Instance.ApplyChanges(ChangeEvent.Create(EventType.FateTokens));
		
		return true;

	}

	protected override bool CanBeExecutedWithCorrectPhase() {
		Player player = CurGame.GetPlayer(_playerIndex);
		
		return CurPhase.LastPlayedCard != null &&
		       IsTurn(player) &&
		       player.FatePool > 0;
	}
}


using UnityEngine;

public class PlayerReadyTurn : BaseController {

	private Player _player;
	
	public PlayerReadyTurn(Player player) {
		_player = player;
	}
	
	public override bool CanBeExecuted() {
		return IsTurn(_player);
	}

	public override bool Execute() {
		if (!CanBeExecuted()) return false;
		Game.Instance.EndTurn();
		Game.Instance.ApplyChanges(ChangeEvent.Create(EventType.EndTurn, _player));

		return true;
	}

	
}

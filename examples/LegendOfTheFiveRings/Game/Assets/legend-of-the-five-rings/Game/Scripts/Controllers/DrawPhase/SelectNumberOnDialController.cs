using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectNumberOnDialController : PhaseDependedController<DrawPhase> {

	private Player _player;
	private int _number;
	
	public SelectNumberOnDialController(Player player, int number) {
		_player = player;
		_number = number;
	}

	public override bool Execute() {
		if (!CanBeExecuted()) {
			CurGame.EventText = "Can't select a dial (now)";
			return false;
		}

		CurPhase.SetPlayerSelection(_player, _number);
		CurGame.ApplyChanges(ChangeEvent.Create(EventType.SelectDial, _player));

		return true;
	}

	protected override bool CanBeExecutedWithCorrectPhase() {
		return CurPhase.playerSelection[_player.Index] < 0;
	}
	
}

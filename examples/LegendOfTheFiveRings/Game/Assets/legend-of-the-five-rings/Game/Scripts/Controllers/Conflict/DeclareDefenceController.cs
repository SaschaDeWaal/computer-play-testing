using System.Linq;
using UnityEngine;

public class DeclareDefenceController: PhaseDependedController<ConflictPhase> {

	private Character[] _defendingCharacters;
	private Player _player;

	public DeclareDefenceController(Player player, Character[] defendingCharacters) {
		_defendingCharacters = defendingCharacters;
		_player = player;
	}

	public override bool Execute() {
		if (!CanBeExecuted()) {
			CurGame.EventText = "Not allowed to declare a defence";
			return false;
		}

		CurPhase.DeclareDefence(_player, _defendingCharacters);
		CurGame.EndTurn();
		CurGame.ApplyChanges(ChangeEvent.Create(EventType.DeclaredDefence));
		
		return true;
	}

	protected override bool CanBeExecutedWithCorrectPhase() {
		return IsTurn(_player) &&
		       CurPhase.DeclaredConflict == true &&
		       CurPhase.DeclaredDefence == false &&
		       CurPhase.AttackingPlayer.Index != _player.Index &&
		       _defendingCharacters.Length > 0 &&
		       _defendingCharacters.Any(e => e.Owner.Index != _player.Index) == false &&
		       _defendingCharacters.Any(e => !e.Bowed);
	}
}

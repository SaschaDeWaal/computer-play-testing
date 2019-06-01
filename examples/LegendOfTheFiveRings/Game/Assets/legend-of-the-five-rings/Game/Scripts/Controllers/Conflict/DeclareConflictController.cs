using System.Linq;
using UnityEngine;

public class DeclareConflictController: PhaseDependedController<ConflictPhase> {

	private ConflictType _conflictType;
	private ElementType _elementType;
	private Character[] _attackingCharacters;
	private Player _player;
	private Province _province;

	public DeclareConflictController(ConflictType conflictType, ElementType elementType, Character[] characterInPlayViews, Player player, Province province) {
		_conflictType = conflictType;
		_elementType = elementType;
		_attackingCharacters = characterInPlayViews;
		_player = player;
		_province = province;
	}

	public override bool Execute() {
		
		if (!CanBeExecuted()) {
			CurGame.EventText = "Not allowed to start conflict";
			return false;
		}
		
		CurPhase.DeclareConflict(_player, _conflictType, _elementType, _attackingCharacters, _province);
		_province.OpenProvince();
		CurGame.EndTurn();
		CurGame.ApplyChanges(ChangeEvent.Create(EventType.DeclaredConflict));
		
		return true;
	}

	protected override bool CanBeExecutedWithCorrectPhase() {
		
		return IsTurn(_player) &&
		       _province.AllowedToAttack() &&
		      CurPhase.DeclaredConflict == false &&
		      _conflictType != ConflictType.None &&
		      _elementType != ElementType.None &&
		      CurPhase.ElementOwner.ContainsKey(ElementType.None) == false &&
		      CurPhase.ElementOwner.Count() < 4 &&
		       
		       _attackingCharacters.Length > 0 &&
		       _attackingCharacters.Any(e => e.Owner != _player) == false &&
		       _attackingCharacters.Any(e => !e.Bowed) &&
		       _province.Owner != _player &&
		       _province.Standing;
	}


}

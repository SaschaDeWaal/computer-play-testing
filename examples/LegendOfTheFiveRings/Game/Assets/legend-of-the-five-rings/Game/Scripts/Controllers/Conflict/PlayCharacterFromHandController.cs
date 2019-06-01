using UnityEngine;

public class PlayCharacterFromHandController : PhaseDependedController<ConflictPhase>  {
		
	private Player _player;
	private Character _character;
	private bool _placeInBattle;

	public PlayCharacterFromHandController(Player player, Character character, bool placeInBattle) {
		_player = player;
		_character = character;
		_placeInBattle = placeInBattle;
	}

	public override bool Execute() {
		if (!CanBeExecuted()) {
			//Debug.Log(CurPhase.);
			
			CurGame.EventText = "Not allowed to play this card from hand (now)";
			Debug.Log(IsTurn(_player));
			Debug.Log(CurPhase.AllowedToDoAction);
			Debug.Log(_player.FatePool >= _character.Card.cost);
			return false;
		}


		_player.FatePool -= _character.Card.cost;
		_player.PlayArea.Add(_character);
		_player.Hand.Remove(_character);
		
		CurPhase.CharacterPlaced(_character, _player, _placeInBattle);	
		CurGame.ApplyChanges(ChangeEvent.Create(EventType.PlayerCharacter));

		return true;

	}

	protected override bool CanBeExecutedWithCorrectPhase() {
		return IsTurn(_player) &&
		       CurPhase.AllowedToDoAction &&
		       _player.FatePool >= _character.Card.cost;


	}
}

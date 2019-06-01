
using UnityEngine;

public class AddAttachmentToCharacter : PhaseDependedController<ConflictPhase> {
	
	
	private Player _player;
	private Character _character;
	private Attachment _attachment;
	
	public AddAttachmentToCharacter(Player player, Character character, Attachment attachment) {
		_player = player;
		_character = character;
		_attachment = attachment;
	}
	
	public override bool Execute() {
		if (!CanBeExecuted()) {
			CurGame.EventText = "Not allowed to add this attachment";
			if (!IsTurn(_player)) Debug.LogWarning("It's not your turn");
			if (_character.Owner != _player) Debug.LogWarning("This is not your card");
			if (_player.FatePool < _attachment.CardData.cost) Debug.LogWarning("not enough fate points");
			return false;
		}
		
		_player.FatePool -= _attachment.CardData.cost;
		_character.AddAttachment(_attachment);
		_player.Hand.Remove(_attachment);
		
		CurGame.ApplyChanges(ChangeEvent.Create(EventType.AddAttachment));

		return true;

	}

	protected override bool CanBeExecutedWithCorrectPhase() {
		return IsTurn(_player) &&
		       CurPhase.AllowedToDoAction &&
		       _character.Owner == _player &&
		       _player.FatePool >= _attachment.CardData.cost &&
		       ((_character.Card.characterTraits & _attachment.CardData.characterTraits) != 0);
	}
	
}

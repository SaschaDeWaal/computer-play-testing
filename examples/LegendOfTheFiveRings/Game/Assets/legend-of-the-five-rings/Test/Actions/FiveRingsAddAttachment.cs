using ComputerPlayTesting;
using UnityEngine;

public class FiveRingsAddAttachment : FiveRingsCardAction {
	
	public readonly int Character;
	public Attachment AddedAttachment { get; private set; }
	private readonly int _attachment;
	
	public FiveRingsAddAttachment(int character, int handCard) {
		Character = character;
		_attachment = handCard;
	}
	public override void Execute(GameStatus PlayTestData, int playerIndex) {
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		Player player = data.Game.GetPlayer(playerIndex);

		AddedAttachment = player.Hand[_attachment] as Attachment;

		Controllers.Run((new AddAttachmentToCharacter(player, player.PlayArea[Character] as Character, AddedAttachment)));
	}

	public override int GetWeight() {
		return 10;
	}

	public override bool IsExecutable(GameStatus PlayTestData, int playerIndex) {
		
		FiveRingsGameStatus data = PlayTestData as FiveRingsGameStatus;
		Player player = data.Game.GetPlayer(playerIndex);
		AddAttachmentToCharacter controller = (new AddAttachmentToCharacter(player, player.PlayArea[Character] as Character, player.Hand[_attachment] as Attachment));

		return controller.CanBeExecuted();
	}

	public override bool IsRelevantForCard(int character) {
		return (character == Character);
	}
}

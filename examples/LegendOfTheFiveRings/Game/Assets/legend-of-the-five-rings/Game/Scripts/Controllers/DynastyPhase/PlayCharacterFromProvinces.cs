using UnityEngine;

public class PlayCharacterFromProvinces : PhaseDependedController<DynastyPhase> {

	private int _provinceIndex;
	private int _playerIndex;
	
	public PlayCharacterFromProvinces(int province, int playerIndex) : base() {
		_provinceIndex = province;
		_playerIndex = playerIndex;
	}

	public override bool Execute() {
		Player player = CurGame.GetPlayer(_playerIndex);
		Province province = player.Provinces[_provinceIndex];
		
		if (!CanBeExecuted()) {
			CurGame.EventText = "Not allowed to play this card (now)";
			return false;
		}

		Card card = province.DynastyCard;
		
		player.PlayProvinces(province);
		CurGame.ApplyChanges(ChangeEvent.Create(EventType.PlayerCharacter, card, player));
		return true;
	}

	protected override bool CanBeExecutedWithCorrectPhase() {
		Player player = CurGame.GetPlayer(_playerIndex);
		Province province = player.Provinces[_provinceIndex];
		
		return (province != null && 
		        province.CardCanBePlayed && 
		        CurPhase.AllowedToPlayCard &&
		        IsTurn(player) &&
		        province.DynastyCard.As<Character>().Card.cost <= player.FatePool);
	}
}

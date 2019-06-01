using System.Runtime.Serialization;

public enum EventType {
	None,
	PlayerCharacter,
	RevealCard,
	FateTokens,
	EndTurn,
	PhaseChanged,
	SelectDial,
	HonorTokens,
	DrawCards,
	DeclaredConflict,
	DeclaredDefence,
	AddAttachment,
	GameWon,
	BrokeProvince,
	WonConflict,
	LostConflict,
	EventCard
}
[DataContract]
public struct ChangeEvent {

	[DataMember] public EventType EventType { get; private set; }
	[DataMember] public string Description { get; private set; }
	[DataMember] public Card ChangedCard { get; private set; }
	[DataMember] public string[] Data { get; private set; }
	public Player ChangedPlayer { get; private set; }

	[DataMember] public int ChangedPlayerIndex {
		get => ChangedPlayer?.Index ?? -1;
		set => ChangedPlayer = Game.Instance.GetPlayer(value);
	}
	
	private ChangeEvent(EventType eventType, string description = "", Card changedCard = null, Player changedPlayer = null, string[] data = null) {
		EventType = eventType;
		Description = description;
		ChangedCard = changedCard;
		ChangedPlayer = changedPlayer;
		Data = data;
	}

	public static ChangeEvent Empty => new ChangeEvent(EventType.None, "");

	public static ChangeEvent Create(EventType eventType, string description = "") {
		return new ChangeEvent(eventType, description);
	}
	
	public static ChangeEvent Create(EventType eventType, Player player, string description = "") {
		return new ChangeEvent(eventType, description, null, player);
	}
	
	public static ChangeEvent Create(EventType eventType, Card ChangedCard, Player changedPlayer, string description = "") {
		return new ChangeEvent(eventType, description, ChangedCard, changedPlayer);
	}
	
	public static ChangeEvent Create(EventType eventType, Card ChangedCard, Player changedPlayer, string[] data) {
		return new ChangeEvent(eventType, "", ChangedCard, changedPlayer, data);
	}
}
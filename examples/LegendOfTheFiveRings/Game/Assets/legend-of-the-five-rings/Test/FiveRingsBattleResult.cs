using System.Runtime.Serialization;
using ComputerPlayTesting;

[DataContract]
public class FiveRingsBattleResult : PlayTestResult {

	[DataMember(Name = "cardName")] private string _cardName;
	[DataMember(Name = "player")] private int _player;
	[DataMember(Name = "fatePoints")] private int[] _fatePoints;
	[DataMember(Name = "battles")] private BattleResult _battles;
	[DataMember(Name = "attachments")] private string[] _attachments;
	[DataMember(Name = "militaryHistory")] private int[] _militaryHistory;
	[DataMember(Name = "politicalHistory")] private int[] _politicalHistory;

	[DataMember] public override string ResultType {
		get => "Battle";
		set { }
	}

	public FiveRingsBattleResult(string cardName, int player, BattleResult battles, string[] attachments, int[] militaryHistory, int[] politicalHistory) : base() {
		_battles = battles;
		_cardName = cardName;
		_player = player;
		_attachments = attachments;
		_militaryHistory = militaryHistory;
		_politicalHistory = politicalHistory;
	}
	
}

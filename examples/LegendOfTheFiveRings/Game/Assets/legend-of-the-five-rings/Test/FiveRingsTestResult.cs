using System.Runtime.Serialization;
using ComputerPlayTesting;

[DataContract]
public struct BattleResult {

	public BattleResult(bool started, bool wonBattle, string typeBattles, int selfBattlePoints, int otherBattlePoints, string[] enemyCards) {
		this.started = started;
		this.wonBattle = wonBattle;
		this.typeBattle = typeBattles;
		this.selfBattlePoints = selfBattlePoints;
		this.otherBattlePoints = otherBattlePoints;
		this.enemyCards = enemyCards;

	}
	
	[DataMember] public bool wonBattle;
	[DataMember] public string typeBattle;
	[DataMember] public int selfBattlePoints;
	[DataMember] public int otherBattlePoints;
	[DataMember] public bool started;
	[DataMember] public string[] enemyCards;
}


[DataContract]
public class FiveRingsTestResult : PlayTestResult {

	[DataMember(Name = "cardName")] private string _cardName;
	[DataMember(Name = "player")] private int _player;
	[DataMember(Name = "fatePoints")] private int[] _fatePoints;
	[DataMember(Name = "battles")] private BattleResult[] _battles;
	[DataMember(Name = "attachments")] private string[] _attachments;
	[DataMember(Name = "militaryHistory")] private int[] _militaryHistory;
	[DataMember(Name = "politicalHistory")] private int[] _politicalHistory;

	[DataMember] public override string ResultType {
		get => "Character";
		set { }
	}

	public FiveRingsTestResult(string cardName,  int player, int[] fatePoints, BattleResult[] battles, string[] attachments, int[] militaryHistory, int[] politicalHistory) : base() {
		_fatePoints = fatePoints;
		_battles = battles;
		_cardName = cardName;
		_player = player;
		_attachments = attachments;
		_militaryHistory = militaryHistory;
		_politicalHistory = politicalHistory;
	}
	
}

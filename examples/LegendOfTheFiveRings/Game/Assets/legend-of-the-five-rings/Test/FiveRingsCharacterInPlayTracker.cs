using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ComputerPlayTesting;
using UnityEngine;
using UnityEngine.UI;

[DataContract]
public class FiveRingsCharacterInPlayTracker : PlayTestObjectTracker {


	[DataMember] private int _playerIndex = -1;
	[DataMember] private int _areaIndex = -1;
	
	[DataMember] private string _cardName = "";
	[DataMember] private List<int> _fateCost = new List<int>();
	[DataMember] private List<BattleResult> _battleResults;
	[DataMember] private List<string> _addedAttachments = new List<string>();
	[DataMember] private List<int> _militaryHistory = new List<int>();
	[DataMember] private List<int> _politicalHistory = new List<int>();

	[DataMember] private bool isInBattle = false;

	public FiveRingsCharacterInPlayTracker(int playerIndex, int areaIndex) : base() {
		_playerIndex = playerIndex;
		_areaIndex = areaIndex;
	}

	public override void OnStartTracking(GameStatus gameStatus) {
		Character character = GetCharacter(gameStatus);
		_cardName = character.CharacterCardName;
		_battleResults = new List<BattleResult>();
		_fateCost.Add(character.Card.cost + character.FatePoints);
		_militaryHistory.Add(character.Card.militaryPoints);
		_politicalHistory.Add(character.Card.politicalPoints);
	}

	public override void OnTick(GameStatus gameStatus) {
		Character character = GetCharacter(gameStatus);

		if (!CharacterExsist(gameStatus)) {

			FiveRingsTestResult result = new FiveRingsTestResult(_cardName, _playerIndex, _fateCost.ToArray(), _battleResults.ToArray(), _addedAttachments.ToArray(), _militaryHistory.ToArray(), _politicalHistory.ToArray());
			StoreResult(result);
			Destroy();
			return;
		}
	}

	public override void OnActionExecuted(GameStatus prevGameStatus, GameStatus currentGameStatus, PlayerAction lastAppliedAction, int playerIndex) {
		CheckEvents(prevGameStatus, currentGameStatus, lastAppliedAction, playerIndex);
		CheckAction(prevGameStatus, currentGameStatus, lastAppliedAction, playerIndex);
	}

	private void CheckAction(GameStatus prevGameStatus, GameStatus currentGameStatus, PlayerAction lastAppliedAction, int playerIndex) {
		if (CharacterExsist(currentGameStatus) == false || playerIndex != _playerIndex || !(lastAppliedAction is FiveRingsCardAction) || ((FiveRingsCardAction)lastAppliedAction)?.IsRelevantForCard(_areaIndex) == false) {
			return;
		}
		
		Character curCharacter = GetCharacter(currentGameStatus);
		Character prevCharacter = GetCharacter(prevGameStatus);
		Game game = ((FiveRingsGameStatus) currentGameStatus).Game;
		
		if (lastAppliedAction is FiveRingsAddAttachment) {
			FiveRingsAddAttachment addAttachment = (FiveRingsAddAttachment) lastAppliedAction;
			_fateCost.Add(addAttachment.AddedAttachment.CardData.cost);
			_addedAttachments.Add(addAttachment.AddedAttachment.CardData.Name);
			_militaryHistory.Add(curCharacter.Card.militaryPoints);
			_politicalHistory.Add(curCharacter.Card.politicalPoints);
		}

		if (lastAppliedAction is FiveRingsDeclareConflict) {
			FiveRingsDeclareConflict declareConflict = (FiveRingsDeclareConflict) lastAppliedAction;
			_battleResults.Add(new BattleResult(true, false,  declareConflict.ConflictType.ToString(), 0, 0, new string[0]));
			isInBattle = true;
		}
		
		if (lastAppliedAction is FiveRingsDeclareDefence) {
			FiveRingsDeclareDefence declareConflict = (FiveRingsDeclareDefence) lastAppliedAction;
			_battleResults.Add(new BattleResult(false, false,  (game.PhaseManager.CurrentPhase as ConflictPhase).ConflictType.ToString(), 0, 0, new string[0]));
			isInBattle = true;
		}
	}

	private void CheckEvents(GameStatus prevGameStatus, GameStatus currentGameStatus, PlayerAction lastAppliedAction, int playerIndex) {
		
		if (CharacterExsist(currentGameStatus) == false) {
			return;
		}
		
		Character curCharacter = GetCharacter(currentGameStatus);
		Character prevCharacter = GetCharacter(prevGameStatus);
		Game game = ((FiveRingsGameStatus) currentGameStatus).Game;
		
		if (isInBattle && (game.LastEvent.EventType == EventType.WonConflict || game.LastEvent.EventType == EventType.LostConflict || game.LastEvent.EventType == EventType.BrokeProvince)) {
			Player otherPlayer = game.GetPlayer((_playerIndex == 0) ? 1 : 0);
			ConflictPhase conflictPhase = (ConflictPhase) game.PhaseManager.CurrentPhase;
			
			bool won = (game.LastEvent.EventType == EventType.WonConflict || game.LastEvent.EventType == EventType.BrokeProvince);
			BattleResult battleResult = _battleResults[_battleResults.Count - 1];
			bool isMilitary = (battleResult.typeBattle == "Military");


			battleResult.wonBattle = (won == battleResult.started);
			battleResult.selfBattlePoints = int.Parse(game.LastEvent.Data[battleResult.started ? 0 : 1]);
			battleResult.otherBattlePoints = int.Parse(game.LastEvent.Data[battleResult.started ? 1 : 0]);

			battleResult.enemyCards = game.LastEvent.Data[battleResult.started ? 3 : 2].Split(',');
			
			_battleResults[_battleResults.Count - 1] = battleResult;

			isInBattle = false;
			
			
			// Store result
			FiveRingsBattleResult result = new FiveRingsBattleResult(_cardName, playerIndex, battleResult, _addedAttachments.ToArray(), _militaryHistory.ToArray(), _politicalHistory.ToArray());
			StoreResult(result);
		}


	}

	private Character GetCharacter(GameStatus gameStatus) {
		Player player = (gameStatus as FiveRingsGameStatus)?.Game.GetPlayer(_playerIndex);
		return (player?.PlayArea.Count > _areaIndex) ? player.PlayArea[_areaIndex] as Character : null;
	}

	private bool CharacterExsist(GameStatus gameStatus) {
		Character character = GetCharacter(gameStatus);
		return (character != null && character.CharacterCardName == _cardName);
	}
	
}

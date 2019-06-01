using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class ConflictPhase : BasePhase {
	
	[DataContract]
	public class PlayerHistory {
		[DataMember] public bool StartedPoliticalConflict;
		[DataMember] public bool StartedMilitaryConflict;
		
		public PlayerHistory(bool political, bool military) {
			StartedPoliticalConflict = political;
			StartedPoliticalConflict = military;
		}
	}
	
	// PlayerSide
	[DataMember] public bool AllowedToDoAction { get; private set; }
	[DataMember] public Character LastPlayedCharacter { get; private set; }	

	// Conflict
	[DataMember] public bool DeclaredConflict { get; private set; }
	[DataMember] public bool DeclaredDefence { get; private set; }
	[DataMember] public ConflictType ConflictType { get; private set; }
	[DataMember] public ElementType ElementType { get; private set; }
	[DataMember] public int[][] BattlingCharacters { get; private set; } = new[] {new int[0], new int[0]};
	[DataMember] public int AttackedProvinceIndex{ get; private set; }
	[DataMember] public int AttackingPlayerIndex{ get; private set; }
	
	public bool IsInConflict => DeclaredConflict && DeclaredDefence;
	public Province AttackedProvince => (AttackedProvinceIndex >= 0) ? DefendingPlayer.Provinces[AttackedProvinceIndex] : null;
	

	public Player DefendingPlayer => CurGame.GetPlayer((AttackingPlayerIndex == 1) ? 0 : 1);
	public Player AttackingPlayer => Game.Instance.GetPlayer(AttackingPlayerIndex);

	[DataMember] public PlayerHistory[] PlayerHistories { get; private set; } = new PlayerHistory[2];
	[DataMember] public Dictionary<ElementType, int> ElementOwner { get; private set; }

	[DataMember] private int passInConflictCount = 0;
	[DataMember] private int passCount = 0;

	
		
	public ConflictPhase(Game curGame, PhaseManager phaseManager) : base(curGame, phaseManager) {
		
	}
	
	public override void OnStartPhase() {
		base.OnStartPhase();
		
		PlayerHistories[0] = new PlayerHistory(false, false);
		PlayerHistories[1] = new PlayerHistory(false, false);
		ElementOwner = new Dictionary<ElementType, int>();
		DeclaredConflict = false;
		DeclaredDefence = false;
		AllowedToDoAction = true;
		
		CurGame.SetPlayerTurn(CurGame.GetPlayer(0));
		CurGame.EventText = "Step 1: Choice to attack or to pass";
	}

	public override void OnResumePhase(Game game) {
		base.OnResumePhase(game);
	}

	public override void OnGameUpdated(ChangeEvent changeEvent) {
		switch (changeEvent.EventType) {
			case EventType.EndTurn:
				
				if (DeclaredConflict && DeclaredDefence == false && changeEvent.ChangedPlayer.Index != AttackingPlayerIndex) {
					DeclaredDefence = true;
					AllowedToDoAction = true;
				}else if (IsInConflict) {
					if (AllowedToDoAction) {
						passInConflictCount++;

						if (passInConflictCount >= 2) {
							EndedConflict();
						}
					} else {
						passInConflictCount = 0;
					}
				} else {
					passCount = (AllowedToDoAction) ? passCount + 1 : 0;

					if (passCount >= 2) {
						CurGame.PhaseManager.NextPhase();
					}
				}
				
				
				AllowedToDoAction = true;
				LastPlayedCharacter = null;
				
								
				break;
			case EventType.AddAttachment:
				AllowedToDoAction = false;
				break;
			
				
		}
		
	}

	public void DeclareConflict(Player attackingPlayer, ConflictType conflictType, ElementType elementType, Character[] attackingCharacters, Province attackedProvince) {
		DeclaredConflict = true;
		AttackingPlayerIndex = attackingPlayer.Index;
		ConflictType = conflictType;
		ElementType = elementType;
		AttackedProvinceIndex = DefendingPlayer.Provinces.ToList().IndexOf(attackedProvince);
		BattlingCharacters[attackingPlayer.Index] = attackingCharacters.Select(c => attackingPlayer.PlayArea.IndexOf(c)).ToArray();
		DeclaredDefence = false;
		AllowedToDoAction = false;
		
		CurGame.EventText = "Step 2: Declare defenders or not";
	}

	public void DeclareDefence(Player player, Character[] characters) {
		BattlingCharacters[player.Index] = characters.Select(c => player.PlayArea.IndexOf(c)).ToArray();
		DeclaredDefence = true;
		
		CurGame.EventText = "Step 3: Conflicting and defending";
		AllowedToDoAction = true;
		LastPlayedCharacter = null;
		CurGame.SetPlayerTurn(DefendingPlayer);
	}

	public void CharacterPlaced(Character character, Player player, bool inBattle) {
		AllowedToDoAction = false;
		LastPlayedCharacter = character;

		if (inBattle) {
			// TODO: SHAME TO ME!!
			List<int> characters = BattlingCharacters[player.Index].ToList();
			characters.Add(player.PlayArea.IndexOf(character));
			BattlingCharacters[player.Index] = characters.ToArray();
		}
	}

	private void EndedConflict() {

		int defendingPlayer = (AttackingPlayerIndex == 0) ? 1 : 0;
		
		int attackingPoints = (ConflictType == ConflictType.Political) ? 
			BattlingCharacters[AttackingPlayerIndex].Select(c => AttackingPlayer.PlayArea[c].As<Character>()).Sum(c => c.GetTotalPoliticalPoints()) : 
			BattlingCharacters[AttackingPlayerIndex].Select(c => AttackingPlayer.PlayArea[c].As<Character>()).Sum(c => c.GetTotalMilitaryPoints());
		
		int defendingPoints = (ConflictType == ConflictType.Political) ? 
			BattlingCharacters[defendingPlayer].Select(c => DefendingPlayer.PlayArea[c].As<Character>()).Sum(c => c.GetTotalPoliticalPoints()) : 
			BattlingCharacters[defendingPlayer].Select(c => DefendingPlayer.PlayArea[c].As<Character>()).Sum(c => c.GetTotalMilitaryPoints());

		bool attackingWon = (attackingPoints >= defendingPoints);
		bool brokeProvince = (attackingWon && attackingPoints >= AttackedProvince.GetTotalStrength());
		
		string attackingList = String.Join(",", BattlingCharacters[AttackingPlayerIndex].Select(i => Game.Instance.GetPlayer(AttackingPlayerIndex).PlayArea[i].As<Character>().Card.Name).ToArray());
		string defendingList = String.Join(",", BattlingCharacters[defendingPlayer].Select(i => Game.Instance.GetPlayer(defendingPlayer).PlayArea[i].As<Character>().Card.Name).ToArray());
		
		// return and bow all characters
		for (int i = 0; i < BattlingCharacters.Length; i++) {
			for (int j = 0; j < BattlingCharacters[i].Length; j++) {
				((Character) Game.Instance.GetPlayer(i).PlayArea[j]).Bowed = true;
			}
			BattlingCharacters[i] = new int[0];
		}
		
		// destroyed province?
		if (brokeProvince) {
			AttackedProvince.Destroyed();
		}
		
		
		CurGame.EventText = "attacking: " + attackingPoints + " Defending: " + defendingPoints + " attack won?" + attackingWon + " province is broken: " + brokeProvince;

		AttackedProvinceIndex = -1;
		DeclaredConflict = false;
		ConflictType = ConflictType.None;
		
		CurGame.SetPlayerTurn(DefendingPlayer);
		
		if (brokeProvince) {
			CurGame.ApplyChanges(ChangeEvent.Create(EventType.BrokeProvince, AttackedProvince, AttackingPlayer, new string[]{attackingPoints.ToString(), defendingPoints.ToString(), attackingList, defendingList}));
		} else if (attackingWon){
			CurGame.ApplyChanges(ChangeEvent.Create(EventType.WonConflict, AttackedProvince, AttackingPlayer, new string[]{attackingPoints.ToString(), defendingPoints.ToString(), attackingList, defendingList}));
		} else {
			CurGame.ApplyChanges(ChangeEvent.Create(EventType.LostConflict, AttackedProvince, AttackingPlayer, new string[]{attackingPoints.ToString(), defendingPoints.ToString(), attackingList, defendingList}));
		}
		
	}
	
	public Character GetAttackingCharacters(int playerIndex, int cardIndex) {
		return Game.Instance.GetPlayer(playerIndex).PlayArea[BattlingCharacters[playerIndex][cardIndex]] as Character;
	}
}
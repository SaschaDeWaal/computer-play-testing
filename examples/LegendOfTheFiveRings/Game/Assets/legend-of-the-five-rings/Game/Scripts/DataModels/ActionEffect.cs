using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class ActionEffect {

	public Type CardViewType {
		get {
			switch (CardPosition) {
				case CardType.PlayArea:
					return typeof(CharacterInPlayView);
				
				case CardType.Hand:
					return typeof(HandPlayerObjectView);
				
				case CardType.Province:
					return typeof(ProvincePlayerObjectView);
				
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
	
	public enum ActionType {
		PlayerValueChange,
		CardValueChange
	}
	
	public enum PlayerSide {
		Player1,
		Player2,
		Self,
		Other,
	}
	
	public enum PlayerNumberToChange {
		FatePool,
		HonorPool,
		Hand,
	}
	
	public enum CardType {
		PlayArea,
		Hand,
		Province
	}
	
	public enum CardToChange {
		Remove,
		Bow,
	}
	
	public enum SumChange {
		Add,
		Remove,
		Set,
		Multiply,
	}

	public ActionType Action;
	
	[ConditionalField("Action", ActionType.PlayerValueChange)]
	public PlayerSide Side;

	[ConditionalField("Action", ActionType.PlayerValueChange)]
	public PlayerNumberToChange PlayerChange;
	
	[ConditionalField("Action", ActionType.PlayerValueChange)]
	public SumChange Change;
	
	[ConditionalField("Action", ActionType.PlayerValueChange)]
	public float Number;
	
	[ConditionalField("Action", ActionType.CardValueChange)]
	public CardType CardPosition;
	
	[ConditionalField("Action", ActionType.CardValueChange)]
	public CardToChange CardChange;

	public bool Apply(Game game, Player player, Card card = null) {
		switch (Action) {
			case ActionType.PlayerValueChange:
				return ApplyPlayerValue(game, player);
			case ActionType.CardValueChange:
				return ApplyCardValue(game, player, card);
		}

		return false;
	}

	private bool ApplyPlayerValue(Game game, Player player) {
		Player targetPlayer = TargetPlayer(game, player);
		
		switch (PlayerChange) {
			case PlayerNumberToChange.FatePool:
				targetPlayer.FatePool = Mathf.RoundToInt(CalculateNewNumber(targetPlayer.FatePool));
				return true;
			
			case PlayerNumberToChange.HonorPool:
				targetPlayer.HonorPool = Mathf.RoundToInt(CalculateNewNumber(targetPlayer.HonorPool));
				return true;
			
			case PlayerNumberToChange.Hand:
				if (Change == SumChange.Add) targetPlayer.DrawCards(Mathf.RoundToInt(Number));
				if (Change == SumChange.Remove) targetPlayer.RemoveFromHand(Mathf.RoundToInt(Number));
				if (Change == SumChange.Set) {
					targetPlayer.RemoveFromHand(targetPlayer.Hand.Count);
					targetPlayer.DrawCards(Mathf.RoundToInt(Number));
				}

				if (Change == SumChange.Multiply) {
					targetPlayer.DrawCards(Mathf.RoundToInt(Number * targetPlayer.Hand.Count));
				}
				return true;
				
			default:
				throw new ArgumentOutOfRangeException();
		}

		return false;
	}
	
	private bool ApplyCardValue(Game game, Player player, Card card) {
		
		switch (CardChange) {
			case CardToChange.Remove:
				if (CardPosition == CardType.PlayArea) {
					Character character = (card as Character);
					
					if (character.Card.Deck == CharacterCard.DeckTypes.Dynasty) {
						character.Owner.DynastyDiscard.Add(character);
					} else {
						character.Owner.ConflictDiscard.Add(character);
					}

					character.Owner.PlayArea.Remove(character);
					return true;
				}

				if (CardPosition == CardType.Province) {
					Province province = (card as Province);

					if (province.DynastyCard != null) {
						player.DynastyDiscard.Add(province.DynastyCard);
						province.RemoveDynastyCard();
						return true;
					}
				}
				
				break;
			case CardToChange.Bow:
				if (CardPosition == CardType.PlayArea) {
					(card as Character).Bowed = true;
					return true;
				}
				
				break;
			
			default:
				throw new ArgumentOutOfRangeException();
		}

		return false;
	}

	private Player TargetPlayer(Game game, Player player) {
		switch (Side) {
			case PlayerSide.Player1:
				return game.GetPlayer(0);
			case PlayerSide.Player2:
				return game.GetPlayer(1);
			case PlayerSide.Self:
				return player;
			case PlayerSide.Other:
				return game.GetPlayer(player.Index == 0 ? 1 : 0);
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private float CalculateNewNumber(float value) {
		switch (Change) {
			case SumChange.Add:
				return value + Number;
			case SumChange.Remove:
				return value - value;
			case SumChange.Set:
				return value;
			case SumChange.Multiply:
				return value * Number;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
}

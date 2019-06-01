using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "NewCharacterCard", menuName = "LegendOfFiveRings/Cards/Character")]
public class CharacterCard : CardData {
	
	public enum EventAbility {
		Reaction,
		ForcedReaction,
		Action,
	}
	
	public enum DeckTypes {
		Dynasty,
		Conflict
	}
	
	[System.Flags]
	public enum CharacterTraits {
		None = 0,
		Bushi = 1,
		Berserker = 2,
		Countier = 4,
		Imperial = 8,
		Shugenja = 16,
		Water = 32,
		Earth = 64,
		Robin = 128,
		Scholar = 256,
		Champin = 512,
		Commander = 1024
	}
	
	public string Name = "NewEvent";
	public Sprite Icon;
	public DeckTypes Deck;
	public Clan clan;
	public int cost;

	[Header("Default")]
	public int militaryPoints = -1;
	public int politicalPoints = -1;
	public int gloryPoints = 0;
	[EnumFlag] public CharacterTraits characterTraits;

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class Player {
	[DataMember] public List<Card> ConflictDeck { get; private set; }
	[DataMember] public List<Card> DynastyDeck { get; private set; }
	[DataMember] public List<Card> ConflictDiscard { get; private set; }
	[DataMember] public List<Card> DynastyDiscard { get; private set; }

	[DataMember] public Stronghold Stronghold { get; private set; }
	[DataMember] public Province[] Provinces { get; private set; } = new Province[5];

	[DataMember] public List<Card> Hand { get; private set; }
	[DataMember] public List<Card> PlayArea { get; private set; }
	[DataMember] public int Index { get; private set; }

	[DataMember] public int FatePool;
	[DataMember] public int HonorPool;
	private Game _game;

	public Player(Game game, int index) {
		_game = game;
		Index = index;
	}


	public void SetStartValues(bool isFirstPlayer, PlayerStartValues startValues) {

		// 0. Prepare code stuff
		PlayArea = new List<Card>();
		
		// 1. Select deck
		ConflictDeck = CardHelper.CreateCards(startValues.conflictDeck, this).ToList();
		DynastyDeck  = CardHelper.CreateCards(startValues.dynastyDeck, this).ToList();
		ConflictDiscard = new List<Card>();
		DynastyDiscard  = new List<Card>();
		
		// 2. Create tokens pool and ring rool
		FatePool = 0;
		
		// 3. Determine First playerSide
		if (!isFirstPlayer) {
			FatePool += 1;
		}
		
		// 4. Shuffle Dynasty and Conflict decks
		ConflictDeck = CardHelper.ShuffleCards(ConflictDeck);
		DynastyDeck  = CardHelper.ShuffleCards(DynastyDeck);

		// 5. Place provinces and stronghold
		Provinces  = CardHelper.CreateCards(startValues.provinceCards, this).Select(x => (Province)x).ToArray();
		Stronghold = (Stronghold)CardHelper.CreateCard(startValues.strongholdCard, this);
	
		// 6. Fill provinces
		FillProvince();
		Provinces[Provinces.Length -1].FillProvince(Stronghold);
		Provinces[Provinces.Length -1].RevealDynastyCard();
		
		// 7. Draw starting hand
		Hand = new List<Card>();
		DrawCards(4);
		
		// 8. Gain starting honor
		HonorPool  = Stronghold.Card.StartingHonor;
	}

	public void DrawCards(int amount) {
		for (int i = 0; i < amount; i++) {

			if (!ConflictDeck.Any()) {
				ConflictDeck = CardHelper.ShuffleCards(ConflictDiscard);
				ConflictDiscard.Clear();
			}

			if (ConflictDeck.Any()) {
				Hand.Add(ConflictDeck[0]);
				ConflictDeck.RemoveAt(0);
			}
		}
	}
	
	public void RemoveFromHand(int amount) {
		for (int i = 0; i < Mathf.Min(amount, Hand.Count); i++) {
			ConflictDeck.Add(Hand[0]);
			Hand.RemoveAt(0);
		}
	}

	public void FillProvince() {
		for (int i = 0; i < 4; i++) {
			if (Provinces[i].DynastyCard == null) {
				
				if (!DynastyDeck.Any()) {
					DynastyDeck = CardHelper.ShuffleCards(DynastyDiscard);
					DynastyDiscard.Clear();
				}
				
				Provinces[i].FillProvince(DynastyDeck[0]);
				DynastyDeck.RemoveAt(0);
			}
		}
	}

	public void CollectFate() {
		// TODO: There are more values
		FatePool += Stronghold.Card.FateValue;
	}

	public void PlayProvinces(Province province) {
		if (province.CardCanBePlayed) {
			FatePool -= province.DynastyCard.As<Character>().Card.cost;
			PlayArea.Add(province.DynastyCard);
			province.RemoveDynastyCard();
		}
	}
	
	public static bool operator == (Player a, Player b) {
		return a?.Index == b?.Index;
	}

	public static bool operator != (Player a, Player b) {
		return a?.Index != b?.Index;
	}
}

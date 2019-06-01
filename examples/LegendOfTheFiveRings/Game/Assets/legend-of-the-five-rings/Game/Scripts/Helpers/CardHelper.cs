using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class CardHelper {
	
	public static List<Card> ShuffleCards(List<Card> cards) {
		return cards.OrderBy(c => Random.Range(0, 1000)).ToList();
	}

	public static Card[] CreateCards(CardData[] templates, Player owner) {
		Card[] cards = new Card[templates.Length];
		for (int i = 0; i < cards.Length; i++) {
			cards[i] = CreateCard(templates[i], owner);
		}

		return cards;
	}

	public static Card CreateCard(CardData template, Player owner) {

		Type type = template.GetType();

		if (type == typeof(ConflictAttachmentCard)) {
			return new Attachment((ConflictAttachmentCard)template);

		}else if (type == typeof(CharacterCard)) {
			return new Character((CharacterCard)template, owner);
			
		}else if (type == typeof(ConflictEventCard)) {
			return new ConflictEvent((ConflictEventCard) template);

		}else if (type == typeof(ProvinceCard)) {
			return new Province((ProvinceCard) template, owner);
			
		}else if (type == typeof(DynastyHoldingCard)) {
			return new Holding((DynastyHoldingCard) template);
			
		}else if (type == typeof(StrongholdCard)) {
			return new Stronghold((StrongholdCard) template);
			
		} else {
			Debug.LogError("CardData type not found: " + type);
		}

		return null;

	} 
	
	
}

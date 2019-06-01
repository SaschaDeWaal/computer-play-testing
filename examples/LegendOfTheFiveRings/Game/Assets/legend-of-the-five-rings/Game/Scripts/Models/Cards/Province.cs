using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class Province: Card {

	public ProvinceCard Card { get; private set; }
	
	[DataMember] public string ProvinceCardName {
		get => Card?.name;
		set => Card = ScriptableObjectsDatabase.Instance.FindByName<ProvinceCard>(value);
	}
	
	[DataMember] public bool DynastyCardFaceDown { get; private set; }
	[DataMember] public bool FaceDown { get; private set; }
	[DataMember] public bool Standing { get; private set; }
	public Player Owner { get; private set; }
	
	[DataMember] public int OwnerIndex {
		get => Owner.Index;
		set => Owner = Game.Instance.GetPlayer(value);
	}

	public bool CardCanBePlayed => (DynastyCard != null && DynastyCardFaceDown && DynastyCard.Is<Character>());
	[DataMember] public Card DynastyCard { get; private set; }
	
	public override Sprite CardSprite => Card?.Icon;

	public Province(ProvinceCard provinceCard, Player owner) {
		Card = provinceCard;
		FaceDown = false;
		Standing = true;
		Owner = owner;
	}

	public void FillProvince(Card card) {
		DynastyCard = card;
		DynastyCardFaceDown = false;
	}

	public void RevealDynastyCard() {
		DynastyCardFaceDown = true;
	}

	public void RemoveDynastyCard() {
		DynastyCard = null;
	}

	public void OpenProvince() {
		FaceDown = true;
	}

	public void Destroyed() {
		Standing = false;

		if (DynastyCard != null) {
			Owner.DynastyDiscard.Add(DynastyCard);
			RemoveDynastyCard();
			FillProvince(Owner.DynastyDeck[0]);

			if (Owner.DynastyDeck.Count > 0) {
				Owner.DynastyDeck.RemoveAt(0);
			}
		}
	}

	public int GetTotalStrength() {
		int stronghold = ((DynastyCard != null && DynastyCard.Is<Stronghold>())? DynastyCard.As<Stronghold>().Card.BonusStrength: 0);
		int holding = ((DynastyCard != null && DynastyCard.Is<Holding>()) ? DynastyCard.As<Holding>().Card.bonusStrength: 0);
		return Card.Strength + stronghold + holding;
	}

	public bool AllowedToAttack() {
		return Standing && (Owner.Provinces[4] != this || Owner.Provinces.Count(p => p.Standing == false) >= 3);

	}
}


using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]

public class Character: Card {

	public CharacterCard Card { get; private set; }
	public Player Owner { get; private set; }
	[DataMember] public int OwnerIndex {
		get => Owner.Index;
		set => Owner = Game.Instance.GetPlayer(value);
	}
	[DataMember] public bool Bowed { get; set; }
	[DataMember] public int FatePoints = 0;
	[DataMember] public List<Attachment> Attachment { get; private set;  }
	
	[DataMember] public string CharacterCardName {
		get => Card?.Name;
		set => Card = ScriptableObjectsDatabase.Instance.FindByName<CharacterCard>(value);
	}
	
	public override Sprite CardSprite => Card?.Icon;

	public Character(CharacterCard characterCard, Player owner) {
		Card = characterCard;
		Owner = owner;
		Bowed = false;
		Attachment = new List<Attachment>();
	}

	public void AddAttachment(Attachment attachment) {
		Attachment.Add(attachment);
	}

	public void DiscardCard() {
		Attachment = new List<Attachment>();
		Bowed = false;
	}
	
	public int GetTotalPoliticalPoints() {
		return  Card.politicalPoints + Attachment.Sum(a => a.CardData.politicalBonus);
	}
	
	public int GetTotalMilitaryPoints() {
		return  Card.militaryPoints + Attachment.Sum(a => a.CardData.militaryBonus);
	}
	
}

using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class Holding: Card {

	public DynastyHoldingCard Card { get; private set; }
	
	[DataMember] public string DynastyHoldingCardName {
		get => Card?.Name;
		set => Card = ScriptableObjectsDatabase.Instance.FindByName<DynastyHoldingCard>(value);
	}
	
	public override Sprite CardSprite => Card?.Icon;

	public Holding(DynastyHoldingCard dynastyHoldingCard) {
		Card = dynastyHoldingCard;
	}
}

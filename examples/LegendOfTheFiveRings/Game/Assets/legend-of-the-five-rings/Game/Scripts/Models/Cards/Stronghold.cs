using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class Stronghold: Card {
	

	public StrongholdCard Card { get; private set; }

	[DataMember] public string StrongholdCardName {
		get => Card?.Name;
		set => Card = ScriptableObjectsDatabase.Instance.FindByName<StrongholdCard>(value);
	}
	
	public override Sprite CardSprite => Card.Icon;
	

	public Stronghold(StrongholdCard strongholdCard) {
		Card = strongholdCard;
	}
}

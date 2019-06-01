
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class ConflictEvent : Card {
	public ConflictEventCard Card { get; private set; }
	
	[DataMember] public string ConflictEventCardName {
		get => Card?.Name;
		set => Card = ScriptableObjectsDatabase.Instance.FindByName<ConflictEventCard>(value);
	}
	
	public override Sprite CardSprite => Card?.Icon;

	public ConflictEvent(ConflictEventCard conflictEventCard) {
		Card = conflictEventCard;
	}
}

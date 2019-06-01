using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class Attachment: Card {

	public ConflictAttachmentCard CardData { get; private set;  }
	
	public override Sprite CardSprite => CardData?.Icon;
	
	[DataMember] public string ConflictAttachmentCardName {
		get => CardData?.name;
		set => CardData = ScriptableObjectsDatabase.Instance.FindByName<ConflictAttachmentCard>(value);
	}

	public Attachment(ConflictAttachmentCard conflictAttachmentCardData) {
		CardData = conflictAttachmentCardData;
	}

}

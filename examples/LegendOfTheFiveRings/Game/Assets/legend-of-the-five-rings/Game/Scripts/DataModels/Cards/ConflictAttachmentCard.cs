
using System.Runtime.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttachment", menuName = "LegendOfFiveRings/Cards/Conflict/Attachment")]
public class ConflictAttachmentCard: CardData {
	
	[Header("Default")]
	public string Name = "NewAttachment";
	public Sprite Icon;
	public Clan clan;
	public int cost;

	[Header("Bonus")] 
	public int militaryBonus = 0;
	public int politicalBonus = 0;
	
	[Header("Traits")]
	[EnumFlag] public CharacterCard.CharacterTraits characterTraits;
	
}

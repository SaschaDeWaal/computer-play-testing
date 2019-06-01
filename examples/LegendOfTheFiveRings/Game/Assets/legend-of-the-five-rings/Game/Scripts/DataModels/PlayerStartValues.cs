
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerStartValues", menuName = "LegendOfFiveRings/PlayerStartValues")]
public class PlayerStartValues: ScriptableObject {

	[Header("Deck and cards")] 
	public Clan clan;
	
	[Header("Deck and cards")] 
	public CardData[] dynastyDeck;
	public CardData[] conflictDeck;
	public ProvinceCard[] provinceCards = new ProvinceCard[5];
	public StrongholdCard strongholdCard;

	
}

using System.Runtime.Serialization;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "NewHolding", menuName = "LegendOfFiveRings/Cards/Dynasty/Holding")]
public class DynastyHoldingCard: CardData {

	public enum Traits {
		Battlefield,
		Imperial
	}
	
	public string Name = "NewHolding";
	public Sprite Icon;
	public Clan clan;

	[Header("Stats")]
	public int bonusStrength = 1;
	public Traits traits;
	
}

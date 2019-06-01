using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStrongholdCard", menuName = "LegendOfFiveRings/Cards/Stronghold")]
public class StrongholdCard : CardData {

	[Header("Default")] public string Name = "";
	public Sprite Icon;
	public Clan Clan;

	[Header("Values")] public int BonusStrength = -1;

	public int StartingHonor = 0;
	public int FateValue = 7;
	public int InfluenceValue = 10;

}

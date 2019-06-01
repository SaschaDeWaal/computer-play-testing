using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public struct Player {

	[DataMember] public int Score;
	[DataMember] public List<BaseWeapon> WeaponsInHand;
	[DataMember] public List<BaseWeapon> WeaponsInPlay;

	public Player(List<BaseWeapon> startCards) {
		WeaponsInHand = startCards;
		Score = 0;
		WeaponsInPlay = new List<BaseWeapon>();
	}

}

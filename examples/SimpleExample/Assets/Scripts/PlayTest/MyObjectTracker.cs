using System.Runtime.Serialization;
using ComputerPlayTesting;

[DataContract]
public class MyObjectTracker: PlayTestObjectTracker {

	[DataMember] private int ownerIndex;
	[DataMember] private string weaponName;
	
	[DataMember] private int playedBattles;
	[DataMember] private int wonBattles;
	
	public MyObjectTracker(int owner, string name) {
		ownerIndex = owner;
		weaponName = name;
	}

	public override void OnTick(GameStatus gameStatus) {

		Game game = ((MyGameStatus) gameStatus).Game;
		BaseWeapon weapon = GetTrackingWeapon(game);

		if (weapon == null) {
			// Weapon not found, this means it's life has ended. We can store the results and destroy this tracker
			StoreResult(new MyWeaponResult(weaponName, playedBattles, wonBattles));
			Destroy();

		} else {
			// Every turn we do a battle. So let's count them
			playedBattles++;

			if (game.wonState == ((ownerIndex == 0) ? WonState.Player1 : WonState.Player2)) {
				wonBattles++;
			}
		}
	}


	// We are storing indexes instead of the object itself. This functions will return the object we are tracking.
	private BaseWeapon GetTrackingWeapon(Game game) {
		return game.Players[ownerIndex].WeaponsInPlay.Find(w => w.Name == weaponName);
	}
	
}

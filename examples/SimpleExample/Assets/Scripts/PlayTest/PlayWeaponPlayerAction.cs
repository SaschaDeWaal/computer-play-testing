using ComputerPlayTesting;

public class PlayWeaponPlayerAction : PlayerAction {

	// We want to prevent saving objects, so we store the index
	// So when the gamestatus changes, we are still able to execute this action
	private int weaponIndex = -1;
	
	// Store name for tracking
	public string WeaponName { get; private set; }

	public PlayWeaponPlayerAction(int weapon) {
		weaponIndex = weapon;
	}
	
	public override void Execute(GameStatus gameStatus, int playerIndex) {
		
		// Get the necessary variables
		Game game = ((MyGameStatus) gameStatus).Game;
		BaseWeapon weapon = game.Players[playerIndex].WeaponsInHand[weaponIndex];
		WeaponName = weapon.Name;
		
		// Execute the action
		game.ChoiceWeapon(playerIndex, weapon);
	}

	public override bool IsExecutable(GameStatus gameStatus, int playerIndex) {
		Game game = ((MyGameStatus) gameStatus).Game;
		return game.PlayerCanPlayWeapon(playerIndex);
	}

}

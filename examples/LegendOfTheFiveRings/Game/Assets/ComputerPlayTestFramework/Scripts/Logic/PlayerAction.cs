namespace ComputerPlayTesting {

	public abstract class PlayerAction{

		/**
		 * Apply the PlayerAction
		 * @param gameStatus is the game status where the action should be executed
		 * @param playerIndex is the index of the player who will execute this action
		 */
		public virtual void Execute(GameStatus gameStatus, int playerIndex) {

		}
		
		/**
		 * This checks if this action can be applied at the current state of playTestData
		 * @param gameStatus is the current gamestatus. When checking if the action is executable, you should use this data.
		 * Keep in mind that when when we execute a action, we will have a copy of the gameStatus of the checked game status.
		 */
		public virtual bool IsExecutable(GameStatus gameStatus, int playerIndex) {
			return false;
		}

		/**
		 * Get the importance of this action for the test
		 */
		public virtual int GetWeight () {
			return 1;
		}

	}
}
using System.Collections.Generic;
using System.Linq;
using Random = System.Random;

namespace ComputerPlayTesting {

	// stratagy pattern
	public abstract class PlayTester {

		/**
		 * Called when the the test starts
		 * @param gameStatus this is the first game status
		 * @param playerIndex is the index of this play tester
		 */
		public virtual void OnStartTest(GameStatus gameStatus, int playerIndex) {

		}


		/**
		 * Called before the actions will be executed. Can be treated like a start turn.
		 * @param gameStatus the current gameStatus
		 * @param playerIndex the index of player in turn
		 */
		public virtual void OnWillExecuteActions(GameStatus gameStatus, int playerIndex) {

		}

		/**
		 * Called when all actions are executed. Can be treated like a end turn.
		 * @param gameStatus the current gameStatus
		 * @param playerIndex the index of player in turn
		 */
		public virtual void OnExecutedActions(GameStatus gameStatus, int playerIndex) {

		}

		/**
		 * This function should return all player action this playtester would like to test
		 * You could give all possible PlayerAction if you want to test all combination
		 * @param gameStatus the current gameStatus
		 * @param playerIndex the index of player in turn
		 */
		public virtual List<PlayerAction> GetTestableGameActions(GameStatus gameStatus, int playerIndex) {
			return new List<PlayerAction>();
		}

		/**
		 * Removes all actions that are below a weight
		 * Can be useful to limit the amount of actions
		 * @param list is the list of all actions
		 * @param minWeight is the min weight that a action must be. When the action weight is below this, it will be removed
		 * @param shuffle is a bool, when true it will shuffle the end result.
		 */
		protected virtual List<PlayerAction> FilterOnWeight(List<PlayerAction> list, int minWeight, bool shuffle) {

			list = list.Where(pa => pa.GetWeight() >= minWeight).ToList();

			if (shuffle) {
				Random rnd = new Random();
				list = list.OrderBy(o => rnd.Next()).ToList();
			}

			return list;
		}
		
		/**
		 * This function returns the best actions by weight.
		 * @param list is the list of all actions
		 * @param amount is the amount of actions you would like to get returuned
		 * @param shuffle is a bool, when true it will shuffle the end result.
		 */
		protected virtual List<PlayerAction> BestByWeight(List<PlayerAction> list, int amount, bool shuffle) {
			list = list.OrderByDescending(o => o.GetWeight()).ToList();

			if (amount < list.Count) {
				list.RemoveRange(amount, list.Count - amount);
			}

			if (shuffle) {
				Random rnd = new Random();
				list = list.OrderBy(o => rnd.Next()).ToList();
			}
			
			

			return list;
		}

	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Random = System.Random;

namespace ComputerPlayTesting {

	public abstract class PlayTestManager<TPlayTester, TGameStatus, TObserver> where TPlayTester : PlayTester where TGameStatus : GameStatus where TObserver : PlayTestObserver {

		protected TPlayTester[] PlayTesters = null;
		protected TGameStatus GameStatus = null;
		protected TObserver PlayTestObserver = null;
		protected List<TGameStatus> QueuedGameStatus = new List<TGameStatus>();
		private int _maxGamesInMemory;
		private int _cleanGamesWhenMoreThan;
		
		protected string playtestID;
		

		public bool NeedsMoreTicks => QueuedGameStatus.Any();

		/**
		 * Called when the playtest should be prepared
		 */
		public virtual void PrepareTest(int maxGamesInMemory, int cleanGamesWhenMoreThan, string testID = "") {

			// clean variables.
			_cleanGamesWhenMoreThan = cleanGamesWhenMoreThan;
			_maxGamesInMemory = maxGamesInMemory;
			
			// Create ID
			playtestID = (testID == "") ? Guid.NewGuid().ToString() : testID;

			CreateObjects(playtestID);
			QueuedGameStatus = new List<TGameStatus>();
			
			// Validate all data.
			if (PlayTestObserver == null) {
				throw new Exception("_playTestObserver is null. Make sure this variable is set in the CreateObjects function");
			}
			
			if (GameStatus == null) {
				throw new Exception("_gameStatus is null. Make sure this variable is set in the CreateObjects function");
			}
			
			if (PlayTesters == null) {
				throw new Exception("_playTestersLogic is null. Make sure this variable is set in the CreateObjects function");
			}

			// Set data
			QueuedGameStatus.Add(GameStatus);

			for (int playerIndex = 0; playerIndex < PlayTesters.Length; playerIndex++) {
				PlayTesters[playerIndex].OnStartTest(GameStatus, playerIndex);
			}

		}

		/**
		 * Called every playtest tick from the PlayTestingComponent
		 */
		public virtual void Tick() {

			if (_cleanGamesWhenMoreThan > 0 && QueuedGameStatus.Count > _cleanGamesWhenMoreThan) {
				QueuedGameStatus = QueuedGameStatus.OrderBy(gs => gs.GetWeight()).ToList();
				QueuedGameStatus.RemoveRange(_maxGamesInMemory, QueuedGameStatus.Count - _maxGamesInMemory);
			}
						
			if (QueuedGameStatus.Any()) {
				
				int randomIndex = (new Random()).Next(0, QueuedGameStatus.Count);
				TGameStatus currentDataPoint = QueuedGameStatus[randomIndex];
				QueuedGameStatus.RemoveAt(randomIndex);

				if (currentDataPoint != null) {
					
					// Trackers
					PlayTestObserver.TickTrackers(currentDataPoint);
					
					// Save results
					SendAllQueuedResults(currentDataPoint);
					PlayTestObserver.RemoveTrackersFromStatus(currentDataPoint);
					
					// Execute all actions of players
					for (int i = 0; i < PlayTesters.Length; i++) {
						RunAllActionsOfPlayer(i, PlayTesters[i], currentDataPoint);
					}
					currentDataPoint.OnDestroy();
					
					// Observer
					PlayTestObserver.ObserveAfterActions(currentDataPoint);
					
				}

			}
		}

		/**
		 * This will stop the test and break the connection to the server
		 */
		public virtual void StopTest() {
			QueuedGameStatus.Clear();
		}

		protected abstract void CreateObjects(string testId);

		/**
		 * Execute all PlayerAction of the current game status
		 * @param playerIndex the player who will run all thsoe action
		 * @param playTesters the playtester who will generate a list of all actions that he want to test
		 * @param prevDataPoint the start point of the game status
		 */
		protected virtual void RunAllActionsOfPlayer(int playerIndex, TPlayTester playTesters, TGameStatus prevDataPoint) {

			prevDataPoint.OnWillJumpToThis();
			List<PlayerAction> playerActions = playTesters.GetTestableGameActions(prevDataPoint, playerIndex);
			prevDataPoint.OnWillJumpAway();

			if (playerActions.Any()) {
				
				playTesters.OnWillExecuteActions(prevDataPoint, playerIndex);


				// Loop thought all actions that needs to be tested
				for (int actionIndex = 0; actionIndex < playerActions.Count; actionIndex++) {
					PlayerAction playerAction = playerActions[actionIndex];

					// Copy current and set values	
					TGameStatus nextTestData = prevDataPoint.CloneObject() as TGameStatus;

					// Set Data
					nextTestData.ActionIndex = actionIndex;
					nextTestData.ActionPossible = playerActions.Count;
					nextTestData.OnWillJumpToThis();


					try {

						// Apply data
						playerAction.Execute(nextTestData, playerIndex);

						// Observer
						PlayTestObserver.ObserveAfterExecutedAction(prevDataPoint, nextTestData, playerAction,
							playerIndex);

						// prepare next
						SendAllQueuedResults(nextTestData);
						nextTestData.OnQueued();
						nextTestData.OnWillJumpAway();
						QueuedGameStatus.Add(nextTestData);
					}
					catch (InvalidCastException exp) {
						Debug.LogError(exp);
					}

				}

				playTesters.OnExecutedActions(prevDataPoint, playerIndex);

			}

		}

		protected abstract void SaveResult(string data);
		
		protected virtual void SendAllQueuedResults(GameStatus gameStatus) {
			Queue<PlayTestResult> queue = PlayTestObserver.GetAllQueuedResult(gameStatus);
			while (queue.Any()) {
				string json = queue.Dequeue().FromObjectToJson();
				SaveResult(json);
			}
		}
	}
}
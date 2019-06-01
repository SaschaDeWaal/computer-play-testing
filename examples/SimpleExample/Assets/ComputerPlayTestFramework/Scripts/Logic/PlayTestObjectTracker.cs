using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace ComputerPlayTesting {
	
	[DataContract]
	[KnownType("DerivedTypes")]
	public abstract class PlayTestObjectTracker {

		[DataMember] public bool IsActive { get; private set; } = true;
		[DataMember] public bool IsMarkedAsDestroy { get; private set; } = false;

		[DataMember] public Queue<PlayTestResult> QueuedResults { get; private set; } = new Queue<PlayTestResult>();

		public PlayTestObjectTracker() {
			
		}

		/**
		 * This is called when this tracker is created
		 * @param gameStatus is the current status of the game.
		 */
		public virtual void OnStartTracking(GameStatus gameStatus) {
			
		}


		/**
		 * This is called every tick when we are tracking the current game status
		 * @param gameStatus is the current game status that we are tracking
		 */
		public virtual void OnTick(GameStatus gameStatus) {
			
		}
		
		/**
		 * This is called when a action is applied on the current game status
		 * @param gameStatus is the current game status that we are tracking
		 */
		public virtual void OnActionExecuted(GameStatus prevGameStatus, GameStatus currentGameStatus, PlayerAction lastAppliedAction, int playerIndex) {
			
		}

		/**
		 * Set this tracker active
		 * @param state is the active state. True means tick and OnActionExecuted will be called. If false, then these won't be called.
		 */
		protected void SetActive(bool state) {
			if (IsMarkedAsDestroy == false) {
				IsActive = state;
			}
		}

		/**
		 * This will mark this tracker as destroyed. Later, the PlayTestObserver will delete it when it has time
		 */
		protected void Destroy() {
			IsMarkedAsDestroy = true;
			IsActive = false;
		}

		/**
		 * Store the result of a test. This will add it to the queue until it is sended to the server.
		 */
		protected void StoreResult(PlayTestResult playTestResult) {
			QueuedResults.Enqueue(playTestResult);
		}
		
		private static Type[] DerivedTypes() {
			return Assembly.GetExecutingAssembly().GetTypes().Where(_ => _.IsSubclassOf(typeof(PlayTestObjectTracker))).ToArray();
		}
		
	}
}
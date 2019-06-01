using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;

namespace ComputerPlayTesting {

	[DataContract]
	[KnownType("DerivedTypes")]
	/**
	 * This object contains a status in the game. It is used to load and unload moments
	 * You could add a DataMember with the game data. Set all data every time this will become relevant. But you could also use this to load and unload data from the harddisk
	 */
	public abstract class GameStatus {

		[DataMember]
		public int MadeActions { get; private set; }

		[DataMember] public int ActionIndex = 0;
		[DataMember] public int ActionPossible = 0;
		[DataMember] public List<PlayTestObjectTracker> Trackers = new List<PlayTestObjectTracker>();
		[DataMember] private string _id;
		
		public GameStatus(int madeActions = 0) {
			_id = Guid.NewGuid().ToString();
			this.MadeActions = madeActions;
		}

		/**
		 * This will be called before the data of this GameStatus will be used for testing and before the actions will be applied.
		 * You might need to set some variables, or load some stuff before we could use this state. You can do that here
		 */
		public virtual void OnWillJumpToThis() {
			
		}
		
		/**
		 * This will be called after the data is used. This object is not necessary anymore at the moment. But it is possible we need it later again
		 */
		public virtual void OnWillJumpAway() {
			
		}
		
		/**
		 * This is called when this GameState is added to the Queue. We are going to use it later, but for now it will be added to the Queue.
		 */
		public virtual void OnQueued() {
			
		}
		
		/**
		 * Called when this GameState is not needed anymore. After this call it will be destroyed.
		 */
		public virtual void OnDestroy() {
			
		}

		/**
		 * Called when this object is just copied from a other object.
		 */
		public virtual void OnCopied() {
			_id = Guid.NewGuid().ToString();
		}
		
		public virtual int GetWeight () {
			return 1;
		}


		private static Type[] DerivedTypes() {
			return Assembly.GetExecutingAssembly().GetTypes().Where(_ => _.IsSubclassOf(typeof(GameStatus))).ToArray();
		}

	}
}

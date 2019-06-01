using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ComputerPlayTesting {
    public class PlayTestObserver {

        protected Queue<PlayTestResult> QueuedResults = new Queue<PlayTestResult>();
        
        /**
         * Called when a player action is applied
         * @param prevGameStatus is the game status before the apply
         * @param currentGameStatus is the game status with the apply
         * @param lastAppliedAction is the action that is applied to the game status
         * @param playerIndex is the player index of the player who did do this action
         */
        public virtual void ObserveAfterExecutedAction(GameStatus prevGameStatus, GameStatus currentGameStatus, PlayerAction lastAppliedAction, int playerIndex) {
            currentGameStatus.Trackers.ForEach(t => {
                if (t.IsActive) {
                    t.OnActionExecuted(prevGameStatus, currentGameStatus, lastAppliedAction, playerIndex);
                }
            });
        }
        
        /**
         * Called at the end of the turn
         * @param gameStatus is the current game status
         */
        public virtual void ObserveAfterActions(GameStatus gameStatus) {
            
        }

        /**
         * This is called every playtest tick
         * @param gameStatus is the current gameStatus
         */
        public virtual void TickTrackers(GameStatus gameStatus) {
            
            RemoveTrackersFromStatus(gameStatus);

            // Tick all trackers
            gameStatus.Trackers.ForEach(t => {
                if (t.IsActive) {
                    t.OnTick(gameStatus);
                }
            });
        }

        /**
         * Returns all the Queued playtest result that are waiting to be sended to the server.
         * This will return them, and they will clear the current Qqueue.
         */
        public virtual Queue<PlayTestResult> GetAllQueuedResult(GameStatus gameStatus) {
            Queue<PlayTestResult> currentResults = new Queue<PlayTestResult>();
            
            gameStatus.Trackers.ForEach(t => {
                while (t.QueuedResults.Any()) {
                    currentResults.Enqueue(t.QueuedResults.Dequeue());
                }
            });

            while (QueuedResults.Any()) {
                currentResults.Enqueue(QueuedResults.Dequeue());
            }

            return currentResults;
        }

        /**
         * Remove all unused and mark as destroyed trackers.
         * @param gameStatus is the gameStatus where the change needs to be applied
         */
        public void RemoveTrackersFromStatus(GameStatus gameStatus) {
            List<PlayTestObjectTracker> playTestObjectTrackers = gameStatus.Trackers.FindAll(e => e.IsMarkedAsDestroy);
           
            playTestObjectTrackers.ForEach(t => {
                gameStatus.Trackers.Remove(t);
            });
        }

        /**
         * Adds a tracker to the list
         * @param gameStatus where the tracker should be located
         * @param playTestObjectTracker the tracked that should be added
         */
        protected virtual void AddTracker(GameStatus gameStatus, PlayTestObjectTracker playTestObjectTracker) {
            gameStatus.Trackers.Add(playTestObjectTracker);
            playTestObjectTracker.OnStartTracking(gameStatus);
        }
        
        /**
         * Stops the tracker
         * @param gameStatus the gamestus where the tracker should be removed
         * @param playTestObjectTracker the tracker that should be remoed
         */
        protected virtual void StopTracker(GameStatus gameStatus, PlayTestObjectTracker playTestObjectTracker) {
            gameStatus.Trackers.Remove(playTestObjectTracker);
        }
        
        /**
         * Store a playtest result. It will be added to the Queue until it is sended to the server.
         * @param playTestResult the result that should be stored
         */
        protected void StoreResult(PlayTestResult playTestResult) {
            QueuedResults.Enqueue(playTestResult);
        }
        
    }
}

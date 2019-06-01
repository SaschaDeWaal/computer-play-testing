using System;
using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;


[System.Serializable]
public class PlayCondition {
	
    public enum Triggered {
        DuringTurnOrStep,
        AfterChangeEvent
    }

    public enum ConflictType {
	    None,
	    Both,
	    Military,
	    Political,
    }
    
    public enum PlayerSide {
	    None,
	    Player1,
	    Player2,
	    Self,
	    Other
    }

    public Triggered triggered;
	
	
    // During a step or turn
    [ConditionalField("triggered", Triggered.DuringTurnOrStep)]
    public PhaseManager.GamePhase turnsOrSteps;

    [ConditionalField("turnsOrSteps", PhaseManager.GamePhase.Conflict)]
    public ConflictType duringConflictType;
    
    // After event change
    [ConditionalField("triggered", Triggered.AfterChangeEvent)]
    public EventType ChangeEventType;

    [ConditionalField("triggered", Triggered.AfterChangeEvent)]
    public PlayerSide EventPlayerSide = PlayerSide.None;
    
    public bool ConditionCheck(Game game, Player player) {
	    switch (triggered) {
		    case Triggered.DuringTurnOrStep:
			    return (game.PhaseManager.CurrentGamePhase == turnsOrSteps && (turnsOrSteps != PhaseManager.GamePhase.Conflict || SameConflictType(game)));
		    case Triggered.AfterChangeEvent:
			    return (game.LastEvent.EventType == ChangeEventType && SameSide(game, player));
		    default:
			    throw new ArgumentOutOfRangeException();
	    }
    }

    private bool SameConflictType(Game game) {
	    ConflictPhase phase = (game.PhaseManager.CurrentPhase as ConflictPhase);

	    if (phase.IsInConflict == false) {
		    return duringConflictType == ConflictType.None;
	    } else {
		    return (duringConflictType == ConflictType.Both) ||
		           (duringConflictType == ConflictType.Military && phase.ConflictType == global::ConflictType.Military) ||
		           (duringConflictType == ConflictType.Political && phase.ConflictType == global::ConflictType.Political);
	    }
    }

    private bool SameSide(Game game, Player player) {	    
	    return (EventPlayerSide == PlayerSide.None) ||
	           (game.LastEvent.ChangedPlayer.Index == 0 && EventPlayerSide == PlayerSide.Player1) ||
	           (game.LastEvent.ChangedPlayer.Index == 1 && EventPlayerSide == PlayerSide.Player2) ||
				(game.LastEvent.ChangedPlayer.Index == player.Index && EventPlayerSide == PlayerSide.Self) ||
				(game.LastEvent.ChangedPlayer.Index != player.Index && EventPlayerSide == PlayerSide.Other);


    }
}
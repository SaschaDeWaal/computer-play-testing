using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class PhaseManager {

	public enum GamePhase {
		Preparing,
		Dynasty,
		Draw,
		Conflict,
		Fate,
		Regroup
	}
	
	[DataMember] public BasePhase CurrentPhase { get; private set; }
	[DataMember] public GamePhase CurrentGamePhase { get; private set; } = GamePhase.Dynasty;

	public PhaseManager(Game game) {
		
	}

	public void Start() {
		if (CurrentPhase != null) {
			CurrentPhase.OnEndPhase();
		}

		CurrentPhase = null;
		
		switch (CurrentGamePhase) {
			case GamePhase.Dynasty:
				CurrentPhase = new DynastyPhase(Game.Instance, this);
				break;
			
			case GamePhase.Draw:
				CurrentPhase = new DrawPhase(Game.Instance, this);
				break;
			
			case GamePhase.Conflict:
				CurrentPhase = new ConflictPhase(Game.Instance, this);
				break;
			
			case GamePhase.Fate:
				CurrentPhase = new FatePhase(Game.Instance, this);
				break;
			
			case GamePhase.Regroup:
				CurrentPhase = new RegroupPhase(Game.Instance, this);
				break;
			
			default:
				Debug.Log("GamePhase not yet implemented: " + CurrentGamePhase);
				break;
				
		}
		
		if (CurrentPhase != null) {
			CurrentPhase.OnStartPhase();
		}
		
		Game.Instance.ApplyChanges(ChangeEvent.Create(EventType.PhaseChanged));
	}

	public void NextPhase() {
		switch (CurrentGamePhase) {
			case GamePhase.Dynasty:
				CurrentGamePhase = GamePhase.Draw;
				break;
			case GamePhase.Draw:
				CurrentGamePhase = GamePhase.Conflict;
				break;
			case GamePhase.Conflict:
				CurrentGamePhase = GamePhase.Fate;
				break;
			case GamePhase.Fate:
				CurrentGamePhase = GamePhase.Regroup;
				break;
			case GamePhase.Regroup:
				CurrentGamePhase = GamePhase.Dynasty;
				break;
		}

		Start();
	}

}

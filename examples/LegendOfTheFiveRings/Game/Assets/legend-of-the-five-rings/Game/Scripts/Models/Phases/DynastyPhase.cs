
using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;
[DataContract]
public class DynastyPhase: BasePhase {

	
	[DataMember] public bool AllowedToPlayCard { get; private set; } = false;

	[DataMember] public int LastPlayedCardIndex  = -2;
	[DataMember] public int LastPlayedCardOfPlayerIndex  = -2;

	[DataMember] private bool fisrtPlayerRecivedToken;
	[DataMember] private int passCount;
	[DataMember] public int _currentStep;
	
	public Card LastPlayedCard => (LastPlayedCardIndex >= 0 && LastPlayedCardIndex < CurGame.GetPlayer(LastPlayedCardOfPlayerIndex).PlayArea.Count) ? CurGame.GetPlayer(LastPlayedCardOfPlayerIndex).PlayArea[LastPlayedCardIndex] : null;

	public DynastyPhase(Game curGame, PhaseManager phaseManager) : base(curGame, phaseManager) {
		
	}

	public override void OnStartPhase() {
				
		_currentStep = 0;
		LastPlayedCardIndex = -1;
		fisrtPlayerRecivedToken = false;
		passCount = 0;
		_currentStep = 0;
		Game.GameViewInstance.StartCoroutine(RunSteps());
	}

	public override void OnResumePhase(Game game) {
		base.OnResumePhase(game);
		Game.GameViewInstance.StartCoroutine(RunSteps());
	}

	public override void OnGameUpdated(ChangeEvent changeEvent) {
		
		switch (changeEvent.EventType) {
			case EventType.PlayerCharacter:
				AllowedToPlayCard = false;
				LastPlayedCardIndex = CurGame.PlayerInTurn.PlayArea.IndexOf(changeEvent.ChangedCard) -1;
				LastPlayedCardOfPlayerIndex = CurGame.PlayerInTurn.Index;
				passCount = 0;
				break;
			
			case EventType.EndTurn:
				
				if (AllowedToPlayCard) {
					passCount++;
					if (passCount >= 2) {
						PhaseManager.NextPhase();
					}
				}
				
				if (AllowedToPlayCard && !fisrtPlayerRecivedToken) {
					fisrtPlayerRecivedToken = true;
					changeEvent.ChangedPlayer.FatePool++;
				}
				
				AllowedToPlayCard = true;
				LastPlayedCardIndex = -1;
				LastPlayedCardOfPlayerIndex = -1;

				break;
		}
		
	}
	
	private void RevealFaceDownCards(int playerIndex) {
		Player player = CurGame.GetPlayer(playerIndex);

		foreach (Province province in player.Provinces) {
			province.RevealDynastyCard();
		}
		
	}

	private IEnumerator RunSteps() {
				
		// variables
		float fastStepTime = 0.25f;
		
		// Step 1: Reveal cards
		if (_currentStep == 0) {
			Game.Instance.EventText = "Step 1: Reveal cards";
			for (int playerIndex = 0; playerIndex < 2; playerIndex++) {
				Player player = CurGame.GetPlayer(playerIndex);

				foreach (Province province in player.Provinces) {
					province.RevealDynastyCard();
					CurGame.ApplyChanges(ChangeEvent.Create(EventType.RevealCard));
					if (CurGame.AnimationEnabled) yield return new WaitForSeconds(fastStepTime);
				}
			}

			_currentStep = 1;
		}

		// step 2: Collect Fate
		if (_currentStep == 1) {
			Game.Instance.EventText = "Step 2: Collect Fate";
			for (int playerIndex = 0; playerIndex < 2; playerIndex++) {
				Player player = CurGame.GetPlayer(playerIndex);
				player.CollectFate();
				CurGame.ApplyChanges(ChangeEvent.Create(EventType.FateTokens));
				if (CurGame.AnimationEnabled) yield return new WaitForSeconds(1);
			}
			_currentStep = 2;
		}

		// step 3: Play characters from provinces
		if (_currentStep == 2) {
			Game.Instance.EventText = "Step 3: Play Characters from provinces";
			AllowedToPlayCard = true;
			CurGame.SetPlayerTurn(CurGame.GetPlayer(0));
			CurGame.ApplyChanges(ChangeEvent.Empty);
			_currentStep = 3;
		}

	}
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;

[DataContract]
public class FatePhase : BasePhase {
	
	[DataMember] private int _currentStep = 0;
	
	public FatePhase(Game curGame, PhaseManager phaseManager) : base(curGame, phaseManager) {
		
	}
	
	public override void OnStartPhase() {
		base.OnStartPhase();

		_currentStep = 0;
		CurGame.SetPlayerTurn(null);
		Game.GameViewInstance.StartCoroutine(FateAnimation());
	}
	
	public override void OnResumePhase(Game game) {
		base.OnResumePhase(game);
		Game.GameViewInstance.StartCoroutine(FateAnimation());
	}
	

	private IEnumerator FateAnimation() {

		if (_currentStep == 0) {
			CurGame.EventText = "Step 1: Remove characters without fate";
			for (int player = 0; player < 2; player++) {
				for (int i = 0; i < CurGame.GetPlayer(player).PlayArea.Count; i++) {
					Character character = CurGame.GetPlayer(player).PlayArea[i].As<Character>();

					if (character.FatePoints <= 0) {
						
						// remove Attachment
						character.Attachment.ForEach(attachment => {
							CurGame.GetPlayer(player).ConflictDiscard.Add(attachment);
						});
						character.DiscardCard();

						// remove crad
						if (character.Card.Deck == CharacterCard.DeckTypes.Dynasty) {
							CurGame.GetPlayer(player).DynastyDiscard.Add(character);
						}
						else {
							CurGame.GetPlayer(player).ConflictDiscard.Add(character);
						}

						CurGame.GetPlayer(player).PlayArea.Remove(character);

					}

					CurGame.ApplyChanges(ChangeEvent.Create(EventType.None));
					if (CurGame.AnimationEnabled)  yield return new WaitForSeconds(0.5f);
				}
			}

			_currentStep = 1;
		}


		if (_currentStep == 1) {
			CurGame.EventText = "Step 2: Remove one fate from each character";
			for (int player = 0; player < 2; player++) {
				for (int i = 0; i < CurGame.GetPlayer(player).PlayArea.Count; i++) {
					Character character = CurGame.GetPlayer(player).PlayArea[i].As<Character>();
					character.FatePoints--;

					CurGame.ApplyChanges(ChangeEvent.Create(EventType.None));
					if (CurGame.AnimationEnabled)  yield return new WaitForSeconds(0.5f);
				}
			}

			_currentStep = 2;
		}
		
		CurGame.PhaseManager.NextPhase();

	}

}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class RegroupPhase : BasePhase {
	
	[DataMember] private int _currentStep = 0;
	
	public RegroupPhase(Game curGame, PhaseManager phaseManager) : base(curGame, phaseManager) {
		
	}
	
	public override void OnStartPhase() {
		
		base.OnStartPhase();

		_currentStep = 0;
		CurGame.SetPlayerTurn(null);
		Game.GameViewInstance.StartCoroutine(RegroupAnimation());
	}
	
	public override void OnResumePhase(Game game) {
		base.OnResumePhase(game);
		Game.GameViewInstance.StartCoroutine(RegroupAnimation());
	}


	private IEnumerator RegroupAnimation() {

		if (_currentStep == 0) {
			CurGame.EventText = "Step 1: Ready each cards in play";
			for (int player = 0; player < 2; player++) {
				for (int i = 0; i < CurGame.GetPlayer(player).PlayArea.Count; i++) {
					Character character = CurGame.GetPlayer(player).PlayArea[i].As<Character>();
					character.Bowed = false;

					CurGame.ApplyChanges(ChangeEvent.Create(EventType.None));
					if (CurGame.AnimationEnabled) yield return new WaitForSeconds(0.5f);
				}
			}

			_currentStep = 1;
		}

		if (_currentStep == 1) {
			CurGame.EventText = "Step 2: Fill provinces and remove broken provinces";
			for (int player = 0; player < 2; player++) {

				for (int i = 0; i < 4; i++) {
					if (CurGame.GetPlayer(player).Provinces[i].Standing == false &&
					    CurGame.GetPlayer(player).Provinces[i].DynastyCard != null) {
						CurGame.GetPlayer(player).DynastyDiscard
							.Add(CurGame.GetPlayer(player).Provinces[i].DynastyCard);
						CurGame.GetPlayer(player).Provinces[i].RemoveDynastyCard();
					}
				}

				CurGame.GetPlayer(player).FillProvince();
				CurGame.ApplyChanges(ChangeEvent.Create(EventType.None));
				if (CurGame.AnimationEnabled) yield return new WaitForSeconds(0.5f);
			}

			_currentStep = 2;
		}

		
		CurGame.PhaseManager.NextPhase();
	}

	
}

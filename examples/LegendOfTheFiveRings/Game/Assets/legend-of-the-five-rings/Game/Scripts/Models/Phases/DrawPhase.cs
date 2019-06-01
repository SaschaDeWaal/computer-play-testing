
using System.Collections;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class DrawPhase : BasePhase {

	[DataMember] public int[] playerSelection { get; private set; } = new[] {-1, -1};

	public DrawPhase(Game curGame, PhaseManager phaseManager) : base(curGame, phaseManager) {
		
	}

	public override void OnStartPhase() {
		base.OnStartPhase();
		
		CurGame.SetPlayerTurn(null);
		CurGame.EventText = "Step 1: Each playerSide select a number on their honor dial";
	}
	
	public override void OnResumePhase(Game game) {
		base.OnResumePhase(game);
		if (playerSelection[0] > -1 && playerSelection[1] > -1) {
			Game.GameViewInstance.StartCoroutine(SelectedNumbers());
		}
	}

	public override void OnGameUpdated(ChangeEvent changeEvent) {
		base.OnGameUpdated(changeEvent);

		switch (changeEvent.EventType) {
			case EventType.SelectDial:
				if (playerSelection[0] > -1 && playerSelection[1] > -1) {
					Game.GameViewInstance.StartCoroutine(SelectedNumbers());
				}
				break;
		}
	}

	public void SetPlayerSelection(Player player, int number) {
		playerSelection[player.Index] = number;
	}

	private IEnumerator SelectedNumbers() {
		// step 2: Reveal honor points
		CurGame.EventText = "Step 2: Reveal selected numbers. PlayerSide 1:" + playerSelection[0] + " playerSide 2:" + playerSelection[1];
		if (CurGame.AnimationEnabled) yield return new WaitForSeconds(4);
		
		// step 3: Revice honor points
		bool isSame = playerSelection[0] == playerSelection[1];

		if (isSame) {
			CurGame.EventText = "Step 3: Honor points. Both players selected the same number";
		} else {
			int player1 = playerSelection[1] - playerSelection[0];
			int player2 = playerSelection[0] - playerSelection[1];

			CurGame.GetPlayer(0).HonorPool += player1;
			CurGame.GetPlayer(1).HonorPool += player2;
			CurGame.ApplyChanges(ChangeEvent.Create(EventType.HonorTokens));

			CurGame.EventText = (player1 > 0) ? "Step 3: Honor points. PlayerSide 1 will get " + player1 + " honor points of playerSide 2" : "Step 3: Honor points. PlayerSide 2 will get " + player2 + " honor points of playerSide 1" ;
		}
		
		if (CurGame.AnimationEnabled) yield return new WaitForSeconds(4);
		
		// step 4: draw
		CurGame.EventText = "Step 4: Draw card. PlayerSide 1 draws " + playerSelection[0] + " cards, playerSide 2 draws "  + playerSelection[1] + " cards";
		CurGame.GetPlayer(0).DrawCards(playerSelection[0]);
		CurGame.GetPlayer(1).DrawCards(playerSelection[1]);
		CurGame.ApplyChanges(ChangeEvent.Create(EventType.DrawCards));
		if (CurGame.AnimationEnabled) yield return new WaitForSeconds(4);
		
		CurGame.PhaseManager.NextPhase();

	}
}

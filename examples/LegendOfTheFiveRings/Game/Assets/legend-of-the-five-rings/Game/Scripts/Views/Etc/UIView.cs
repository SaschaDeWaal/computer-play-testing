using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour {

	[SerializeField] private Text turnText;
	[SerializeField] private Text phaseText;
	[SerializeField] private Text eventText;
	
	private void Start() {
		Game.OnGameUpdated += new GameUpdatedEventHandler(OnUpdated);
	}

	private void Update() {
		eventText.text = Game.Instance.EventText;
	}

	private void OnUpdated(ChangeEvent changeEvent) {
		Game game = Game.Instance;

		turnText.text = "PlayerSide: " + ((game.PlayerInTurn != null) ? game.PlayerInTurn.Index.ToString() : "none");
		phaseText.text = "Phase: " + game.PhaseManager.CurrentGamePhase.ToString();
	}
}

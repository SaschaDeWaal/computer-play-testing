using UnityEngine;
using UnityEngine.UI;

public class WinUIView : BasePlayerObjectView {

	[SerializeField] private Text _text;
	
	private void Start() {
		transform.localScale = Vector3.zero;
	}
	
	protected override void OnGameChanged(ChangeEvent changeEvent) {
		if (changeEvent.EventType == EventType.GameWon) {
			transform.localScale = Vector3.one;

			_text.text = CurGame.CurrentGameStatus == Game.GameStatus.WonByPlayer1 ? "Won by playerSide 1" : "Won by playerSide 2";
		} else {
			transform.localScale = Vector3.zero;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadyView : BasePlayerObjectView {

	private Vector3 _orginalScale;
	private void Start() {
		_orginalScale = transform.localScale;
	}

	protected override void OnGameChanged(ChangeEvent changeEvent) {
		bool playersTurn = (Game.Instance.PlayerInTurn?.Index == Owner.Index);
		transform.localScale = (playersTurn) ? _orginalScale : Vector3.zero;
	}

	public override void OnClicked() {
		Controllers.Run(new PlayerReadyTurn(Owner));
		
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedDialNumberView : BasePlayerObjectView {
    
	[SerializeField] private TextMesh textMesh;

	protected override void OnGameChanged(ChangeEvent changeEvent) {

		if (Game.Instance.PhaseManager.CurrentPhase is DrawPhase) {
			DrawPhase drawPhase = (DrawPhase)Game.Instance.PhaseManager.CurrentPhase;
			textMesh.text = (drawPhase.playerSelection[Owner.Index] > -1) ? drawPhase.playerSelection[Owner.Index].ToString() : "";
		} else {
			textMesh.text = "";
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementTokenView : BasePlayerObjectView, ISelectable {

	public ElementType Element;

	protected override void OnGameChanged(ChangeEvent changeEvent) {
		if (CurGame.PhaseManager.CurrentPhase is ConflictPhase) {
			ConflictPhase conflictPhase = CurGame.PhaseManager.CurrentPhase as ConflictPhase;

			transform.localScale = conflictPhase.ElementOwner.ContainsKey(Element) ? Vector3.zero : new Vector3(1, 0.067155f, 1);
		}
	}

	public void OnSelected() {
		transform.localScale *= 0.5f;
	}

	public void OnDeselected() {
		
	}
}

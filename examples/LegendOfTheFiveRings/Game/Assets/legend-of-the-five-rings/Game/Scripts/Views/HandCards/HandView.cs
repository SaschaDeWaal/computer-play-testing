using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HandView : BasePlayerObjectsView<HandPlayerObjectView, Card> {
	
	protected override void OnGameChanged(ChangeEvent changeEvent) {
		List<HandPlayerObjectView> characterInPlayViews = CreateList(Owner.Hand.ToArray(), new Vector3(0.8f, 0, 0));
		
		characterInPlayViews.ForEach(e => { e.SetPlayerIndex(_playerIndex); });
	}
}

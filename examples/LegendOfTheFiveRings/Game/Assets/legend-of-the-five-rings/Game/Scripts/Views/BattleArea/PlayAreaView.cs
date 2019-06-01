using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayAreaView : BasePlayerObjectsView<CharacterInPlayView, Character> {
	
	protected override void OnGameChanged(ChangeEvent changeEvent) {
		List<CharacterInPlayView> characterInPlayViews = CreateList(Owner.PlayArea.Select(c => c.As<Character>()).ToArray(), new Vector3(1f, 0, 0));
		
		characterInPlayViews.ForEach(e => { e.SetPlayerIndex(_playerIndex); });
	}
}

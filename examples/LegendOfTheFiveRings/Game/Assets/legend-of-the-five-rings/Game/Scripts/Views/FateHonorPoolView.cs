using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FateHonorPoolView : BasePlayerObjectView {
    
	private enum PoolType {
		Fate,
		Honor
	}

	[SerializeField] private PoolType _poolType;

	private TextMesh _textMesh;

	protected override void OnGameStart() {
		_textMesh = GetComponent<TextMesh>();
	}

	protected override void OnGameChanged(ChangeEvent changeEvent) {
		_textMesh.text = (_poolType == PoolType.Fate) ? Owner.FatePool.ToString() : Owner.HonorPool.ToString();
	}

	public override void OnClicked() {


		if (_poolType == PoolType.Fate) {
			Controllers.Run(new PlaceFateOnCharacterFromProvince(Owner.Index));
			Controllers.Run(new PlaceFateOnCharacterFromHandController(Owner));
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using ComputerPlayTesting;
using UnityEngine;

public class ProvincePlayerObjectView : BasePlayerObjectView, ISelectable {

	[Header("Settings")]	
	[SerializeField] private int _provinceIndex = 0;
	[SerializeField] private Sprite _spriteClosed;
	[SerializeField] private Sprite _spriteDynastyClosed;
	
	[Header("Components")]	
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private SpriteRenderer _spriteRendererDynastyCard;
	[SerializeField] private GameObject _underAttackSign;

	private bool bWasFaceDown = true;
	private bool bDynastyFaceDown = true;
	private Province _province => Owner.Provinces[_provinceIndex];

	protected override void OnGameStart() {
		_underAttackSign.SetActive(false);
	}

	protected override void OnGameChanged(ChangeEvent changeEvent) {

		if (!_province.FaceDown) {
			_spriteRenderer.sprite = _spriteClosed;
			bWasFaceDown = true;
		}else if (bWasFaceDown) {
			bWasFaceDown = false;
			StopAllCoroutines();
			TurnCard(_province.Card.Icon, _spriteRenderer);
		}

		// Dynasty card
		if (_province.DynastyCard == null) {
			_spriteRendererDynastyCard.sprite = null;
		}else if (!_province.DynastyCardFaceDown) {
			_spriteRendererDynastyCard.sprite = _spriteDynastyClosed;
			bDynastyFaceDown = true;
		}else if (bDynastyFaceDown) {
			bDynastyFaceDown = false;
			TurnCard(_province.DynastyCard.CardSprite, _spriteRendererDynastyCard);
		}

		// Conflict phase
		if (CurGame.PhaseManager.CurrentPhase is ConflictPhase) {
			ConflictPhase conflictPhase = (ConflictPhase)CurGame.PhaseManager.CurrentPhase;
			bool inAttack = conflictPhase.DeclaredConflict && conflictPhase.AttackedProvince == _province;

			_underAttackSign.SetActive(inAttack);
		}
		
		// Province standing?
		_spriteRenderer.color = (_province.Standing) ? Color.white : Color.red;
	}

	public override Card GetCard() {
		return _province;
	}

	public override void OnClicked() {
		Controllers.Run(new PlayCharacterFromProvinces(Owner.Provinces.ToList().IndexOf(_province), Owner.Index));		
	}

	public void OnSelected() {
		
	}

	public void OnDeselected() {
		
	}

	private IEnumerator RotateCard() {

		while (transform.eulerAngles.x > 0) {
			transform.eulerAngles -= new Vector3(Time.deltaTime * 80, 0, 0);
			yield return null;
		}
		
		_spriteRenderer.sprite = _province.Card.Icon;
		
		while (transform.eulerAngles.x < 0) {
			transform.eulerAngles += new Vector3(Time.deltaTime * 80, 0, 0);
			yield return null;
		}
		
		
	}
}

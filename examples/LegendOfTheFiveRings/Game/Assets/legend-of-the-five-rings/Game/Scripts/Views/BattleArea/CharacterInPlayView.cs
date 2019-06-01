using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterInPlayView : BasePlayerObjectsView<AttachmentView, Attachment>, ISelectable {

	private const float MOVE_FORWARD = 1.5f;
	
	
	[SerializeField] private SpriteRenderer _sprite;
	[SerializeField] private TextMesh _textMesh;

	private Character _character;
	private float _startZ;
	
	private void Start() {
		_startZ = transform.localPosition.z;
		ViewsOffset = new Vector3(0, -2, -0.2f );
	}
	
	public override void SetCard(Card character) {
		_character = character as Character;
		_sprite.sprite = _character.Card.Icon;

		//OnGameChanged(new ChangeEvent());
	}

	public override Card GetCard() {
		return _character;
	}

	protected override void OnGameChanged(ChangeEvent changeEvent) {
		
		// Fate points
		if (_character == null || _textMesh == null) {
			return;
		}
		_textMesh.text = _character.FatePoints.ToString();
		
		
		// Conflict phase
		if (CurGame.PhaseManager.CurrentPhase is ConflictPhase) {
			ConflictPhase conflictPhase = (ConflictPhase)CurGame.PhaseManager.CurrentPhase;
			bool inAttack = conflictPhase.DeclaredConflict && conflictPhase.BattlingCharacters[Owner.Index].Contains(Owner.PlayArea.IndexOf(_character));

			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition .y, _startZ + (inAttack ? MOVE_FORWARD : 0));
		}
		
		// Bowed
		transform.eulerAngles = new Vector3(transform.eulerAngles.x, (_character.Bowed) ? 90 : 0, transform.eulerAngles.z);
		
		// Attachments
		CreateList(_character.Attachment.ToArray(), new Vector3(0, 1, 0));
	}


	public void OnSelected() {
		PlayClickAnim(_sprite);
	}

	public void OnDeselected() {
		
	}
}

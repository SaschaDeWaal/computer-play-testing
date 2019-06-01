
using UnityEngine;

public class AttachmentView : BasePlayerObjectView {

	[SerializeField] private SpriteRenderer _sprite;
	public Attachment Attachment { get; private set; }

	public override void SetCard(Card card) {
		Attachment = card as Attachment;
		_sprite.sprite = Attachment.CardData.Icon;
	}

	public override Card GetCard() {
		return Attachment;
	}
	
	protected override void OnGameChanged(ChangeEvent changeEvent) {
		
	}
}

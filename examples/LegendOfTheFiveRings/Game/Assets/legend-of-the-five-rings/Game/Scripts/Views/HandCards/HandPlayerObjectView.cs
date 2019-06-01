using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class HandPlayerObjectView : BasePlayerObjectView, IClickable {

	[SerializeField] private SpriteRenderer _spriteRenderer;

	private Card _card;

	public ConflictPhase CurPhase => (ConflictPhase) CurGame.PhaseManager.CurrentPhase;
	
	public override void SetCard(Card card) {
		_spriteRenderer.sprite = card.CardSprite;
		_card = card;
	}

	public override void OnClicked() {

		if (_card.Is<ConflictEvent>()) {
			ConflictEventCard conflictEvent = _card.As<ConflictEvent>().Card;
			bool result = conflictEvent.Conditon.ConditionCheck(CurGame, Owner);
			bool changedSomething = false;
			
			if (result) {
				if (conflictEvent.Effect.Any(e => e.Action == ActionEffect.ActionType.CardValueChange)) {					
					SelectObjectInGame.Instance.SelectOption(new Type[] {conflictEvent.Effect[0].CardViewType}, o => {
						CurGame.EventText = "Select card";
						Card card = (o as BasePlayerObjectView).GetCard();
						foreach (ActionEffect actionEffect in conflictEvent.Effect) {
							if (actionEffect.Apply(CurGame, Owner, card)) {
								changedSomething = true;
							}
						}
						
						if (changedSomething) {
							Owner.ConflictDiscard.Add(_card);
							Owner.Hand.Remove(_card);
							CurGame.ApplyChanges(ChangeEvent.Create(EventType.EventCard, Owner));
						}
					});
				}else {
					foreach (ActionEffect actionEffect in conflictEvent.Effect) {
						if (actionEffect.Apply(CurGame, Owner)) {
							changedSomething = true;
						}
					}
					
					if (changedSomething) {
						Owner.ConflictDiscard.Add(_card);
						Owner.Hand.Remove(_card);
						CurGame.ApplyChanges(ChangeEvent.Create(EventType.EventCard, Owner));
					}
				}

			} else {
				CurGame.EventText = "Can not do action at this moment";
			}
		}

		if (CurGame.PhaseManager.CurrentGamePhase != PhaseManager.GamePhase.Conflict) {
			return;
		}
		
		if (!CurPhase.DeclaredDefence && CurPhase.DeclaredConflict) {
			return;
		}

		if (_card.Is<Character>()) {
			if (CurPhase.IsInConflict) {
				QuestionPanelView.Instance.AskQuestion("Where to place it?", new[] {"Home Area", "Battle area"},i => {
					Controllers.Run(new PlayCharacterFromHandController(Owner, _card.As<Character>(), i == 1));
				});
			}else {
				Controllers.Run(new PlayCharacterFromHandController(Owner, _card.As<Character>(), false));
			}
		}

		if (_card.Is<Attachment>()) {
			SelectObjectInGame.Instance.SelectOption<CharacterInPlayView>( o => {
				Controllers.Run(new AddAttachmentToCharacter(Owner, (o as CharacterInPlayView).GetCard() as Character, _card as Attachment));
			});
		}

		

		PlayClickAnim(_spriteRenderer);


	}
}

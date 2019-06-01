using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckView : BasePlayerObjectView {

    private enum DeckType {
        DynastyDeck,
        DynastyDiscard,
        ConflictDeck,
        ConflictDiscard
    }
    
    [SerializeField] private Sprite _deckSprite;
    [SerializeField] private Sprite _emptyDeckSprite;
    [SerializeField] private DeckType _deckType = DeckType.DynastyDeck;
        
    private SpriteRenderer _spriteRenderer;
    
    protected override void OnGameStart() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void OnGameChanged(ChangeEvent changeEvent) {
        bool isNotEmpty = false;

        switch (_deckType) {
            case DeckType.DynastyDeck:
                isNotEmpty = (Owner.DynastyDeck.Any());
                break;
            case DeckType.ConflictDeck:
                isNotEmpty = (Owner.ConflictDeck.Any());
                break;
            case DeckType.DynastyDiscard:
                isNotEmpty = (Owner.DynastyDiscard.Any());
                break;
            case DeckType.ConflictDiscard:
                isNotEmpty = (Owner.ConflictDiscard.Any());
                break;
        }
        
        
        _spriteRenderer.sprite = isNotEmpty ? _deckSprite : _emptyDeckSprite;
    }
}

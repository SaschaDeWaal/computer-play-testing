using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class BasePlayerObjectView : MonoBehaviour, IClickable {

    private bool _playing = false;
    [SerializeField] protected int _playerIndex = 0;

    protected Player Owner => Game.Instance.GetPlayer(_playerIndex);
    protected Game CurGame => Game.Instance;

    private void Update() {
        if (!_playing && Game.Instance != null && Game.Instance.PhaseManager.CurrentGamePhase != PhaseManager.GamePhase.Preparing) {
            
            Game.OnGameUpdated += new GameUpdatedEventHandler(OnGameChanged);
            
            OnGameStart();
            OnGameChanged(ChangeEvent.Empty);
            _playing = true;
        }
    }

    public virtual void OnClicked() {
        
    }

    public virtual void SetCard(Card card) {
        
    }
    
    public virtual Card GetCard() {
        return null;
    }

    public virtual void SetPlayerIndex(int index) {
        _playerIndex = index;
    }


    protected virtual void OnGameStart() {
        
    }

    protected virtual void OnGameChanged(ChangeEvent changeEvent) {
        
    }

    protected void TurnCard(Sprite sprite, SpriteRenderer spriteRenderer) {
        //StopCoroutine("TurnCardAnimation");
        //StartCoroutine(TurnCardAnimation(sprite, spriteRenderer));
        
        spriteRenderer.sprite = sprite;
    }
    
    protected void PlayClickAnim(SpriteRenderer spriteRenderer) {
        StopCoroutine("ClickAnimation");
        StartCoroutine(ClickAnimation(spriteRenderer));
    }
    
    private IEnumerator TurnCardAnimation(Sprite sprite, SpriteRenderer spriteRenderer) {

        Transform rotatingTransform = spriteRenderer.transform;
        
        Quaternion start = rotatingTransform.localRotation;
        Quaternion middle = Quaternion.Euler( start.eulerAngles.x,   90 - start.eulerAngles.y,  start.eulerAngles.z);
        Quaternion end = Quaternion.Euler( start.eulerAngles.x,   180 - start.eulerAngles.y,  start.eulerAngles.z);

        Vector3 startPos = rotatingTransform.localPosition;
        Vector3 middlePos = startPos + new Vector3(0, 1, 0);
        Vector3 originalScale = rotatingTransform.localScale;
        
        float time = 0;


        while (time <= 1.0f) {
            rotatingTransform.localRotation = Quaternion.Lerp(start, middle, time);
            rotatingTransform.localPosition = Vector3.Lerp(startPos, middlePos, time);
            time += Time.deltaTime * 5;
            yield return null;
        }

        spriteRenderer.sprite = sprite;
        rotatingTransform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        time = 0;
        
        while (time <= 1.0f) {
            rotatingTransform.localRotation = Quaternion.Lerp(middle, end, time);
            rotatingTransform.localPosition = Vector3.Lerp(middlePos, startPos, time);
            time += Time.deltaTime * 5;
            yield return null;
        }
        
        rotatingTransform.localRotation = start;
        rotatingTransform.localPosition = startPos;
        rotatingTransform.localScale = originalScale;

    }

    private IEnumerator ClickAnimation(SpriteRenderer spriteRenderer) {

        float time = 0;
        
        Transform spriteTransform = spriteRenderer.transform;
        Vector3 scale = spriteTransform.localScale;

        while (time <= 1.0f) {
            spriteTransform.localScale = scale * (1.0f + (float)Math.Cos(time * 2) * 0.25f);
            
            time += Time.deltaTime * 10;
            yield return null;
        }

        spriteTransform.localScale = scale;
    }
}

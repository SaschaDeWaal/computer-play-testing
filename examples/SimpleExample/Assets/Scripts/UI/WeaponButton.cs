using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour {


    [SerializeField] private Text text;
    [SerializeField] private Text info;
    [SerializeField] private int player;

    private BaseWeapon baseWeapon;

    public void Set(BaseWeapon baseWeapon) {
        this.baseWeapon = baseWeapon;
        text.text = baseWeapon.Name;
        info.text = $"Power: {baseWeapon.Power} lifetime: {baseWeapon.GetCurrentLifeCount()}/{baseWeapon.LifeTime}";

        gameObject.SetActive(true);
    }

    public void Click() {
        Game.Instance.ChoiceWeapon(player, baseWeapon);
    }
    
}

using System.Collections.Generic;
using UnityEngine;

public class WeaponsPanel : MonoBehaviour {

    [SerializeField] private int player = 0;
    [SerializeField] private bool isHand = true;
    [SerializeField] private GameObject baseButton;

    private int oldCount = -1;

    private void Start() {
	    baseButton.SetActive(false);
    }

    private void Update() {	   
	    List<BaseWeapon> list = (isHand) ? Game.Instance.Players[player].WeaponsInHand : Game.Instance.Players[player].WeaponsInPlay;
	    
	    
	    if (list.Count != oldCount) {
		    oldCount = list.Count;
		    
		    RemoveAll();
		    list.ForEach(CreateNew);
		    
	    }
    }

    private void RemoveAll() {
	    for(int i = 0; i < transform.childCount; i++) {
		    GameObject obj = transform.GetChild(i).gameObject;

		    if (obj != baseButton) {
			    Destroy(obj);
		    }
	    }
    }

    private void CreateNew(BaseWeapon baseWeapon) {
	    GameObject obj = Instantiate(baseButton, transform);
	    obj.GetComponent<WeaponButton>().Set(baseWeapon);
    }
}

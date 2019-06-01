using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialNumberView : BasePlayerObjectView {

	[SerializeField] private int dialNumber = 0;
	
	
	public override void OnClicked() {
		Controllers.Run(new SelectNumberOnDialController(Owner, dialNumber));
	}
	
}

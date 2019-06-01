using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BasePlayerObjectsView<T, U> : BasePlayerObjectView where T : BasePlayerObjectView where U : Card{
	
	[SerializeField] private GameObject template;

	private U[] _lastList;
	private List<T> createdItems = new List<T>();
	
	protected Vector3 ViewsOffset = Vector3.zero;
	

	protected void ClearList() {		
		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild(i).gameObject.GetComponent<T>() != null) {
				Destroy(transform.GetChild(i).gameObject);
			}
		}
	}
	
	protected List<T> CreateList(U[] list, Vector3 offset) {

		if (_lastList != null && Enumerable.SequenceEqual(_lastList, list)) {
			return createdItems;
		}
		_lastList = list;
		
		ClearList();
		createdItems.Clear();
		
		for (int i = 0; i < list.Length; i++) {
			GameObject obj = GameObject.Instantiate(template, transform);

			obj.GetComponent<T>().SetCard(list[i]);
			obj.transform.localPosition = (offset * i) + ViewsOffset;
			createdItems.Add(obj.GetComponent<T>());
		}

		return createdItems;
	}
	
	
}

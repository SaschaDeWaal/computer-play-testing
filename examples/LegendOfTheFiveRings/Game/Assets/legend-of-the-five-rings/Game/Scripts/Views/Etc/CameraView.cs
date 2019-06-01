using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.NetworkSystem;

public delegate void SelectionEventHandler(ISelectable selectable);

public class CameraView : MonoBehaviour {

    [SerializeField] private bool movementEnabled = true;

    private const float ROTATE_SPEED = 20;
    
    public event SelectionEventHandler OnSelect;
    
    private Vector3 _originalRotation;
    private ISelectable _selectedItem = null;
    
    private void Start() {
        _originalRotation = transform.eulerAngles;
    }

    private void Update() {
        CameraMovement();
        ClickHandling();
    }

    private void ClickHandling() {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit)) {
                
                // Clickable
                IClickable[] clickable = hit.transform.gameObject.GetComponents<IClickable>();
                foreach (IClickable t in clickable) {
                    t.OnClicked();
                }
                

                // Selectable
                ISelectable selectable = hit.transform.gameObject.GetComponent<ISelectable>();
                if (selectable != null) {
                    _selectedItem?.OnDeselected();
                    _selectedItem = selectable;
                    _selectedItem.OnSelected();
                    
                    OnSelect?.Invoke(_selectedItem);
                }
            }
        }
    }

    private void CameraMovement() {
        if (movementEnabled) {
            transform.eulerAngles = _originalRotation +
                                    (new Vector3(((Input.mousePosition.y - (Screen.height)) / Screen.height) * -1.5f,
                                         ((Input.mousePosition.x - (Screen.width * 0.5f)) / Screen.width)) *
                                     ROTATE_SPEED);
        }

    }
}

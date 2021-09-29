using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Sc_PlacementObject : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler {
    private bool placed = false;
    public GameObject notPlacedIndicator;
	private float target = 0;
	private float targetz = 0;
	private bool touching = false;
	public Camera cameraPersp;

	private void OnEnable() {
        placed = false;
        notPlacedIndicator.SetActive(true);
    }

    private void FixedUpdate() {
		if (touching) {
			Vector3 temPos = this.transform.position;
			this.transform.position = new Vector3(target, 0, targetz);
		}
	}

    public void FinishedPlacement() {
        placed = true;
        notPlacedIndicator.SetActive(false);
    }

	//Detectores de Touch
	#region IPointerDownHandler	implementation
	public void OnPointerDown(PointerEventData data) {
		touching = true;
		Ray ray = cameraPersp.ScreenPointToRay(data.position);
		//Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
		Plane xy = new Plane(Vector3.up, new Vector3(0, 0, 0));
		float distance;
		xy.Raycast(ray, out distance);
		Vector3 perspPos = ray.GetPoint(distance);
		target = perspPos.x;
		targetz = perspPos.z;
	}
	#endregion


	#region IDragHandler implementation
	public void OnDrag(PointerEventData data) {
		Ray ray = cameraPersp.ScreenPointToRay(data.position);
		Plane xy = new Plane(Vector3.up, new Vector3(0, 0, 0));
		float distance;
		xy.Raycast(ray, out distance);
		Vector3 perspPos = ray.GetPoint(distance);
		target = perspPos.x;
		targetz = perspPos.z;

	}
	#endregion

	#region IPointerUpHandler implementation
	public void OnPointerUp(PointerEventData data) {
		touching = false;
	}
	#endregion
}

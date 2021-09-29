//This class handles the touch and mouse controls for the beam attack
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Sc_TouchManager : MonoBehaviour,IPointerDownHandler, IDragHandler, IPointerUpHandler {

	private float speed = 0;
	private bool touch = false;
	private bool touching =  false;
	private float target;
	public float targetz;
	private float aceleracion = 300;
	private float[] bordes = new float[4]; //izquierda, derecha, abajo, arriba
	public GameObject objectToMove;
	private Animator objectToMoveAnim;
	public Camera cameraPersp;
	public bool drag;

	public GameObject RayRenderer;
	public GameObject RayTarget;

	public AudioSource bounceSound;
    // Use this for initialization
	
	// Update is called once per frame
	void Update () {
		if (drag) {
			if (touching) {
				Vector3 temPos = objectToMove.transform.position;
				objectToMove.transform.position = new Vector3(IncrementTowards(temPos.x, target, aceleracion), 0, IncrementTowards(temPos.z, targetz, aceleracion));
			}
		} 

	}
	

	//Increase n towards target by speed
	private float IncrementTowards (float n, float meta, float a){
		if(n==meta){
			return n;
		}
		else{
			float dir = Mathf.Sign(meta -n);//must n be increased or decreased to get cloaser to target
			n += a * Time.deltaTime * dir;
			return (dir==Mathf.Sign(meta-n))? n: meta;	//if n has now passed target then return target, otherwise return n
		}
	}

		


	//Detectores de Touch
	#region IPointerDownHandler	implementation
	public void OnPointerDown (PointerEventData data){
		if(drag){
			touching = true;
            bounceSound.Play();
			RayRenderer.SetActive(true);
			RayTarget.SetActive(true);
			//objectToMoveAnim.SetBool ("Camina", true);
			//shadowAnim.SetBool ("Camina", true);
			Ray ray = cameraPersp.ScreenPointToRay(data.position);
			//Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
			Plane xy = new Plane(Vector3.up, new Vector3(0, 0, 0));
			float distance;
			xy.Raycast(ray, out distance);
			Vector3 perspPos =  ray.GetPoint(distance);
			target = perspPos.x;
			targetz = perspPos.z;
		}
	}
	#endregion


	#region IDragHandler implementation
	public void OnDrag (PointerEventData data){
		if (drag) {
			Ray ray = cameraPersp.ScreenPointToRay (data.position);
			Plane xy = new Plane (Vector3.up, new Vector3 (0, 0, 0));
			float distance;
			xy.Raycast (ray, out distance);
			Vector3 perspPos = ray.GetPoint (distance);
			target = perspPos.x;
			targetz = perspPos.z;

		}
	}
	#endregion

	#region IPointerUpHandler implementation
	public void OnPointerUp (PointerEventData data){
		if(drag){
			touching = false;
            bounceSound.Stop();
			//objectToMoveAnim.SetBool ("Camina", false);
			//shadowAnim.SetBool ("Camina", false);
			//desactivar animacion
			RayRenderer.SetActive(false);
			RayTarget.SetActive(false);
		}
	}
	#endregion

}

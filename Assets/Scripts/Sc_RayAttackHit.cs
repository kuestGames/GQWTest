using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_RayAttackHit : MonoBehaviour{  

    public void OnTriggerEnter(Collider coll) {
        if (coll.tag == "Enemy") {
            Vector3 position = coll.transform.position;
            GameObject clon = Sc_GameManager.gameManager.explosionPool.GetObj();
            if (clon == null) {
                return;
            }
            clon.transform.position = position;
            clon.transform.localScale = new Vector3(6 , 6, 6);
            clon.gameObject.name = "Explosion";

            clon.gameObject.SetActive(true);
            coll.gameObject.SetActive(false);
            ///////////////////////////////////////codigo de manejo de hit

            Sc_GameManager.gameManager.GanarPuntos(1);
            Sc_SoundPlayer.sPlayer.Play(1);

        }
    }
}

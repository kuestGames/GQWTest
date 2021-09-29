using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Enemy : MonoBehaviour {
    public bool tieneHP = false;
    public bool tieneHPUI = false;
    private float _hp = 50;
    public float _ohp = 50;
    //public ObjType _objType;
    public int danoMultiplier;

    /*public enum ObjType{
        Enemigo, 
        Pared, 
        Base,
        Humano,
        Piso
    }*/


    void Start() {
        _hp = _ohp;
    }

    void OnTriggerEnter(Collider coll) {
        Vector3 position = coll.transform.position;
        if (coll.tag == "Base") {
            if (gameObject.tag == "Enemy") {
                GameObject clon = Sc_GameManager.gameManager.explosionPool.GetObj();
                if (clon == null) {
                    return;
                }
                clon.transform.position = transform.position;
                clon.transform.localScale = new Vector3(6, 6, 6);
                clon.gameObject.name = "Explosion";
                //clon.gameObject.SetActive(false);
                clon.gameObject.SetActive(true);
                ///////////////////////////////////////codigo de manejo de hit
                Sc_SoundPlayer.sPlayer.Play(1);
                Sc_GameManager.gameManager.RecibirGolpe(danoMultiplier);
                this.gameObject.SetActive(false);

            }
        }
    }

    /*private void RecibirDano(int dM) {
        _hp -= dM;
        if (_hp <= 0) {
            _hp = 0;
            /////////////animacion o algo de que se destruyo
            if (gameObject.tag == "Base") {

                Sc_GameManager.gameManager.Terminar();
                //////////algo de que termina
            }
        }
        if (tieneHPUI) {
            //disminuir barra de hp
            Sc_GameManager.gameManager..ActualizarBarra(_hp, _ohp);
        }

    }*/
}

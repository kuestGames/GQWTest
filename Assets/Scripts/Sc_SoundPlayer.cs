using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_SoundPlayer : MonoBehaviour {
    public static Sc_SoundPlayer sPlayer;
    public GameObject musicBar;
    void Awake() {
        if (sPlayer == null) {
            sPlayer = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void Play(int s) {
        switch (s) {
            case 1: //choque
                AudioSource audioSource= transform.Find("SGolpe1").GetComponent<AudioSource>();
                audioSource.PlayOneShot(audioSource.clip);
                break;

            case 2: //Estrella aparece
                transform.Find("SGolpe2").GetComponent<AudioSource>().Play();
                break;

            case 3: //EstrellaTouch
                transform.Find("SGolpe3").GetComponent<AudioSource>().Play();
                break;

            case 4: //Reverse
                transform.Find("SDisparo").GetComponent<AudioSource>().Play();
                break;

            case 5: //NuevoCycle
                transform.Find("SGanar").GetComponent<AudioSource>().Play();
                break;

            case 6: //NewHigh
                transform.Find("SRecord").GetComponent<AudioSource>().Play();
                break;

            case 7: //Boton
                transform.Find("SPerder").GetComponent<AudioSource>().Play();
                break;

            case 8: //Boton
                transform.Find("SBoton").GetComponent<AudioSource>().Play();
                break;

            case 9: //Camera
                transform.Find("SCamara").GetComponent<AudioSource>().Play();
                break;

            case 10: //Carril
                transform.Find("SPlacement").GetComponent<AudioSource>().Play();
                break;
            default:
                break;
        }


    }


    public void SwitchMusica(bool nuevoEstado) {
        if (nuevoEstado) {
            transform.GetComponent<AudioSource>().volume = 1;
            musicBar.SetActive(false);
        }
        else {
            transform.GetComponent<AudioSource>().volume = 0;
            musicBar.SetActive(true);
        }
    }
}

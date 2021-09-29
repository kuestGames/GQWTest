using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using GQW;

public class Sc_GameManager : MonoBehaviour{
    public static Sc_GameManager gameManager;
    public Sc_Pool enemyPool;
    public Sc_Pool explosionPool;
    public Sc_Pool wallPool;
    public Sc_Pool towerPool;
    public Sc_Pool warriorPool;
    public Sc_Pool bulletPool;

	private bool empezado = false;
	private bool terminado = false;
	private bool pausado = false;
	private float timer = 0;
	private float timerMax = 1f;
	private float timerWave = 0;
	private float timerWaveMax = 10f;
	public bool playingFlag = false;//solo usada para bloquear los demas botones al tocar jugar
	public float speedMultiplier = 5f;
	public float bulletSpeed = 500f;

	public GameObject PausaScreen;
	public GameObject FinalScreen;
	public GameObject InicioScreen;
	public RectTransform SeleccionScreen;
	public GameObject InGameUIScreen;
	public GameObject PlacementScreen;
	public TextMeshPro PointsLabel;
	public Vector3 choquePos;
	public int points = 0;
	public int coins = 0;
	public Transform shootObject;
	public GameObject placementManager;


	public GameObject display;
	public GameObject explosion;
	public int multiplier = 1;


	private int wave = 0;
	public GameObject waveObject;
	private bool waveBreak = false;
	private float tamanoEnemys = 6f;
	private float spawnRadio = 120f;
	private float pi2 = 0;
	public Transform originTarget;
	[HideInInspector]
	public float globalSizeRatio = 1;

	private float hp = 9999; //current hp
	public float ohp = 0; //original hp
	public Transform barraHP;

	public Transform EnemiesHolder;


	void Awake() {
		if (gameManager == null) {
			DontDestroyOnLoad(gameObject);
			gameManager = this;
		}
		else if (gameManager != this) {
			Destroy(gameObject);
		}
	}
	// Start is called before the first frame update
	void Start() {
		pi2 = Mathf.PI * 2f;
		InGameUIScreen.SetActive(false);
		
	}

	void Update() {
		if (empezado && !terminado && !pausado) {
			if (!waveBreak) {
				timer = timer + Time.deltaTime;
				timerWave = timerWave + Time.deltaTime;
				if (timer > timerMax) {
					timer = 0;
					//snap2.TransitionTo (1f);
					SpawnEnemy();
				}
				/*if (!timerMaxReach) {
					timerMax = timerMax - Time.deltaTime * 0.02f;
					speedMultiplier = speedMultiplier + Time.deltaTime * 0.01f;
					if (timerMax < 0.12f) {
						timerMaxReach = true;
					}
				}*/

				if (timerWave > timerWaveMax) {
					timerWave = 0;
					PreNewWave();
				}
			}
		}
	}

	public void SpawnEnemy() {// nuevo de arriba a abajo
		int type = Random.Range(0, 100);
		int tempInt = 1;
		float randomAngulo = Random.Range(0, pi2);
		float rX = Mathf.Sin(randomAngulo) * spawnRadio;
		float rZ = Mathf.Cos(randomAngulo) * spawnRadio;
		if (type < 59) {    //enemy1
			int randomP = Random.Range(tempInt * (-1), tempInt + 1);
			Vector3 position = new Vector3(rX, originTarget.position.y, rZ);
			GameObject clon = enemyPool.GetObj();
			if (clon == null) return;
			clon.transform.SetParent(EnemiesHolder);
			clon.transform.localPosition = position;
			clon.transform.localScale = new Vector3(tamanoEnemys, tamanoEnemys, tamanoEnemys);
			clon.gameObject.name = "Enemy1";
			clon.gameObject.SetActive(true);
			clon.transform.LookAt(originTarget.position);
			Rigidbody clonrigid = clon.GetComponent<Rigidbody>();
			clonrigid.velocity = Vector3.zero;
			clonrigid.angularVelocity = Vector3.zero;
			clonrigid.AddForce((originTarget.position - position) * speedMultiplier);
			//clon.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, speedMultiplier*Random.Range (-300f, -500.0f)));
			//cambiar modelo


		}
		else {
			if (type < 79) { //enemy2
				int randomP2 = Random.Range(tempInt * (-1) + 1, tempInt + 1);
				Vector3 position2 = new Vector3(rX, originTarget.position.y, rZ);
				GameObject clon2 = enemyPool.GetObj();
				if (clon2 == null)
					return;
				clon2.transform.SetParent(EnemiesHolder);
				clon2.transform.localPosition = position2;
				clon2.transform.localScale = new Vector3(tamanoEnemys, tamanoEnemys, tamanoEnemys);
				clon2.gameObject.name = "Enemy2";
				clon2.gameObject.SetActive(true);
				clon2.transform.LookAt(originTarget.position);
				Rigidbody clonrigid = clon2.GetComponent<Rigidbody>();
				clonrigid.velocity = Vector3.zero;
				clonrigid.angularVelocity = Vector3.zero;
				clonrigid.AddForce((originTarget.position - position2) * speedMultiplier);
				//clon2.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, speedMultiplier*Random.Range (-400f, -700.0f)));

			}
			else {
				if (type < 91) { //enemy3
					int randomP = Random.Range(tempInt * (-1), tempInt + 1);
					Vector3 position = new Vector3(rX, originTarget.position.y, rZ);
					GameObject clon = enemyPool.GetObj();
					if (clon == null) return;
					clon.transform.SetParent(EnemiesHolder);
					clon.transform.localPosition = position;
					clon.transform.localScale = new Vector3(tamanoEnemys, tamanoEnemys, tamanoEnemys);
					clon.gameObject.name = "Enemy3";
					clon.gameObject.SetActive(true);
					clon.transform.LookAt(originTarget.position);
					Rigidbody clonrigid = clon.GetComponent<Rigidbody>();
					clonrigid.velocity = Vector3.zero;
					clonrigid.angularVelocity = Vector3.zero;
					clonrigid.AddForce((originTarget.position - position) * speedMultiplier);

				}
				else { //powerup
					if (type < 95) { //powerup
						/*int randomP = Random.Range(tempInt * (-1), tempInt + 1);
						Vector3 position = new Vector3(rX, originTarget.position.y, rZ);
						GameObject clon = powerPool.GetObj();
						if (clon == null)
							return;
						clon.transform.SetParent(EnemiesHolder);
						clon.transform.localPosition = position;
						clon.transform.localScale = new Vector3(tamanoEnemys, tamanoEnemys, tamanoEnemys);
						clon.gameObject.name = "Power";
						clon.gameObject.SetActive(true);
						clon.transform.LookAt(originTarget.position);
						clon.GetComponent<Rigidbody>().AddForce((originTarget.position - position) * speedMultiplier);*/
					}
					else {//coin
						/*int randomP = Random.Range(tempInt * (-1), tempInt + 1);
						Vector3 position = new Vector3(rX, originTarget.position.y, rZ);
						GameObject clon = coinPool.GetObj();
						if (clon == null)
							return;
						clon.transform.SetParent(EnemiesHolder);
						clon.transform.localPosition = position;
						clon.transform.localScale = new Vector3(tamanoEnemys, tamanoEnemys, tamanoEnemys);
						clon.gameObject.name = "Coin";
						clon.gameObject.SetActive(true);
						clon.transform.LookAt(originTarget.position);
						clon.GetComponent<Rigidbody>().AddForce((originTarget.position - position) * speedMultiplier);
						//clon.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, Random.Range (-200f, -400.0f)));*/
					}
				}
			}
		}
	}
	public void Pausar() {
		pausado = true;
		PausaScreen.SetActive(true);
		Time.timeScale = 0;
	}

	public void Despausar() {
		pausado = false;
		PausaScreen.SetActive(false);
		Time.timeScale = 1;
	}

	public void Terminar() {
		if (!terminado) {

			terminado = true;
			playingFlag = false;
			Time.timeScale = 1f;

			GameObject clon4 = explosionPool.GetObj();
			if (clon4 == null)
				return;
			clon4.gameObject.name = "Explosion";
			clon4.gameObject.SetActive(true);
			//FinalScreen.transform.Find ("FondoPuntos").Find ("PuntosFinal").GetComponent<TextMesh> ().text = "" + puntos;
			if (points > Sc_MainManager.manager.record) {
				//FinalScreen.transform.Find ("FondoPuntos").Find ("NewRecord").gameObject.SetActive (true);
				Sc_MainManager.manager.NewRecord(points);
			}
			else {
				//FinalScreen.transform.Find ("FondoPuntos").Find ("NewRecord").gameObject.SetActive (false);
			}
			//FinalScreen.transform.Find ("FondoPuntos").Find ("PuntosMejor").GetComponent<TextMesh> ().text = "RECORD: " + record;

			InicioScreen.SetActive(true);
			Sc_SoundPlayer.sPlayer.Play(4);
			Sc_SoundPlayer.sPlayer.Play(1);
		}

	}

	public void BotonInicio() {
		Transform canvas = GameObject.Find("Canvas").transform;

		//canvas.Find ("Pausar").gameObject.SetActive (true);
		//canvas.Find ("Points").gameObject.SetActive (true);
		//InicioScreen.GetComponent<Animator> ().SetTrigger ("Cerrar");
		//Sc_SoundPlayer.sPlayer.Play(6);
		Empezar();
	}

	public void Empezar() {
		//shootObject.gameObject.SetActive(true);
		hp = ohp;
		timerWave = 0;
		wave = 0; //debe ser 0
		PreNewWave();
		enemyPool.DesactivarTodo();
		//superManager.GetComponent<Sc_SuperManager> ().ApagarAtaqueGameOver ();

		points = 0;
		PointsLabel.text = points + "";
		//PuntosLabel.GetComponent<Animator>().SetTrigger("Animar");
		int randomBack = Random.Range(1, 5); //1,5 es del 1 al 4
											 //int randomBack = 4;
		switch (randomBack) {
			case 1:

				break;
			case 2:

				break;
			case 3:

				break;
			case 4:

				break;
			default:
				break;
		}
		timerMax = 1f;
		//timerMaxReach = false;
		speedMultiplier = 4;
		empezado = true;
		pausado = false;
		terminado = false;
		PausaScreen.SetActive(false);
		//FinalScreen.SetActive (false);
		//InicioScreen.SetActive (false);
		Time.timeScale = 1;
		switch (Application.systemLanguage) {
			case SystemLanguage.Spanish:
				DesplegarMensaje("Defiende tu Casa");
				break;
			case SystemLanguage.English:
				DesplegarMensaje("Defend your Home");
				break;
			case SystemLanguage.French:
				DesplegarMensaje("Defend your Home");
				break;
			default:
				DesplegarMensaje("Defend your Home");
				break;
		}
		//shootPosition = shootObject.transform.localPosition;
		InicioScreen.SetActive(false);
		InGameUIScreen.SetActive(true);

	}

	public void PreNewWave() {
		waveBreak = true;
		wave++;
		NewWave();
	}
	public void NewWave() {
		timerWaveMax = wave * 5f;//15
		switch (wave) {
			case 1:
				timerWaveMax = 15f;//15
				timerMax = 0.3f;
				speedMultiplier = 4f;
				break;
			case 2:
				timerWaveMax = 15f;//15
				timerMax = 0.3f;
				speedMultiplier = 5f;
				break;
			case 3:
				timerWaveMax = 20f;//20
				timerMax = 0.25f;
				speedMultiplier = 5f;
				break;
			case 4:
				timerWaveMax = 20f;//20
				timerMax = 0.2f;
				speedMultiplier = 5;
				break;
			case 5:
				timerWaveMax = 30f;
				timerMax = 0.25f;
				speedMultiplier = 5f;
				break;
			case 6:
				timerWaveMax = 30f;
				timerMax = 0.2f;
				speedMultiplier = 5;
				break;
			case 7:
				//timerWaveMax = 45f;
				timerMax = 0.15f;
				speedMultiplier = 5f;
				break;
			case 8:
				//timerWaveMax = 50f;
				timerMax = 0.14f;
				speedMultiplier = 6;
				break;
			case 9:
				//timerWaveMax = 50f;
				timerMax = 0.13f;
				speedMultiplier = 6;
				break;
			case 10:
				//timerWaveMax = 50f;
				timerMax = 0.12f;
				speedMultiplier = 6;
				break;
			default:
				//timerWaveMax = 51f;
				timerMax = 0.12f;
				speedMultiplier = 6;
				break;
		}
		waveBreak = false;
		//waveObject.SetActive (true);
	}

	private void DesplegarMensaje(string msj) {

	}

	public void Disparar() {
		Vector3 position = shootObject.position;
		GameObject clon = bulletPool.GetObj();
		if (clon == null)
			return;
		clon.transform.position = position;
		clon.transform.localScale = new Vector3(tamanoEnemys, tamanoEnemys, tamanoEnemys);
		clon.gameObject.name = "Bullet";
		clon.gameObject.SetActive(true);
		//clon.transform.LookAt(originTarget);
		//clon.transform.position = shootObject.position;
		clon.transform.rotation = shootObject.rotation;
		clon.GetComponent<Rigidbody>().velocity = Vector3.zero;
		clon.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
		clon.GetComponent<Rigidbody>().AddForce((clon.transform.forward) * bulletSpeed * 100);


	}

	public void ActualizarBarra() {
		Vector3 tempScale = barraHP.localScale;
		tempScale.z = hp / ohp;
		barraHP.localScale = tempScale;

	}

	public void GanarPuntos(int p) {
		points += p;
		PointsLabel.text = points + "";
	}

   
	public void RecibirGolpe(int dM) {
		hp -= dM;
		if (hp <= 0) {
			hp = 0;
			Sc_GameManager.gameManager.Terminar();
		}
		ActualizarBarra();

	}


}

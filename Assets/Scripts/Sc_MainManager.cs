//Octavio Lopez 2021/09
//This class serves as the main manager of the game, and persist across the scenes (when there are more than one)
//The loading and saving (local and cloud) are handled by this class
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using SimpleJSON;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;


namespace GQW {
    public class Sc_MainManager : MonoBehaviour {
        public static Sc_MainManager manager;
        private bool matchStarted;
        private bool paused;
        private bool finished;
        public GameObject LoginPanel;
        public GameObject StartPanel;
        
        
        //Persistent values
        public int userID; 
        public int record; //solo esta va public
        public string username;
        public bool alreadyInLocal = false;
        public bool alreadyInCloud = false;

        private bool alreadyTriedGet = false; //this variable is used to prevent a loop between creating and getting user data from cloud


        //private string dataURL = "https://kuestgames.com/gqwtest/api/highscore/read_single.php";
        //private string dataURL = "https://kuestgames.com/gqwtest/api/highscore/read_single.php?name=userX";
        private string dataURL = "https://kuestgames.com/gqwtest/api/highscore/";
        private string jsonString;

        private string dataString;


        void Awake() {

            manager = this;
        }
        // Start is called before the first frame update
        void Start() {
            ////TestRestAPi
            //GetRecordFromUser("usuarioX");
            //username = "plm";
            //CreateCloudUser();
            //UpdateCloudRecord(0);

            if (LoadLocal()) {
                if (alreadyInLocal) {
                    StartPanel.transform.Find("RecordLabel").GetComponent<Text>().text = "Record: " + record;
                    if (alreadyInCloud) {
                        //Skip login panel
                        GetSingleCloudRecord();
                        LoginPanel.SetActive(false);
                        StartPanel.SetActive(true);

                    }
                    else {
                        //Show login panel
                        LoginPanel.SetActive(true);
                        StartPanel.SetActive(false);
                    }
                }
                else {
                    alreadyInLocal = true;
                    //Show login panel
                    LoginPanel.SetActive(true);
                    StartPanel.SetActive(false);
                }
            }

        }

        public void NewRecord(int rec) {
            record = rec;
            SaveLocal();
            UpdateCloudRecord(rec);
            StartPanel.transform.Find("RecordLabel").GetComponent<Text>().text = "Record: " + record;
        }


        public void PressedConfirmUser(string na) {
            Debug.Log("llegue");
            username = na;
            GetSingleCloudRecord();
            LoginPanel.SetActive(false);
            StartPanel.SetActive(true);

        }


        ///////////////////////////LOCAL PERSISTENCE DATA MANAGEMENT
        public bool SaveLocal() {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

            PlayerData data = new PlayerData();
            data.userID = userID;
            data.username = username;
            data.record = record;
            data.alreadyInLocal = alreadyInLocal;
            data.alreadyInCloud = alreadyInCloud;

            bf.Serialize(file, data);
            file.Close();
            return true;
        }

        private bool LoadLocal() {
            if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")) {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);

                PlayerData data = (PlayerData)bf.Deserialize(file);
                file.Close();

                userID = data.userID;
                username = data.username;
                record = data.record;
                alreadyInLocal = data.alreadyInLocal;
                alreadyInCloud = data.alreadyInCloud;
                
            }
            return true;
        }


        /////////////////////////////REST API FUNCTIONS

        ///////////////////////////Get the record and id data from a given username
        public void GetSingleCloudRecord() {
            StartCoroutine(CorGetCloudRecord(dataURL + "read_single.php?name=" + username));
        }

        IEnumerator CorGetCloudRecord(string uri) {
            UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            //webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            //webRequest.SetRequestHeader("Authorization", pass);
            webRequest.SetRequestHeader("Content-Type", "application/json");

            using (webRequest) {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError) {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else {
                    // Print Body
                    dataString = webRequest.downloadHandler.text;
                    var jOBJ = JSON.Parse(dataString);
                    var rec = jOBJ["record"].Value;

                    if (jOBJ["record"] == null) {
                        Debug.Log("no hay un usuaio con ese nombre");
                        SingleUserGotten(false, 0, "", 0);
                    }
                    else {
                        Debug.Log("este usuario tiene de record: " + rec);
                        SingleUserGotten(true, jOBJ["id"], jOBJ["name"], jOBJ["record"]);
                    }
                }
            }
        }
        public void SingleUserGotten(bool achieved, int uid, string na, int re) {
            if (achieved) {
                userID = uid;
                username = na;
                if (re>record) {
                    record = re;
                    StartPanel.transform.Find("RecordLabel").GetComponent<Text>().text = "Record: " + record;
                }
                alreadyInCloud = true;
                SaveLocal();
            }
            else {
                if (!alreadyTriedGet) {
                    alreadyTriedGet = true;
                    CreateCloudUser();
                }
                
            }
        }

        ///////////////////////////Get all the users stored on database (just for testing purposes)
        public void GetAllCloudUsers() {
            StartCoroutine(CorGetAllCloudUsers(dataURL + "read.php"));

        }

        IEnumerator CorGetAllCloudUsers(string uri) {
            UnityWebRequest webRequest = UnityWebRequest.Get(uri);
            webRequest.SetRequestHeader("Content-Type", "application/json");
            using (webRequest) {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError) {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else {
                    // Print Body
                    dataString = webRequest.downloadHandler.text;
                    var jOBJ = JSON.Parse(dataString);
                }
            }
        }
        public void AllUsersGotten(bool achieved) {

        }

        ///////////////////////////Create new user and get id
        public void CreateCloudUser() {

            StartCoroutine(CorCreateCloudUser(dataURL + "create.php"));

        }
        IEnumerator CorCreateCloudUser(string uri) {

            JSONObject jsonForm = new JSONObject();
            jsonForm.Add("name", username);
            jsonForm.Add("record", "0");

            
            UnityWebRequest webRequest = UnityWebRequest.Post(uri, jsonForm.ToString());
            webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");
            //webRequest.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            using (webRequest) {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                string[] pages = uri.Split('/');
                int page = pages.Length - 1;
                //if(UnityWebRequest.Result == UnityWebRequest.Result.ConnectionError) { 
                if (webRequest.isNetworkError) {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else {
                    // Print Body
                    dataString = webRequest.downloadHandler.text;
                    var jOBJ = JSON.Parse(dataString);
                    Debug.Log(dataString);
                    UserCreated(true);
                }
            }
        }
        public void UserCreated(bool achieved) {
            GetSingleCloudRecord();
        }

        ///////////////////////////Update existing user with new record
        public void UpdateCloudRecord(int rec) {
            StartCoroutine(CorUpdateCloudRecord(dataURL + "update.php"));
        }
        IEnumerator CorUpdateCloudRecord(string uri) {
            /*WWWForm jsonnode = new WWWForm();
            jsonnode.AddField("id", "6");
            jsonnode.AddField("name", username);
            jsonnode.AddField("record", 55);*/

            JSONObject jsonForm = new JSONObject();
            jsonForm.Add("id", userID.ToString());
            jsonForm.Add("name", username);
            jsonForm.Add("record", record.ToString());

            /*UpdateJSON ujson = new UpdateJSON();
            ujson.id = "6";
            ujson.name = username;
            ujson.record = "55";*/
            
            UnityWebRequest webRequest = UnityWebRequest.Put(uri, jsonForm.ToString());
            webRequest.SetRequestHeader("Content-Type", "application/json;charset=UTF-8");
            using (webRequest) {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();
                string[] pages = uri.Split('/');
                int page = pages.Length - 1;

                if (webRequest.isNetworkError) {
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                }
                else {
                    // Print Body
                    dataString = webRequest.downloadHandler.text;
                    var jOBJ = JSON.Parse(dataString);
                }
            }
        }
        public void RecordUpdated(bool achieved) {

        }
    }
}

[System.Serializable]
class PlayerData {
    public int userID;
    public string username;
    public int record;
    public bool alreadyInLocal;
    public bool alreadyInCloud;
}


//pendientes
//el playerdata se esta haciendo hasta que se guarda en cloud
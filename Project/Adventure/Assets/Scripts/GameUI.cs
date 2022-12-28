using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    [System.Serializable]
    public class Result
    {
        public string result;
        public int id;
    }

    public GameObject player;
    public Avatar[] avatar;
    public GameObject[] character;

    public GameObject mainUI;
    public GameObject signinUI;
    public GameObject signupUI;

    public Text characterNum;
    public InputField playerName;
    public InputField password0;
    public InputField playerName1;
    public InputField password1;
    public InputField password2;

    public string absURL = "";
    string signupURL;
    string signinURL;

    public static int characterN = 2;

    # region MonoBehaviour
    void Start()
    {
        signupURL = absURL + "api/signup";
        signinURL = absURL + "api/login";
    }
    
    void Update()
    {
        mainUI.transform.localScale = new Vector3(Screen.width / 1366f, Screen.height / 768f, 1f);
    }

    #endregion

    # region UI
    public void StartBtnClick()
    {
        if (playerName.text != "" && password0.text != "")
        {
            // Request "sign in" Api.
            StopAllCoroutines();
            StartCoroutine(PostRequestSign(signinURL, playerName.text, password0.text));
        }
        else
        {
            print("Please fill all fields.");
        }
    }

    public void NextBtnClick()
    {
        if(int.Parse(characterNum.text) < 4)
        {
            characterNum.text = (int.Parse(characterNum.text) + 1).ToString();
        }
    }

    public void PreviousBtnClick()
    {
        if(int.Parse(characterNum.text) > 1)
        {
            characterNum.text = (int.Parse(characterNum.text) - 1).ToString();
        }
    }

    public void SignupBtnClick()
    {
        if (playerName1.text != "" && password1.text != "" && password2.text != "")
        {
            if (password1.text == password2.text)
            {
                // Request "sign up" Api
                StopAllCoroutines();
                StartCoroutine(PostRequestSign(signupURL, playerName1.text, password2.text));
            }
            else
            {
                print("Please type password again.");
                password2.text = "";
            }
        }
        else
        {
            print("Please fill all fields.");
        }
    }

    #endregion

    #region Backend API
    IEnumerator PostRequestSign(string url, string name, string password)
    {
        // Send user name and password.
        WWWForm form = new WWWForm();
        form.AddField("name", name);
        form.AddField("password", password);

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
            characterN = int.Parse(characterNum.text) - 1;
            SceneManager.LoadScene("Level1");
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);

            // Recieve data from backend.
            Result loadData = JsonUtility.FromJson<Result>(uwr.downloadHandler.text);
            string result = loadData.result;
            int userId = loadData.id;

            if (url == signinURL)
            {
                if (string.Equals(result, "1")) // Sign in successfully.
                {
                    PlayerPrefs.SetInt("USER_ID", userId);
                    characterN = int.Parse(characterNum.text) - 1;
                    SceneManager.LoadScene("Level1");                    
                }
                else if (string.Equals(result, "2")) // User doesn't existed
                {
                    print("Not registered.");
                }
                else // Enter wrong password.
                {
                    print("Invalid password.");
                    password0.text = "";
                }
            }
            else if (url == signupURL)
            {
                if (string.Equals(result, "1")) // User already exists
                {
                    print("Already Exists");
                }
                else // Sign up succesfully.
                {
                    signinUI.SetActive(true);
                    signupUI.SetActive(false);
                    print("Successful");
                }
            }
        }
    }
    #endregion
}
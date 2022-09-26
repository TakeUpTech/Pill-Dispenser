using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginMenu : MonoBehaviour
{
    private string user, password, currentUser, currentPassword;
    public GameObject loginErrorText;

    // Start is called before the first frame update
    void Start()
    {
      user = "user";
      password = "0000";
    }

    public void ReadUser(TMP_InputField input)
    {
      currentUser = input.text;
      CheckLogin();
    }

    public void ReadPassword(TMP_InputField input)
    {
      currentPassword = input.text;
      CheckLogin();
    }

    private void CheckLogin()
    {
      if(currentUser == user && currentPassword == password)
      {
        SceneManager.LoadScene(1);
      } 
      else 
      {
        loginErrorText.SetActive(true);
        //Debug.Log("Incorrect password or user name.");
      }
    }

    public void Quit()
    {
      Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationMenu : MonoBehaviour
{
    public void Logout()
    {
        SceneManager.LoadScene(0);
    }
}

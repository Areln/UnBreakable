using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SignInManager : MonoBehaviour
{
    public TextMeshProUGUI LoginUsername;
    public TextMeshProUGUI LoginPassword;
    public GameObject LoginScreen;
    internal static SignInManager Instance;

	internal void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void SignUpPressed()
    {
        SignUpManager.Instance.SignUpScreen.SetActive(true);
        LoginScreen.SetActive(false);        
    }

    public void SignInPressed()
	{
        new SignInHandle().WriteMessage(LoginUsername.text, LoginPassword.text);
        SceneManager.LoadScene("Overworld");
    }
}

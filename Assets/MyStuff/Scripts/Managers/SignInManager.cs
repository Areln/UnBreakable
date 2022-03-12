using UnityEngine;
using UnityEngine.UI;

public class SignInManager : MonoBehaviour
{
    public Text LoginUsername;
    public Text LoginPassword;
    public Text LoginErrorMessage;
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
        new SignInHandle().WriteMessage();
        LoginScreen.SetActive(false);
    }
}

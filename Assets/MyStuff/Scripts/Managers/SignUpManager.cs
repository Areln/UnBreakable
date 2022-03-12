using UnityEngine;
using UnityEngine.UI;

public class SignUpManager : MonoBehaviour
{
    public Text SignUpUsername;
    public Text SignUpEmail;
    public Text SignUpPassword;
    public Text SignUpConfirmPassword;
    public Text SignUpErrorMessage;

    public GameObject SignUpScreen;
    internal static SignUpManager Instance;

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

    public void CancelSignUpPressed()
    {
        SignUpErrorMessage.text = string.Empty;
        SignUpScreen.SetActive(false);
        SignInManager.Instance.LoginScreen.SetActive(true);
    }

    public void CreatePressed()
    {
        if (string.Equals(SignUpPassword.text, SignUpConfirmPassword.text, System.StringComparison.OrdinalIgnoreCase))
        {
            new SignUpHandle().WriteMessage();
            SignUpScreen.SetActive(false);
            SignInManager.Instance.LoginScreen.SetActive(false);
        }
        else
        {
            SignUpErrorMessage.text = "The passwords must match.";
        }
        
    }
}

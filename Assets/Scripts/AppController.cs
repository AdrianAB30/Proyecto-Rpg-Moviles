using UnityEngine;

public class AppController : MonoBehaviour
{
    [Header("Scripts")]
    public AuthService authService;
    public AuthUIController authUI;

    async void Start()
    {
        authUI.mainMenuPanel.gameObject.SetActive(false);
        authUI.authPanel.gameObject.SetActive(true);

        authUI.statusText.text = "Connecting server...";
        await authService.InitializeServicesAsync();

        bool isReturningUser = PlayerPrefs.GetInt("HasRegistered", 0) == 1;
        authUI.statusText.text = isReturningUser ? "¡Welcome Back!" : "Create account.";

        authUI.Setup(authService);
    }
}
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AuthUIController : MonoBehaviour
{
    [Header("Configuraciones de Animación")]
    [SerializeField] private UIAnimationSO blackScreenAnim; 
    [SerializeField] private UIAnimationSO profileAnim;    

    [Header("Paneles (RectTransforms)")]
    public RectTransform authPanel;
    public RectTransform mainMenuPanel;
    [SerializeField] private RectTransform blackScreen;
    [SerializeField] private RectTransform profilePanel;

    [Header("UI - Authentication")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button registerBtn;
    [SerializeField] private Button loginBtn;
    [SerializeField] private Button authEyeBtn;
    public TMP_Text statusText;

    [Header("UI - Main Menu")]
    [SerializeField] private TextMeshProUGUI welcomeText;
    [SerializeField] private Button openProfileBtn;

    [Header("UI - Profile & Change Password")]
    [SerializeField] private TMP_Text profileUsernameTxt;
    [SerializeField] private TMP_InputField profileCurrentPassInput; 
    [SerializeField] private Button profileEyeBtn;

    [SerializeField] private TMP_InputField newPasswordInput;
    [SerializeField] private Button newPassEyeBtn;
    [SerializeField] private Button confirmChangeBtn;
    [SerializeField] private Button closeProfileBtn;

    [SerializeField] private AuthService authService;
    private string currentUsername;
    private string currentPassword;

    private bool authPassVisible, profilePassVisible, newPassVisible;

    public void Setup(AuthService service)
    {
        authService = service;

        authEyeBtn.onClick.AddListener(() => ToggleVisibility(passwordInput, ref authPassVisible));

        closeProfileBtn.onClick.AddListener(() => profileAnim.PlayExit(profilePanel));
        profileEyeBtn.onClick.AddListener(() => ToggleVisibility(profileCurrentPassInput, ref profilePassVisible));
        newPassEyeBtn.onClick.AddListener(() => ToggleVisibility(newPasswordInput, ref newPassVisible));

        profilePanel.gameObject.SetActive(false);
        blackScreen.gameObject.SetActive(false);
    }

    private void ToggleVisibility(TMP_InputField input, ref bool isVisible)
    {
        isVisible = !isVisible;
        input.contentType = isVisible ? TMP_InputField.ContentType.Standard : TMP_InputField.ContentType.Password;
        input.ForceLabelUpdate();
    }

    public async void OnRegisterClicked()
    {
        statusText.text = "Registered...";
        var result = await authService.RegisterAsync(usernameInput.text, passwordInput.text);
        if (result.success)
        {
            PlayerPrefs.SetInt("HasRegistered", 1);
            PlayerPrefs.Save();
            ProceedToMenu();
        }
        else statusText.text = result.message;
    }

    public async void OnLoginClicked()
    {
        statusText.text = "Login...";
        var result = await authService.LoginAsync(usernameInput.text, passwordInput.text);
        if (result.success)
        {
            ProceedToMenu();
        }

        else statusText.text = result.message;
    }

    public void ProceedToMenu()
    {
        currentUsername = usernameInput.text;
        currentPassword = passwordInput.text;

        blackScreenAnim.PlayEnter(blackScreen, () =>
        {
            authPanel.gameObject.SetActive(false);
            mainMenuPanel.gameObject.SetActive(true);
            welcomeText.text = $"Welcome {currentUsername}";

            DOVirtual.DelayedCall(blackScreenAnim.stayDuration, () =>
            {
                blackScreenAnim.PlayExit(blackScreen);
            });
        });
    }

    public void OpenProfile()
    {
        profileUsernameTxt.text = currentUsername;
        profileCurrentPassInput.text = currentPassword;

        profilePassVisible = false;
        profileCurrentPassInput.contentType = TMP_InputField.ContentType.Password;
        profileCurrentPassInput.ForceLabelUpdate();

        newPassVisible = false;
        newPasswordInput.contentType = TMP_InputField.ContentType.Password;
        newPasswordInput.text = "";
        newPasswordInput.ForceLabelUpdate();

        profileAnim.PlayEnter(profilePanel);
    }

    public async void OnChangePasswordClicked()
    {
        string newPass = newPasswordInput.text;
        if (string.IsNullOrEmpty(newPass)) return;

        bool success = await authService.ChangePasswordAsync(currentPassword, newPass);
        if (success)
        {
            currentPassword = newPass;
            profileCurrentPassInput.text = currentPassword;
            newPasswordInput.text = ""; 
            Debug.Log("Password Update.");
        }
    }
}
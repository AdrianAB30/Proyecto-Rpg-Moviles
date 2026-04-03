using System;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;

public class AuthService : MonoBehaviour
{
    public async Task InitializeServicesAsync()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogError("Error starting Unity Services: " + e.Message);
        }
    }

    public async Task<(bool success, string message)> RegisterAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            return (true, "Registered successfully");
        }
        catch (AuthenticationException e) 
        {
            return (false, "Registration error. Please check your information..");
        }
        catch (Exception e)
        {
            return (false, "Error to connect.");
        }
    }

    public async Task<(bool success, string message)> LoginAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            return (true, "Login succesful");
        }
        catch (AuthenticationException e)
        {
            return (false, "User or Password incorrect.");
        }
        catch (Exception e)
        {
            return (false, "Error to connect.");
        }
    }

    public async Task<bool> ChangePasswordAsync(string currentPassword, string newPassword)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePasswordAsync(currentPassword, newPassword);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Error changing password: " + e.Message);
            return false;
        }
    }
}
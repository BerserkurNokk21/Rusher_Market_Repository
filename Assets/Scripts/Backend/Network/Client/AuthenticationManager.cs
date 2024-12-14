using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public enum AuthState
{
    NotAuthenticated,
    Authenticating,
    Authenticated,
    Error,
    TimeOut
} 

public static class AuthenticationManager
{
    public static AuthState stateAuth { get; private set; } = AuthState.NotAuthenticated;

    public static async Task<AuthState> DoAuth(int maxTries = 5)
    {
        if (stateAuth == AuthState.Authenticated)
            return stateAuth;

        if (stateAuth == AuthState.Authenticating)
        {
            Debug.LogWarning("Already Authenticating");
            await Authenticating();
            return stateAuth;
        }

        await SignInAnonymouslyAsync(maxTries);
        
        return stateAuth;
    }
    private static async Task<AuthState> Authenticating()
    {
        while (stateAuth == AuthState.Authenticating || stateAuth == AuthState.NotAuthenticated)
        {
            await Task.Delay(500);
        }
        return stateAuth;
    }
    private static async Task SignInAnonymouslyAsync(int maxTries = 5)
    {
        stateAuth = AuthState.Authenticating;
        int tries = 0;
        while (stateAuth == AuthState.Authenticating && tries < maxTries)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    stateAuth = AuthState.Authenticated;
                    break;
                }
            }
            catch (AuthenticationException ex)
            {
                Debug.LogError(ex);
                stateAuth = AuthState.Error;
            }
            catch(RequestFailedException ex)
            {
                Debug.LogError(ex);
                stateAuth = AuthState.Error;
            }
            

            tries++;
            await Task.Delay(1000);
        }

        if(stateAuth != AuthState.Authenticated)
        {
            Debug.LogError($"Authentication TimeOut in {tries}");
            Debug.Log(stateAuth);
            stateAuth = AuthState.TimeOut;
        }
    }
}

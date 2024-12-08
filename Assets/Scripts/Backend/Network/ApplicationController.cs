using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ClientSingleton clientPrefab;
    [SerializeField] private HostSingleton hostPrefab;
    private async void Start()
    {
        DontDestroyOnLoad(gameObject);

        await LaunchingInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    private async Task LaunchingInMode(bool isDedicateServer)
    {
        if (isDedicateServer)
        {

        }
        else
        {

            HostSingleton hostSingleton = Instantiate(hostPrefab);

            hostSingleton.CreateHost();


            ClientSingleton clientSingleton = Instantiate(clientPrefab);

            bool isAuthenticated = await clientSingleton.CreateClient();
            
            if (isAuthenticated)
            {
                clientSingleton.GameManager.GoToMainMenu();
            }
        }
    }
}

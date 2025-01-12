using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class LobbysList : MonoBehaviour
{
	[SerializeField] private Transform lobbyItemParent;
	[SerializeField] private LobbyItem lobbyItemPrefab;
    private bool _isRefreshing = false;
    private bool _isJoining;
	public GameObject m_LoadingScreen;
	public UiLoadAnimation m_UiLoadAnimation;

    private void OnEnable()
    {
		RefreshList();
    }

	public async void RefreshList() 
	{
		if (_isRefreshing) { return; }
		_isRefreshing = true;

		try
		{
			QueryLobbiesOptions options = new QueryLobbiesOptions();

			options.Count = 25;

			options.Filters = new List<QueryFilter>()
			{
				new QueryFilter(
                
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
					value: "0"
					),
				new QueryFilter(
					field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ,
                    value: "0"
                    )
                
			};

			QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);	

			foreach (Transform child in lobbyItemParent)
            {
                Destroy(child.gameObject);
            }

            foreach (Lobby lobby in lobbies.Results)
            {
                LobbyItem lobbyItem = Instantiate(lobbyItemPrefab, lobbyItemParent);
                lobbyItem.Initialise(this, lobby);
            }
        }
		catch (LobbyServiceException ex)
		{
			Debug.LogException(ex);
			return;
		}
	}


    public async void JoinAsync(Lobby lobby)
    {
		_isJoining = true;
		try
		{
			Lobby joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
			string joinCode = joiningLobby.Data["JoinCode"].Value;
			m_LoadingScreen.SetActive(true);
			m_UiLoadAnimation.Func_PlayUIAnim();

			await ClientSingleton.Instance.GameManager.StartClientAsync(joinCode);
		}
		catch (LobbyServiceException ex)
		{
			Debug.LogException(ex);
			return;
		}
		_isJoining = false;
    }
}

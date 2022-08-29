using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPrefabSwitcher : MonoBehaviour
{
    [SerializeField] private List<GameObject> playersPrefabs;
    [SerializeField] private int maxPlayers = 2;
    private PlayerInputManager inputManager;
    private int currentPlayerPrefabIndex = 0;

    private bool isMosquito = false;

    private void Awake()
    {
        inputManager = GetComponent<PlayerInputManager>();
    }

    public void SwitchToMosquito()
    {

       inputManager.playerPrefab = playersPrefabs[++currentPlayerPrefabIndex % playersPrefabs.Count];
    }
}

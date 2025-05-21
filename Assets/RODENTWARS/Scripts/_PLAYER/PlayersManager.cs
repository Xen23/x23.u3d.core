using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace X23
{
    public class PlayersManager : MonoBehaviour
    {
        public static PlayersManager Instance;
        public static List<PlayerController> PlayersList;
        [SerializeField] private int playerCount;

        [Header("Settings Options")] 
        [SerializeField] int maxPlayers = 4;
        [SerializeField] public List<GameObject> playerPrefabs;


        [SerializeField] private UnityEvent PlayerConnected; 
        [SerializeField] private UnityEvent PlayerDisconnected; 
        
        [SerializeField] private UnityEvent WorldLoaded; 
        [SerializeField] private UnityEvent WorldClosing; 
        
        
        // Unity functions
        private void Start()
        {
            if (Instance == null) Instance = this;
            //else Destroy(this);
            PlayersList = new List<PlayerController>();
            playerPrefabs = new List<GameObject>();
        }
        private void Awake()
        {
            if (PlayersList == null) return;
            playerCount = PlayersList.Count;
        }

        private void OnEnable()
        {
            EnableAllPlayers();
        }
        private void OnDisable()
        {
            DisableAllPlayers();
        }
        private void Update()
        {
        }
        private void FixedUpdate()
        {
        }

        // Called by external scripts
        public void RegisterPlayerRequest(PlayerController player)
        {
            if (playerCount < maxPlayers) AddPlayer(player);            
        }
        public void UnRegisterPlayerRequest(PlayerController player)
        {
            RemovePlayer(player);            
        }
        
        // Internal functions
        private void EnablePlayer(PlayerController player)
        {
            player.gameObject.SetActive(true);
        }
        private void DisablePlayer(PlayerController player)
        {
            player.gameObject.SetActive(false);
        }
        
        private void EnableAllPlayers()
        {
            PlayersList?.ForEach(EnablePlayer);
        }
        private void DisableAllPlayers()
        {
            PlayersList?.ForEach(DisablePlayer);
        }

        private void AddPlayer(PlayerController player)
        {
            var c = playerCount;
            PlayersList.Add(player);
            playerCount = PlayersList.Count;
            if (c < playerCount)
            {
                var i = PlayersList.IndexOf(player);
                player.RegisterPlayerWithManger(this, i);
                PlayerConnected.Invoke();
            }
        }
        private void RemovePlayer(PlayerController player)
        {
            var c = playerCount;
            PlayersList.Remove(player);
            playerCount = PlayersList.Count;
            if (c > playerCount)
            {
                PlayerConnected.Invoke();
            }
        }


    }
}
﻿using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// Class for all Player prefabs.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Ship))]
public partial class PlayerController : NetworkBehaviour {

    #region Properties

    /// <summary>
    /// Name of the player. Synchronized variable
    /// </summary>
    public string PlayerName
    {
        get { return _playerName; }
        set { CmdChangeName(value); }
    }

    /// <summary>
    /// Ship script of the Player.
    /// </summary>
    public Ship Ship { get; private set; }

    #endregion

    // Use this for initialization
    void Start()
    {
        // Update PlayerName, to reflect actual value for players that just joined
        OnPlayerNameChanged(PlayerName);

        // All other players
        if (isLocalPlayer)
            return;

        // Set the reference for Ship
        Ship = GetComponent<Ship>();
    }

    // Update is called once per physics tick
    void FixedUpdate ()
	{
        // Check if this code runs on the game object that represents my Player
	    if (!isLocalPlayer)
	        return;

	    if (Input.GetKeyDown(KeyCode.F))
	    {
	        Ship.AddPart(new PartData(0, new Vector3(0, 1, 0)));
	        Debug.Log(Ship.PartsData.Count);
	    }

        //todo temporary code for testing - remove later
        Ship.Thrust(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
	    
        //todo how to shoot - remove later
        if (Input.GetKeyDown(KeyCode.Space))
	        Ship.Shoot();

        //todo changing aiming direction - upgrade later
	    if (Input.GetKey(KeyCode.E))
	        Ship.Target = Ship.Target + new Vector3(0f, Time.fixedDeltaTime * 30f, 0f);
	    if (Input.GetKey(KeyCode.Q))
	        Ship.Target = Ship.Target + new Vector3(0f, -Time.fixedDeltaTime * 30f, 0f);

        //todo manually refresh all parts
	    if (Input.GetKeyUp(KeyCode.R))
	        Ship.RefreshParts();
    }

}

﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

/// <summary>
/// Class for handling player input in buildmode.
/// </summary>
public class BuildController : MonoBehaviour {

    public bool buildmode;
    //ID of part to spawn. Set currentNode to null to regenerate ghost
    public int SelectedPartID { 
        private get { return selectedPartID; }
        set { selectedPartID = value; RecreateGhost(); } 
    }
    private int selectedPartID;
    private Node currentNode;
    private Ship playerShip;

    private ReadOnlyCollection<Node> availableNodes;

    //todo not used - maybe not needed
    private Dictionary<Vector3Int, Part> currentParts;

    private GameObject ghost;
    private CameraModeToggle cameraModeToggle;

    // Use this for initialization
    void Start ()
    {
        //initialise
        currentNode = null;
        buildmode = false;
 
        //Get player controller
        cameraModeToggle = FindObjectOfType<CameraModeToggle>();
    }

    // Update is called once per frame
    void Update()
    {
        // Buildmode
        if (!buildmode)
            return;


        playerShip = GameController.LocalPlayerController.Ship;

        if (currentNode == null)
        {
            currentParts = playerShip.Parts;
            availableNodes = playerShip.GetNodes(true);
            //todo needs to check if there are nodes available
            currentNode = availableNodes[0];

            //Display 'ghost' block
            RecreateGhost();
        }
    

        //Node Cycling
        // Cycle left
        if (Input.GetKeyDown(KeyCode.A))
        {
            //Cycle
            currentNode = availableNodes.Count - 1 > availableNodes.IndexOf(currentNode) ? 
                availableNodes[availableNodes.IndexOf(currentNode) + 1] : availableNodes[0];

            //Move ghost
            UpdateGhostTransform();
        }
        // Cycle right
        if (Input.GetKeyDown(KeyCode.D))
        {
            //Cycle
            currentNode = availableNodes.IndexOf(currentNode) > 0 ?
                availableNodes[availableNodes.IndexOf(currentNode) - 1] : availableNodes[availableNodes.Count - 1];

            //Update ghost
            UpdateGhostTransform();
        }

        //Build Part
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 position = ghost.transform.localPosition;

            //Destory ghost and reset currentNode
            Destroy(ghost);
            currentNode = null;

            //Build Part
            PartData newPart = new PartData(selectedPartID, position);
            playerShip.AddPart(newPart);

            //Update bounds for camera zoom
            cameraModeToggle.CalculateBounds();
        }

        //Remove Part
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            //if part is not core
            if (currentNode.transform.parent.parent.gameObject.GetComponent<Ship>() == null)
            {
                //Remove Part (Networked)
                playerShip.RemovePart(currentNode.transform.parent.parent.transform.localPosition);

                // Update currentParts collection
                currentParts = playerShip.Parts;

                //Destory ghost and reset currentNode
                Destroy(ghost);
                currentNode = null;

                //Update bounds for camera zoom
                cameraModeToggle.CalculateBounds();
            }
        }

        //TESTING - RETURN NETWORKED PART COUNT
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Debug.Log(playerShip.Parts.Count);
        }
    }

    private void RecreateGhost()
    {
        if(ghost) Destroy(ghost);

        ghost = Instantiate(PartManager.Instance.GetPartById(selectedPartID).Prefab, transform);
        ghost.name = "ghost";
        ghost.GetComponent<Part>().isGhost = true;

        //Make transparent - requires matrial rendering mode: Transparent. Doing this programatically is unfortunatly not currently simple.
        //todo not working for Test Weapon
        //Color col = ghost.gameObject.GetComponent<Renderer>().material.color;
        //col.a = 0.66f;
        //ghost.gameObject.GetComponent<Renderer>().material.color = col;

        UpdateGhostTransform();
    }

    private void UpdateGhostTransform()
    {
        ghost.transform.localPosition = currentNode.AttachmentPosition;
        //todo should also update rotation
    }

    public void UpdateBuildmode(bool b)
    {
        buildmode = b; //Hard set rather than toggle to ensure sync
        if (!buildmode)
        {
            Destroy(ghost);
            currentNode = null;
        }
    }
}
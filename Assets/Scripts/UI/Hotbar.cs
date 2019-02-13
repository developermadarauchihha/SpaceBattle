﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{

	///<summary>
	/// reference to a part in inventory corresponding to the index of the highlighted cell
	///</summary>
	public Part SelectedPart { get; private set; }

	private List<Image> highlights = new List<Image>();
	private List<Button> cells = new List<Button>();
	private int activeCell = 0;

	// Use this for initialization
	void Start()
	{
		//populate cells
		cells = GetComponentsInChildren<Button>().ToList();
		//populate highlights with the child image from each cell
		foreach (Button b in cells)
		{
			//use getComponents plural and index of 1 to avoid finding the image on the parent instead.
			Image highlightBorder = b.GetComponentsInChildren<Image>() [1];
			highlights.Add(highlightBorder);
		}
		Debug.Log("cells : " + cells.Count);
		Debug.Log("highlights : " + highlights.Count);
	}

	// Update is called once per frame
	void Update()
	{
		bool[] hotkeys = new bool[5];

		//all the inputs checked so I dont have lots of repeat code
		hotkeys[0] = Input.GetKeyDown(KeyCode.Alpha1);
		hotkeys[1] = Input.GetKeyDown(KeyCode.Alpha2);
		hotkeys[2] = Input.GetKeyDown(KeyCode.Alpha3);
		hotkeys[3] = Input.GetKeyDown(KeyCode.Alpha4);
		hotkeys[4] = Input.GetKeyDown(KeyCode.Alpha5);

		//if a kotkey is pressed set its correspondng cell as the active cell
		for (int i = 0; i < hotkeys.Length; i++)
		{
			if (hotkeys[i])
			{
				Debug.Log("hotbar index : " + i);
				activeCell = i;
			}
		}

		//turn on the highlight for the active cell and off for inactive cells
		for (int i = 0; i < cells.Count; i++)
		{
			if (i == activeCell)
			{
				highlights[i].gameObject.SetActive(true);
			}
			else
			{
				highlights[i].gameObject.SetActive(false);
			}
		}

		//set selected part to a reference to a part on the top row of the inventory
		SelectedPart = Inventory.locations[0, activeCell];

	}

	public void SetHotbarIndex(Button button)
	{
		//check if a valid button sent the message
		if (cells.Contains(button))
		{
			//set that cell to be highlighted
			activeCell = cells.IndexOf(button);
		}
	}
}
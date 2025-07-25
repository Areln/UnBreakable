﻿using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
	internal PlayerBrain brain;

	//DestinationMarker
	public GameObject destinationMarkerPrefab;

	//currently placed destinationMarker
	internal GameObject destinationMarkerPlaced;

	// Start is called before the first frame update
	void Start()
	{
		brain = GetComponent<PlayerBrain>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (brain.updateMove)
		{
			brain.updateMove = false;
			Destroy(destinationMarkerPlaced);
			destinationMarkerPlaced = null;
		}
		if (destinationMarkerPlaced == null && brain.targetPosition.HasValue && Vector3.Distance(transform.position, brain.targetPosition.Value) > .05f)
		{
			var position = brain.positions.Count > 0 ? brain.positions.Last().Position : brain.targetPosition.Value;
			var markerPosition = new Vector3(position.x, position.y-1.025f, position.z);
			destinationMarkerPlaced = Instantiate(destinationMarkerPrefab, markerPosition, Quaternion.identity);
		}

		// Distance check for StorageObject UI

		try
		{
			if (brain.GetActiveWorldObjectTransform())
			{
				if (Vector3.Distance(brain.GetActiveWorldObjectTransform().position, transform.position) > brain.interactableRange)
				{
					HudManager.Instance.DisableContainerDisplay();
				}
			}
		}
		catch (System.Exception)
		{
			throw;
		}
	}


	internal void DestroyDestinationMarker()
	{
		Destroy(destinationMarkerPlaced);
	}
}

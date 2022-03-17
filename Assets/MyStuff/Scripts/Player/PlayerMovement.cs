using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
	//player's move agent
	NavMeshAgent agent;
	internal PlayerBrain brain;
	internal Animator animator;

	//DestinationMarker
	public GameObject destinationMarkerPrefab;

	//currently placed destinationMarker
	internal GameObject destinationMarkerPlaced;

	private Vector3? nextPoint;

	// Start is called before the first frame update
	void Start()
	{
		brain = GetComponent<PlayerBrain>();
		animator = GetComponent<Animator>();
		agent = GetComponent<NavMeshAgent>();

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
		if (agent.remainingDistance <= .05f)
		{
			StopPlayerFromMoving();
		}
		else if (destinationMarkerPlaced == null && agent.remainingDistance > .05f)
		{
			animator.SetBool("IsWalking", true);
			destinationMarkerPlaced = Instantiate(destinationMarkerPrefab, agent.destination, Quaternion.identity);
		}
	}


	internal void StopPlayerFromMoving()
	{
		animator.SetBool("IsWalking", false);
		Destroy(destinationMarkerPlaced);
	}
}

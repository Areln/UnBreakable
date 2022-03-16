using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    //player's move agent
    internal PlayerBrain brain;
	internal Animator animator;

	//DestinationMarker
	public GameObject destinationMarkerPrefab;
    
    //currently placed destinationMarker
    internal GameObject destinationMarkerPlaced;

	private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
		brain = GetComponent<PlayerBrain>();
		animator = GetComponent<Animator>();
    }

	// Update is called once per frame
	void FixedUpdate()
	{
		if (brain.PathPoints != null && brain.PathPoints.Count > 0)
		{
			if (!isMoving)
			{
				isMoving = true;
				animator.SetBool("IsWalking", isMoving);
			}

			if (destinationMarkerPlaced != null)
			{
				var destinationPoint = brain.PathPoints.Last();
				destinationMarkerPlaced = Instantiate(destinationMarkerPrefab, destinationPoint, Quaternion.identity);
			}

			var nextPoint = brain.PathPoints.Peek();
			// move towards point
			if (Vector3.Distance(transform.position, nextPoint) > 0.1f)
			{
				Vector3.MoveTowards(transform.position, nextPoint, Speed * Time.fixedDeltaTime);
				Destroy(destinationMarkerPlaced);
			}
			else
			{
				brain.PathPoints.Dequeue();
			}
		}
		else if (isMoving)
		{
			isMoving = false;
			animator.SetBool("IsWalking", isMoving);
		}


		//Destroys destination marker if close
		if (Vector3.Distance(transform.position, destinationMarkerPlaced.transform.position) <= 0.1f)
		{
			Destroy(destinationMarkerPlaced);
		}
	}


	internal void StopPlayerFromMoving()
	{
        Destroy(destinationMarkerPlaced);
		brain.PathPoints = null;
	}
}

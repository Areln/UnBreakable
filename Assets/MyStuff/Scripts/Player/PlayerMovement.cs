using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    //player's move agent
    internal NavMeshAgent agent;

    //DestinationMarker
    public GameObject destinationMarkerPrefab;
    
    //currently placed destinationMarker
    internal GameObject destinationMarkerPlaced;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PlayerBrain>() == GameManager.Instance.ClientPlayer)
        {
            //sets destination for navmesh and creates marker
            if (Input.GetMouseButton(0) && !GameManager.Instance.UsingUI && !GameManager.Instance.DraggingObject)
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, LayerMask.GetMask("Ground")))
                {

                    if (destinationMarkerPlaced != null)
                        Destroy(destinationMarkerPlaced.gameObject);

                    destinationMarkerPlaced = Instantiate(destinationMarkerPrefab, hit.point, Quaternion.identity);
                    agent.destination = hit.point;
                }
            }

            //Destroys destination marker if close
            if (agent.remainingDistance <= 0.1f)
            {
                Destroy(destinationMarkerPlaced);
            }
        }
    }

    internal void StopPlayerFromMoving()
	{
        Destroy(destinationMarkerPlaced);
        agent.destination = gameObject.transform.position;
	}

    public void SetDestination(Vector3 newDest) 
    {
        agent.destination = newDest;
    }
}

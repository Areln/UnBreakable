using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    //player's move agent
    public NavMeshAgent agent;

    //DestinationMarker
    public GameObject destinationMarkerPrefab;
    //currently placed destinationMarker
    public GameObject destinationMarkerPlaced;

    public LayerMask moveMask;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //sets destination for navmesh and creates marker
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, moveMask))
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

    public void SetDestination(Vector3 newDest) 
    {
        agent.destination = newDest;
    }
}

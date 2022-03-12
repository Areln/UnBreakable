using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //player's move agent
    internal NavMeshAgent agent;

    //DestinationMarker
    public GameObject destinationMarkerPrefab;
    
    //currently placed destinationMarker
    internal GameObject destinationMarkerPlaced;

    private bool InUI = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //sets destination for navmesh and creates marker
        if (Input.GetMouseButton(0) && !GetInMenu())
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
    public void SetInMenu(bool _value)
    {
        InUI = _value;
    }
    public bool GetInMenu()
    {
        return InUI;
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

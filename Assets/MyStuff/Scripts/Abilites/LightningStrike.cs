using System.Collections;
using UnityEngine;

public class LightningStrike : Ability
{
	private LineRenderer lineRenderer;
	public int LinePositionCount;
	private Vector3 startPosition;
	private Vector3 endScale;
	void Start()
	{
		endScale = new Vector3(transform.localScale.x * 8, transform.localScale.y * 8, transform.localScale.z * 8);
		startPosition = transform.position;
		startPosition += transform.up * 100;
		startPosition.x += UnityEngine.Random.Range(-2f, 2f);
		startPosition.z += UnityEngine.Random.Range(-2f, 2f);
		lineRenderer = GetComponent<LineRenderer>();

		//Enables line renderer
		lineRenderer.enabled = true;
		// 
		Vector3[] positions = new Vector3[LinePositionCount];

		lineRenderer.positionCount = LinePositionCount;
		//Sets starting position to players position
		lineRenderer.SetPosition(0, startPosition);



		//Loop that moves line to "random directions"
		for (int i = 1; i < LinePositionCount; i++)
		{
			//var pos = Vector3.Lerp(startPosition, transform.position, i / 4f);

			Vector3 pos = transform.position;

			//randomises lines position
			pos.x += UnityEngine.Random.Range(-1.5f, 1.5f);
			pos.z += UnityEngine.Random.Range(-1.5f, 1.5f);

			pos.y += startPosition.y / (i + 1);

			lineRenderer.SetPosition(i, pos);
		}
		//lineRenderer.positionCount = positions.Length;
		//lineRenderer.SetPositions(positions);
		//Lines end postion at the target
		//lineRenderer.SetPosition(LinePositionCount, transform.position);
		StartCoroutine(waitForSec(.25f));
	}

	private void Update()
	{
		transform.localScale = Vector3.Lerp(transform.localScale, endScale, Time.deltaTime * 5);
	}

	private IEnumerator waitForSec(float sec)
	{
		yield return new WaitForSeconds(sec);
		Destroy(gameObject);
	}

	public override void Activate(Vector3 startPosition, Vector3 targetPosition)
	{
		throw new System.NotImplementedException();
	}

	public override void RemoveAbility()
	{
		throw new System.NotImplementedException();
	}

	public override void SetupAbility(CharacterBrain _owner)
	{
		owner = _owner;
	}
}

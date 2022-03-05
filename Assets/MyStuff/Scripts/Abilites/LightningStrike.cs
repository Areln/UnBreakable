using System;
using System.Collections;
using UnityEngine;

public class LightningStrike : Ability
{
	private LineRenderer lineRenderer;
	private Vector3 startPosition;
	private Vector3 startScale;
	private Vector3 endScale;
	void Start()
	{
		startScale = transform.localScale;
		endScale = new Vector3(transform.localScale.x * 8, transform.localScale.y * 8, transform.localScale.z * 8);
		startPosition = transform.up * 10;
		lineRenderer = GetComponent<LineRenderer>();

		//Enables line renderer
		lineRenderer.enabled = true;
		//Sets starting position to players position
		lineRenderer.SetPosition(0, startPosition);
		//Loop that moves line to "random directions"
		for (int i = 1; i < 4; i++)
		{
			var pos = Vector3.Lerp(startPosition, transform.position, i / 4f);

			//randomises lines position
			pos.x += UnityEngine.Random.Range(-0.4f, 0.4f);
			pos.y += UnityEngine.Random.Range(-0.4f, 0.4f);

			lineRenderer.SetPosition(i, pos);
		}
		//Lines end postion at the target
		lineRenderer.SetPosition(4, transform.position);
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

	public override void Activate(Vector3 targetPosition)
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

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public abstract class CharacterBrain : MonoBehaviour
{
    public string characterName;
    public float Speed;
    public int maxHealth;
    public int currentHealth;
    public int maxMana;
    public int currentMana;
    internal Animator animator;

    internal Stats Stats;
    internal bool updateMove;

    public Transform AbilityHolder;
    public Ability[] abilities;

    internal Ability CurrentlyCastingAbility { get; set; }

    internal Vector3? targetPosition;
    internal float? targetRotation;
    internal bool IsMovementPaused { get; set; }
    internal Queue<MoveData> positions = new Queue<MoveData>();

    public abstract void CharacterDie();

    public void ChangeHealth(int healthChange)
    {
        currentHealth += healthChange;
        GetComponent<HPScript>().ChangeHP(healthChange, gameObject.transform.position);
    }

    internal void InitializeData(CharacterData characterData)
    {
        characterName = characterData.CharacterName;
    }

    public void StopCharacterFromMoving()
    {
        animator.SetBool("IsWalking", false);
        positions.Clear();
        targetPosition = null;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FixedUpdate()
	{
        if (!IsMovementPaused && targetPosition.HasValue)
        {
            if (Vector3.Distance(transform.position, targetPosition.Value) > .01f)
            {
                //move towards target
                var newPosition = Vector3.MoveTowards(transform.position, targetPosition.Value, Speed * Time.fixedDeltaTime);
                transform.position = newPosition;
            }
            else if (positions.Count > 0)
            {
                UpdateTargetPosition();
            }
			else
			{
                targetPosition = null;
                animator.SetBool("IsWalking", false);
                updateMove = true;
            }
        }
        else if(!IsMovementPaused && positions.Count > 0)
        {
            UpdateTargetPosition();
        }

        if(targetRotation.HasValue && transform.rotation.eulerAngles.y != targetRotation.Value)
		{
            var currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(currentRotation.x, targetRotation.Value, currentRotation.z)), Speed * Time.fixedDeltaTime);
        }
		else
		{
            targetRotation = null;
        }
    }

	private void UpdateTargetPosition()
    {
        var moveData = positions.Dequeue();
        targetPosition = moveData.Position;
        targetRotation = moveData.Rotation;
        animator.SetBool("IsWalking", true);
    }

	public void SetCharacterPath(MoveData[] moveData)
    {
        updateMove = true;
        positions = new Queue<MoveData>(moveData);
        UpdateTargetPosition();
    }

    internal virtual void CastAbility(int abilityIndex, Vector3 startPosition, Vector3 targetPosition)
    {
        abilities[abilityIndex].Activate(startPosition, targetPosition);
    }
}

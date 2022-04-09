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
    private Vector3? abilityCastPosition;
    internal float? targetRotation;
    private float? abilityCastRotation;
    internal bool IsMovementPaused { get; set; }
    internal Queue<MoveData> positions = new Queue<MoveData>();

    private const float catchUpSpeedMultiplier = 1.5f;
    private const float hardCorrectionDistance = 1f;
    private float catchUpSpeed;

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
        if (CurrentlyCastingAbility == null)
        {
            abilityCastRotation = null;
            abilityCastPosition = null;
        }
        // handle postion
        if (abilityCastPosition.HasValue)
		{
            var newPosition = Vector3.MoveTowards(transform.position, abilityCastPosition.Value, catchUpSpeed * Time.fixedDeltaTime);
            transform.position = newPosition;
        }
        else if (!IsMovementPaused && targetPosition.HasValue)
        {
            if (Vector3.Distance(transform.position, targetPosition.Value) > .01f)
            {
                //move towards target
                var newPosition = Vector3.MoveTowards(transform.position, targetPosition.Value, catchUpSpeed * Time.fixedDeltaTime);
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

        // handle rotation
        if(abilityCastRotation.HasValue)
		{
            var currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(currentRotation.x, abilityCastRotation.Value, currentRotation.z)), Speed * Time.fixedDeltaTime);
        }
        else if (targetRotation.HasValue && !Mathf.Approximately(transform.rotation.eulerAngles.y, targetRotation.Value) && !abilityCastRotation.HasValue)
		{
            var currentRotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(currentRotation.x, targetRotation.Value, currentRotation.z)), Speed * Time.fixedDeltaTime);
        }
		else
		{
            targetRotation = null;
        }
    }

	private void UpdateTargetPosition(Vector3? startPosition = null)
    {
        var moveData = positions.Dequeue();
        targetPosition = moveData.Position;

        if(startPosition.HasValue && startPosition.Value != targetPosition)
        {
            if (Vector3.Distance(startPosition.Value, transform.position) > hardCorrectionDistance)
            {
                catchUpSpeed = Speed;
                transform.position = startPosition.Value;
            }
			else
            {
                catchUpSpeed = Speed * catchUpSpeedMultiplier;
            }
		}
		else
		{
            catchUpSpeed = Speed;
		}

        targetRotation = moveData.Rotation;
        animator.SetBool("IsWalking", true);
    }

	public void SetCharacterPath(MoveData[] moveData)
    {
        updateMove = true;
        positions = new Queue<MoveData>(moveData);
        UpdateTargetPosition(positions.First().Position);
    }

    internal virtual void CastAbility(int abilityIndex, Vector3 startPosition, Vector3 targetPosition)
    {
        float angleRad = Mathf.Atan2(targetPosition.x - startPosition.x, targetPosition.z - startPosition.z);
        float angle = (180 / Mathf.PI) * angleRad;
        abilityCastRotation = angle;
        abilityCastPosition = startPosition;
        abilities[abilityIndex].Activate(startPosition, targetPosition);
    }
}

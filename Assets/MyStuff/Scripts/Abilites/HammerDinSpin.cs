using UnityEngine;

public class HammerDinSpin : Ability
{

    Vector3 initPos;

    float timeCounter;
    public float lifeTime;
    public float speed;
    public float radiusGrowth;
    public float growthRate;

    public float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        initPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        timeCounter += Time.deltaTime * speed;
        if (timeCounter >= lifeTime)
        {
            Destroy(gameObject.transform.gameObject);
        }
        radiusGrowth += (growthRate/1000);

        float xPos = Mathf.Sin(timeCounter) * radiusGrowth;
        float zPos = Mathf.Cos(timeCounter) * radiusGrowth;

        transform.GetChild(0).position = new Vector3(xPos + initPos.x, transform.GetChild(0).position.y, zPos + initPos.z);

        float yRot = Time.deltaTime * rotSpeed;

        transform.Rotate(new Vector3(0, 1, 0) * yRot);
    }

    public override void Activate(Vector3 startPosition, Vector3 targetPosition)
    {
        throw new System.NotImplementedException();
    }

    public override void SetupAbility(CharacterBrain _owner)
    {
        owner = _owner;
    }

    public override void RemoveAbility()
    {
        throw new System.NotImplementedException();
    }
}

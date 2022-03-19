using UnityEngine;

namespace Server
{
    public class AIRangeDetection : MonoBehaviour
    {

        internal ServerBasicAi aiBrain;

        public void Start()
        {
            aiBrain = GetComponentInParent<ServerBasicAi>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                aiBrain.AddInRage(other.gameObject);
                if (aiBrain.currentAIState != ServerBasicAi.AIState.Attack)
                {
                    aiBrain.currentAIState = ServerBasicAi.AIState.Attack;
                }
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}

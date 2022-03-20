using UnityEngine;

namespace Server
{
    public class AIRangeDetection : MonoBehaviour
    {

        internal ServerBasicAI aiBrain;

        public void Start()
        {
            aiBrain = GetComponentInParent<ServerBasicAI>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                aiBrain.AddInRage(other.gameObject);
                if (aiBrain.currentAIState != ServerBasicAI.AIState.Attack)
                {
                    aiBrain.currentAIState = ServerBasicAI.AIState.Attack;
                }
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}

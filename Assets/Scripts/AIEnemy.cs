using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AIEnemy : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
    GameObject player;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
         if (target != null)
          agent.SetDestination(target.position);
    }
    float runningRange = 50.0f;

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("collider");
            player = other.gameObject;
            // Set a target location to move to in the direction the player is looking
            Vector3 moveDirection = (transform.position - player.transform.position);
            // Vector3 targetDestination = player.transform.TransformDirection(transform.right) + new Vector3(UnityEngine.Random.Range(-runningRange, runningRange), 0, UnityEngine.Random.Range(-runningRange, runningRange));
            // Use this targetDestination to where you want to move your enemy NavMesh Agen
            agent.SetDestination(moveDirection*3);
        }
    }

    

     void Update()
        {


            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }
    }


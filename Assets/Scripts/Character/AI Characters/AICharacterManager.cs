using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AG
{
    public class AICharacterManager : CharacterManager
    {
        [Header("Current State")]
        [SerializeField]
        private AIState currentState = null;

        public AICharacterCombatManager aICharacterCombatManager = null;

        protected override void Awake()
        {
            base.Awake();

            aICharacterCombatManager = GetComponent<AICharacterCombatManager>();
        }

        protected override void FixedUpdate()
        {
            ProcessStateMachine();
        }

        private void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(this);

            if (nextState != null)
            {
                currentState = nextState;
            }
        }
    }
}
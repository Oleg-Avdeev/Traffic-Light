using System.Collections.Generic;
using UnityEngine;

namespace TrafficLight
{
    public class TrafficLightController : MonoBehaviour
    {
        [SerializeField] private Cycle _cycle = default;
        [SerializeField] private List<TrafficSignal> _signals = default;

        private float _stepStartTime = 0;
        private Step _currentStep = default;
        
        private void OnEnable() => Restart();

        public void Restart()
        {
            var step = _cycle.CalculateCurrentStep(Time.fixedTime);
            DisplayStep(step);
        }

        private void DisplayStep(Step step)
        {
            Debug.Log($"Displaying step: {step.Signal} for the next {step.Duration}s");

            _currentStep = step;
            _stepStartTime = Time.fixedTime;

            foreach (var signal in _signals) 
                signal.Activate(step);
        }

        private void LateUpdate()
        {
            if (Time.fixedTime - _stepStartTime >= _currentStep.Duration)
            {
                DisplayStep(_cycle.GetNextStep());
            }
        }

    }
}

using System.Collections.Generic;
using UnityEngine;
using System;

namespace TrafficLight
{
    public enum Signal : int
    {
        Stop,
        Go,
        Wait,
        TurnLeft,
        Bicycle
    }

    [Serializable]
    public struct Step
    {
        public Signal Signal;
        public float Duration;
    }

    public class Cycle : MonoBehaviour
    {
        public List<Step> Steps => _steps;

        [SerializeField] private List<Step> _steps = default;

        private float _cycleDuration = 0;
        private int _currentStep = 0;

        public Step GetNextStep()
        {
            if (_steps.Count == 0) return default;

            _currentStep = (_currentStep + 1) % _steps.Count;
            return _steps[_currentStep];
        }

        public Step CalculateCurrentStep(float currentTime)
        {
            if (_steps.Count == 0) return default;

            currentTime = currentTime % GetCycleDuration();

            for (int i = 0; i < _steps.Count; i++)
            {
                var step = _steps[i];

                currentTime -= step.Duration;
                
                if (currentTime < 0)
                {
                    step.Duration = -currentTime;
                    _currentStep = i;
                    return step;
                }
            }

            return default;
        }

        public void SetSteps(List<Step> steps)
        {
            _cycleDuration = 0;
            _steps = steps;
        }

        private float GetCycleDuration()
        {
            if (_cycleDuration == 0)
                foreach (var step in _steps)
                    _cycleDuration += step.Duration;

            return _cycleDuration;
        }
    }
}

using TrafficLight;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Example.UI
{
    public sealed class CycleRenderer : MonoBehaviour
    {
        [SerializeField] private Cycle _cycle = default;
        [SerializeField] private TrafficLightController _trafficController = default;

        [SerializeField] private StepRenderer _stepPrefab = default;
        [SerializeField] private RectTransform _stepsContainer = default;
        [SerializeField] private Button _addStepButton = default;
        [SerializeField] private Button _commitButton = default;

        private Stack<StepRenderer> _stepsPool = new Stack<StepRenderer>();
        private List<StepRenderer> _steps = new List<StepRenderer>();

        private void Start() => Initialize();

        public void Initialize()
        {
            var steps = _cycle.Steps;

            foreach (var step in steps)
            {
                var renderer = GetStepRenderer();
                renderer.SetStep(step);
                _steps.Add(renderer);
            }

            _addStepButton.onClick.AddListener(HandleAddStep);
            _commitButton.onClick.AddListener(HandleCommit);
        }

        private void HandleAddStep()
        {
            Step step = default;
            
            var renderer = GetStepRenderer();
            renderer.SetStep(step);
            _steps.Add(renderer);
        }

        private void HandleCommit()
        {
            var steps = _steps.ConvertAll<Step>(r => r.GetStep());
            _cycle.SetSteps(steps);
            _trafficController.Restart();
        }

        private StepRenderer GetStepRenderer()
        {
            if (_stepsPool.Count == 0)
            {
                var renderer = Instantiate(_stepPrefab);
                renderer.transform.SetParent(_stepsContainer);
                renderer.Initialize(OnStepDelete);
                return renderer;
            }
            else
            {
                return _stepsPool.Pop();
            }
        }

        private void OnStepDelete(StepRenderer renderer)
        {
            _stepsPool.Push(renderer);
            _steps.Remove(renderer);
        }

    }
}
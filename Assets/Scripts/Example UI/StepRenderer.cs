using TrafficLight;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using System;

namespace Example.UI
{
    public sealed class StepRenderer : MonoBehaviour
    {

        [SerializeField] private Dropdown _signalDropdown = default;
        [SerializeField] private InputField _durationInput = default;
        [SerializeField] private Button _deleteButton = default;

        private bool _initialized = false;
        private Action<StepRenderer> _onDeleted;

        public void Initialize(Action<StepRenderer> onDeleted)
        {
            if (_initialized == false)
            {
                _signalDropdown.ClearOptions();
                var signals = Enum.GetValues(typeof(Signal)).Cast<Signal>().ToList();
                var options = signals.ConvertAll<string>(s => s.ToString());
                _signalDropdown.AddOptions(options);

                _deleteButton.onClick.AddListener(HandleDeleteClick);
                
                _initialized = true;
                _onDeleted = onDeleted;
            }
        }

        public void SetStep(Step step)
        {
            gameObject.SetActive(true);
            _signalDropdown.value = (int)step.Signal;
            _durationInput.text = step.Duration.ToString();
        }

        public Step GetStep()
        {
            return new Step() {
                Signal = (Signal)_signalDropdown.value,
                Duration = float.Parse(_durationInput.text)
            };
        }

        private void HandleDeleteClick()
        {
            gameObject.SetActive(false);
            _onDeleted?.Invoke(this);
        }
    }
}
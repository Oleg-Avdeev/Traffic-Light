using UnityEngine;

namespace TrafficLight
{
    public class TrafficSignal : MonoBehaviour
    {
        [SerializeField] private Signal _signal = default;
        [SerializeField] private Color _color = default;
        [SerializeField] private TextMesh _text = default;
        [SerializeField] private MeshRenderer _meshRenderer = default;
        
        private const string _materialActiveProperty = "_Active";
        private MaterialPropertyBlock _props;
        private float _timeLeft;
        private bool _blinking;

        public void Activate(Step step)
        {
            bool active = _signal == step.Signal;
            
            if (_props == null)
            {
                _props  = new MaterialPropertyBlock();
                _props.SetColor("_Color", _color);
            }

            _props.SetFloat("_Blinking", 0);
            _props.SetFloat("_Active", active ? 1 : 0);
            _meshRenderer.SetPropertyBlock(_props);

            _text.gameObject.SetActive(active);
         
            if (active)
            {
                _text.text = step.Signal.ToString();
                _text.color = _color;

                _blinking = false;
                _timeLeft = step.Duration;
            }
        }

        private void FixedUpdate()
        {
            if (_timeLeft > 0)
            {

                _timeLeft -= Time.fixedDeltaTime;

                if (!_blinking && _timeLeft < 3f)
                {
                    _blinking = true;
                    _props.SetFloat("_Blinking", 1);
                    _meshRenderer.SetPropertyBlock(_props);
                }
            }
        }
    }
}

using UnityEngine;

namespace _Project.Code.Core.CustomAttributes
{
    public class FloatRangeSliderAttribute : PropertyAttribute
    {
        public float Min { get; private set; }

        public float Max { get; private set; }

        public FloatRangeSliderAttribute(float min, float max)
        {
            Min = min;
            Max = max < min ? min : max;
        }
    }

    [System.Serializable]
    public struct FloatRange
    {
        [SerializeField] private float _min;
        [SerializeField] private float _max;

        public float Min
        {
            get => _min;
            set => _min = value;
        }

        public float Max
        {
            get => _max;
            set => _max = value;
        }
        public FloatRange(float min, float max)
        {
            _min = min;
            _max = max < min ? min : max;
        }

        public FloatRange(float value)
        {
            _min = _max = value;
        }
    }
}
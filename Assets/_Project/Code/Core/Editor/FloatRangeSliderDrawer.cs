using _Project.Code.Core.CustomAttributes;
using UnityEditor;
using UnityEngine;

namespace _Project.Code.Core.Editor
{
[CustomPropertyDrawer(typeof(FloatRangeSliderAttribute))]
    public class FloatRangeSliderDrawer : PropertyDrawer
    {
        private Rect _minValueRect, _maxValueRect, _sliderRect;
        private SerializedProperty _propertyMinValue, _propertyMaxValue;

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            position = EditorGUI.PrefixLabel(
                position, GUIUtility.GetControlID(FocusType.Passive), label
            );
            float valueFieldWidth = position.width / 4 - 4f;
            float sliderWidth = position.width / 2;

            _minValueRect = new Rect(position.x, position.y, valueFieldWidth, position.height);

            _sliderRect = new Rect(position.x + valueFieldWidth + 4f,
                position.y, sliderWidth, position.height);

            _maxValueRect = new Rect(position.x + sliderWidth + valueFieldWidth + 4f,
                position.y, valueFieldWidth, position.height);

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                _propertyMinValue = property.FindPropertyRelative("_min");
                _propertyMaxValue = property.FindPropertyRelative("_max");
                
                float minValue = _propertyMinValue.floatValue;
                float maxValue = _propertyMaxValue.floatValue;
                
                FloatRangeSliderAttribute limit = attribute as FloatRangeSliderAttribute;
                
                minValue = EditorGUI.FloatField(_minValueRect, minValue);
                maxValue = EditorGUI.FloatField(_maxValueRect, maxValue);
                EditorGUI.MinMaxSlider(_sliderRect, ref minValue,
                    ref maxValue, limit.Min, limit.Max);
                
                if (minValue < limit.Min) {
                    minValue = limit.Min;
                }
                if (maxValue < minValue) {
                    maxValue = minValue;
                }
                else if (maxValue > limit.Max) {
                    maxValue = limit.Max;
                }
                
                _propertyMinValue.floatValue = minValue;
                _propertyMaxValue.floatValue = maxValue;
            }
        }
    }
}
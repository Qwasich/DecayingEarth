using UnityEngine;

namespace Racist
{
    public abstract class Setting : ScriptableObject
    {
        [SerializeField] protected string m_Title;
        public string Title => m_Title;

        public virtual bool isMinValue { get; }
        public virtual bool isMaxValue { get; }

        public virtual void SetNextValue() { }
        public virtual void SetPreviousValue() { }
        public virtual object GetValue() { return default(object); }
        public virtual string GetStringValue() { return string.Empty; }

        public virtual void Apply() { }
        public virtual void Load() { }
    }
}

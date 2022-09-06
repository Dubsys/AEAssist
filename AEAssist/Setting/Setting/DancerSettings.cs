using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class DancerSettings : IBaseSetting
    {
        public DancerSettings()
        {
            Reset();
        }

        public bool EarlyDecisionMode { get; set; }

        public bool UseDanceOnlyInRange { get; set; } = false;

        public bool TechFirst { get; set; } = false;
        public void Reset()
        {
            EarlyDecisionMode = true;
            UseDanceOnlyInRange = false;
            TechFirst = false;
        }

        public void OnLoad()
        {

        }
    }
}
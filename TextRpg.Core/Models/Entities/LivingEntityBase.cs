using TextRpg.Core.Models.Enums;

namespace TextRpg.Core.Models.Entities
{
    public abstract class LivingEntityBase
    {
        public float CurrentHealth { get; set; }
        public float CurrentSpecialResource { get; set; }

        public virtual Dictionary<BaseStat, float> Stats { get; } = [];

        public Dictionary<BaseStat, float> CalculatedStats { get; private set; } = [];

        public virtual float MaxHealth => CalculatedStats.TryGetValue(BaseStat.MaxHealth, out var maxHealth) ? maxHealth : 0;
        public virtual float MaxSpecialResource => CalculatedStats.TryGetValue(BaseStat.MaxSpecialResource, out var maxResource) ? maxResource : 0;

        protected LivingEntityBase(float currentHealth, float currentSpecialResource)
        {
            CurrentHealth = currentHealth;
            CurrentSpecialResource = currentSpecialResource;
        }

        protected LivingEntityBase()
        {
            CurrentHealth = MaxHealth;
            CurrentSpecialResource = MaxSpecialResource;
        }

        public void SetCalculatedStats(Dictionary<BaseStat, float> newStats)
        {
            CalculatedStats = newStats;
        }
    }

}

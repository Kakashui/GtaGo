namespace Whistler.Core.Pets.Models
{
    public class PetConfig
    {
        public uint Model;
        public string Name;
        public int MaxHealth;
        public int Damage;
        public int Heal;
        public int Price;
        public bool ImmuneToPlayerDamage = false;

        public PetConfig(uint model, string name, int maxHealth = 100, int damage = 0, int heal = 0, int price = 0, bool immuneToPlayerDamage = false)
        {
            Model = model;
            Name = name;
            MaxHealth = maxHealth;
            Damage = damage;
            Price = price;
            Heal = heal;
            ImmuneToPlayerDamage = immuneToPlayerDamage;
        }
    }
}

namespace Whistler.Houses
{
    public class HouseBlipType
    {
        public uint Sprite { get; }
        public int Color { get; }
        public float Scale { get; }
        public string Name { get; }


        public HouseBlipType(uint sprite, int color, float scale, string name)
        {
            Sprite = sprite;
            Color = color;
            Scale = scale;
            Name = name;
        }
    }
}
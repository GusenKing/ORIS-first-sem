using System.Drawing;

namespace PointGame.Packages
{
    public class AddUser
    {
        public string Name { get; set; }

        public Color Color { get; set; }

        public AddUser(string name, Color color)
        {
            Name = name;
            Color = color;
        }

        public AddUser(string name)
        {
            Name = name;
        }
    }
}

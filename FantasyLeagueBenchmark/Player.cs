using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FantasyLeagueBenchmark
{
    public class Player
    {
        public String Name { get; set; }
        public String Position { get; set; }
        public String Team { get; set; }
        public double Price { get; set; }
        public int Points { get; set; }
        public int PercPicked { get; set; }
        public int Seq { get; set; }
        public int IsPicked { get; set; }
        public int Pickable { get; set; }
    }

    public class PlayerComp : IComparer<Player>
    {
        public int Compare(Player x, Player y)
        {
            if (x.Pickable.CompareTo(y.Pickable) != 0)
            {
                return -x.Pickable.CompareTo(y.Pickable);
            }
            else if (x.PercPicked.CompareTo(y.PercPicked) != 0)
            {
                return -x.PercPicked.CompareTo(y.PercPicked);
            }
            else if (x.Price.CompareTo(y.Price) != 0)
            {
                return x.Price.CompareTo(y.Price);
            }
            else if (x.Points.CompareTo(y.Points) != 0)
            {
                return -x.Points.CompareTo(y.Points);
            }
            else
            {
                return 0;
            }
        }
    }
}

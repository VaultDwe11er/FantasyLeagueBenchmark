using System;
using System.Collections.Generic;

namespace FantasyLeagueBenchmark
{
    public class Player
    {
        public String Name { get; set; }
        public String Position { get; set; }
        public String Team { get; set; }
        public double Price { get; set; }
        public int Points { get; set; }
        public double PercPicked { get; set; }
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
            else if (x.Name.CompareTo(y.Name) != 0)
            {
                return x.Name.CompareTo(y.Name);
            }
            else
            {
                return 0;
            }
        }
    }

    public class Pro14Json
    {
        public Pro14Player[] players { get; set; }
    }

    public class Pro14Player
    {
        public int categoryId { get; set; }
        public string code { get; set; } //This should actually be int, but one of the values is "WRONG"
        public string fullName { get; set; }
        public string info1 { get; set; }
        public int lastSeasonPoints { get; set; }
        public int lastWeekPoints { get; set; }
        public string name { get; set; }
        public int nextFixtureId { get; set; }
        public int sideId { get; set; }
        public int teams_selected { get; set; }
        public float value { get; set; }
        public int totalPoints { get; set; }
    }

    public class FoxJson
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int squad_id { get; set; }
        public int cost { get; set; }
        public string status { get; set; }
        public FoxPlayerStats stats { get; set; }
        public int[] positions { get; set; }
        public int is_bye { get; set; }
        public int locked { get; set; }
    }

    public class FoxPlayerStats
    {
        //public object scores { get; set; }
        //public object match_scores { get; set; }
        public int round_rank { get; set; }
        public int season_rank { get; set; }
        public int games_played { get; set; }
        public int total_points { get; set; }
        public double avg_points { get; set; }
        public int high_score { get; set; }
        public double last_3_avg { get; set; }
        public double last_5_avg { get; set; }
        public int selections { get; set; }
        public double owned_by { get; set; }
    }
}

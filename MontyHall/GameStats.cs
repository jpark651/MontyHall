using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MontyHall
{
    class GameStats
    {
        public int GamesWon { get { return SwitchedGamesWon + KeptGamesWon; } }
        public int GamesLost { get { return SwitchedGamesLost + KeptGamesLost; } }
        public int GamesTotal { get { return GamesLost + GamesWon; } }
        public int SwitchedGamesWon;
        public int SwitchedGamesLost;
        public int SwitchedGamesTotal { get { return SwitchedGamesLost + SwitchedGamesWon; } }
        public int KeptGamesWon;
        public int KeptGamesLost;
        public int KeptGamesTotal { get { return KeptGamesLost + KeptGamesWon; } }
        public double PercentWins { get { return Percent(GamesWon, GamesTotal); } }
        public double PercentSwitchedWins { get { return Percent(SwitchedGamesWon, SwitchedGamesTotal); } }
        public double PercentKeptWins { get { return Percent(KeptGamesWon, KeptGamesTotal); } }

        public GameStats()
        {
            SwitchedGamesWon = 0;
            SwitchedGamesLost = 0;
            KeptGamesWon = 0;
            KeptGamesLost = 0;
        }

        private double Percent(int partial, int total)
        {
            if (total == 0)
            {
                return 0;
            }
            return (((double)partial / (double)total) * 100);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    class Player3 : Player
    {
        public Player3(int idNum, string nm, int mny) : base(idNum, nm, mny)
        {
        }

        public override PlayerAction BettingRound1(List<PlayerAction> actions, Card[] hand)
        {
            // TODO: Implement Betting Round 1 Algorithm
            PlayerAction playerAction = new PlayerAction(Name, "Bet1", "bet", 0);

            return playerAction;
        }

        public override PlayerAction Draw(Card[] hand)
        {
            // TODO: Implement Drawing Algorithm
            return new PlayerAction(Name, "Draw", "stand pat", 0);
        }

        public override PlayerAction BettingRound2(List<PlayerAction> actions, Card[] hand)
        {
            // TODO: Implement Betting Round 2 Algorithm
            PlayerAction playerAction = new PlayerAction(Name, "Bet2", "bet", 0);

            return playerAction;
        }



    }
}

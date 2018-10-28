using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerTournament
{
    class Player3 : Player
    {
        private List<PlayerAction> previousActions;
        public Player3(int idNum, string nm, int mny) : base(idNum, nm, mny)
        {
        }


        public override PlayerAction BettingRound1(List<PlayerAction> actions, Card[] hand)
        {
            // select an action
            string actionSelection = "";
            PlayerAction pa = null;

            float hS = HandStrength(hand);
            float maxBet = this.Money * hS;

            do
            {
                //Console.WriteLine("Select an action:\n1 - bet\n2 - raise\n3 - call\n4 - check\n5 - fold");

                hS = HandStrength(hand);

                var hasP = HasPairs(hand);
                var hasF = HasFlush(hand);
                var longS = LongestStraight(hand);

                float[] evaluation = new float[5];
                // Evaluate A Card
                for (int i = 0; i < hand.Length; ++i)
                {
                    evaluation[i] = 11.27f * hasP[i] + 2.64f * hasF[i] +
                                    0.86f * longS[i];
                }

                if (actions.Count == 0)
                {
                    if (hS < 0.1 && evaluation.Average() < 11.3f)
                    {
                        actionSelection = "5";
                    }
                    else if (hS > 0.95f)
                    {
                        actionSelection = "1";
                    }
                    else
                    {
                        actionSelection = "4";
                    }
                }
                else
                {

                    previousActions = actions;
                    PlayerAction prev = previousActions[previousActions.Count - 1];

                    if (prev.ActionName == "check")
                    {
                        if (hS >= 0.4f)
                        {
                            actionSelection = "1";
                        }
                        if (hS < 0.4f)
                        {
                            actionSelection = "4";
                        }
                    }

                    if (prev.ActionName == "bet")
                    {
                        int bet = prev.Amount;

                        if (hS == 1.0f)
                        {
                            actionSelection = "1";
                        }
                        if (hS > 0.95f)
                        {
                            actionSelection = "1";
                        }
                        if (hS >= 0.3f && hS <= 0.95f && bet < maxBet)
                        {
                            actionSelection = "3";
                        }
                        else if (bet > maxBet)
                        {
                            actionSelection = "5";
                        }
                        else
                        {
                            actionSelection = "3";
                        }
                        if (hS <= 0.1f)
                        {
                            actionSelection = "5";
                        }
                        else if (hS < 0.3f)
                        {
                            actionSelection = "1";
                        }
                    }

                    if (prev.ActionName == "raise")
                    {
                        int bet = prev.Amount;

                        // call, raise, or bet
                        if (hS == 1.0f)
                        {
                            actionSelection = "1";
                        }
                        if (hS >= 0.95f && bet <= maxBet)
                        {
                            actionSelection = "2";
                        }
                        else if (hS >= 0.7f && bet <= maxBet)
                        {
                            actionSelection = "3";
                        }
                        else if (hS < 0.7f && hS > 0.1f)
                        {
                            actionSelection = "1";
                        }
                        if (hS <= 0.1f)
                        {
                            actionSelection = "5";
                        }

                    }

                    if (prev.ActionName == "call")
                    {
                        // should never happen?
                        actionSelection = "3";
                    }

                    if (prev.ActionName == "draw" || prev.ActionName == "stand pat")
                    {
                        if (hS == 1.0f)
                        {
                            actionSelection = "1";
                        }
                        else if (hS >= 0.95f)
                        {
                            actionSelection = "1";
                        }
                        else
                        {
                            actionSelection = "4";
                        }
                    }
                }



                //actionSelection = Console.ReadLine();

                // get amount if appropriate
                int amount = 0;
                if (actionSelection[0] == '1' || actionSelection[0] == '2')
                {
                    string amtText = "";
                    do
                    {
                        if (actionSelection[0] == '1') // bet
                        {
                            //Console.Write("Amount to bet? ");
                            //amtText = Console.ReadLine();
                            if (hS == 1.0f)
                            {
                                amtText = this.Money.ToString();
                            }
                            else if (hS >= 0.95)
                            {
                                amtText = ((int)(maxBet * hS)).ToString();
                            }
                            else
                            {
                                amtText = "1";
                            }
                        }
                        else if (actionSelection[0] == '2') // raise
                        {
                            //Console.Write("Amount to raise? ");
                            if (hS == 1.0f)
                            {
                                amtText = this.Money.ToString();
                            }
                            else if (hS >= 0.95)
                            {
                                amtText = ((int)maxBet * hS - previousActions[actions.Count - 1].Amount).ToString();
                            }
                            else
                            {
                                amtText = "1";
                            }

                            //amtText = Console.ReadLine();
                        }
                        // convert the string to an int
                        int tempAmt = 0;
                        int.TryParse(amtText, out tempAmt);

                        // check input
                        if (tempAmt > this.Money) //
                        {
                            //Console.WriteLine("Amount bet is more than the amount you have available.");
                            amount = 0;
                        }
                        else if (tempAmt < 0)
                        {
                            //Console.WriteLine("Amount bet or raised cannot be less than zero.");
                            amount = 0;
                        }
                        else
                        {
                            amount = tempAmt;
                        }
                    } while (amount <= 0);
                }

                // create the PlayerAction
                switch (actionSelection)
                {
                    case "1": pa = new PlayerAction(Name, "Bet1", "bet", amount); break;
                    case "2": pa = new PlayerAction(Name, "Bet1", "raise", amount); break;
                    case "3": pa = new PlayerAction(Name, "Bet1", "call", amount); break;
                    case "4": pa = new PlayerAction(Name, "Bet1", "check", amount); break;
                    case "5": pa = new PlayerAction(Name, "Bet1", "fold", amount); break;
                    default: /*Console.WriteLine("Invalid menu selection - try again");*/ continue;
                }
            } while (actionSelection != "1" && actionSelection != "2" &&
                    actionSelection != "3" && actionSelection != "4" &&
                    actionSelection != "5");
            // return the player action
            return pa;
        }

        public override PlayerAction BettingRound2(List<PlayerAction> actions, Card[] hand)
        {
            PlayerAction pa1 = BettingRound1(actions, hand);

            // create a new PlayerAction object
            return new PlayerAction(pa1.Name, "Bet2", pa1.ActionName, pa1.Amount);

        }

        public override PlayerAction Draw(Card[] hand)
        {
            var hasP = HasPairs(hand);
            var hasF = HasFlush(hand);
            var longS = LongestStraight(hand);

            float[] evaluation = new float[5];
            // Evaluate A Card
            for (int i = 0; i < hand.Length; ++i)
            {
                evaluation[i] = 11.27f * hasP[i] + 2.64f * hasF[i] +
                                0.86f * longS[i];
            }

            int cardsToDelete = 0;

            // Remove Cards
            for (int i = 0; i < hand.Length; ++i)
            {
                //Console.Write(hand[i].ToString() + "\t");
                //Console.Write(hasP[i] + "\t");
                //Console.Write(hasF[i] + "\t");
                //Console.Write(longS[i] + "\t");

                if (evaluation[i] < evaluation.Average())
                {
                    //Console.WriteLine("Removed");
                    hand[i] = null;
                    ++cardsToDelete;
                }
            }
            return new PlayerAction(Name, "Draw", cardsToDelete == 0 ? "stand pat" : "draw", cardsToDelete);
        }



        private int[] HasPairs(Card[] hand)
        {
            bool[] boolResult = new bool[hand.Length];
            int[] count = new int[13];

            for (int i = 0; i < hand.Length; ++i)
                count[hand[i].Value - 2] = count[hand[i].Value - 2] * (hand.Length + 1) + i + 1;

            for (int i = 0; i < count.Length; ++i)
            {
                if (count[i] <= 0) continue;
                int c = (int)Math.Log(count[i], hand.Length + 1);
                if (c > 0)
                    while (count[i] > 0)
                    {
                        int card = count[i] % (hand.Length + 1) - 1;
                        count[i] /= hand.Length + 1;
                        boolResult[card] = true;
                    }
            }

            int[] result = new int[hand.Length];
            int trueCount = boolResult.Count(t => t);

            for (int i = 0; i < result.Length; ++i)
            {
                if (boolResult[i]) result[i] = trueCount;
            }

            return result;
        }

        private int[] HasFlush(Card[] hand)
        {
            int[] result = new int[hand.Length];
            Dictionary<String, int> count = new Dictionary<string, int>();
            Dictionary<String, int> suitIndex = new Dictionary<string, int>();
            string[] suits = { "Hearts", "Clubs", "Diamonds", "Spades" };
            for (int i = 0; i < suits.Length; i++)
            {
                count[suits[i]] = 0;
                suitIndex[suits[i]] = i;
            }
            for (int i = 0; i < hand.Length; ++i)
                count[hand[i].Suit] = count[hand[i].Suit] * (hand.Length + 1) + i + 1;

            foreach (KeyValuePair<string, int> i in count)
            {
                var value = i.Value;
                if (value <= 0) continue;
                int c = (int)Math.Log(value, hand.Length + 1);
                if (c > 0)
                    while (value > 0)
                    {
                        int card = value % (hand.Length + 1) - 1;
                        value /= hand.Length + 1;
                        result[card] = c + 1;
                    }
            }

            return result;
        }

        struct SortedCard : IComparable<SortedCard>
        {
            public readonly Card Card;
            public readonly int IndexInHand;

            public SortedCard(Card card, int index)
            {
                Card = card;
                IndexInHand = index;
            }

            public int CompareTo(SortedCard other)
            {
                return Card.Value.CompareTo(other.Card.Value);
            }
        }

        private int[] LongestStraight(Card[] hand)
        {
            bool[] boolResult = new bool[hand.Length];
            SortedCard[] c = new SortedCard[hand.Length];
            for (int i = 0; i < c.Length; ++i)
                c[i] = new SortedCard(hand[i], i);

            Array.Sort(c);
            //c = c.Distinct().ToArray();

            int[] d = new int[c.Length - 1];
            for (int i = 0; i < d.Length; ++i)
                d[i] = c[i + 1].Card.Value - c[i].Card.Value;
            int[] a = new int[d.Length];

            for (int i = 0; i < d.Length; ++i)
            {
                int k = 0;
                for (int j = i + 1; j < d.Length; ++j)
                {
                    //if (d[j] == 0) continue;
                    if (d[i] + d[j] > 4) break;
                    d[i] += d[j];
                    ++k;
                }

                a[i] = k;
            }

            int maxInA = -1;
            int maxIndexInA = -1;
            for (int i = 0; i < a.Length; ++i)
            {
                if (a[i] >= maxInA)
                {
                    maxInA = a[i];
                    maxIndexInA = i;
                }
            }

            if (maxInA == -1 && maxIndexInA == -1)
            {
                int[] result_ = new int[hand.Length];
                int trueCount_ = boolResult.Count(t => t);

                for (int i = 0; i < result_.Length; ++i)
                {
                    if (boolResult[i]) result_[i] = trueCount_;
                }

                return result_;
            }

            Dictionary<String, int> suitCount = new Dictionary<string, int>();
            string[] suits = { "Hearts", "Clubs", "Diamonds", "Spades" };
            foreach (var t in suits)
                suitCount[t] = 0;


            for (int i = maxIndexInA; i < maxIndexInA + maxInA + 2; ++i)
            {
                boolResult[c[i].IndexInHand] = true;
                ++suitCount[hand[c[i].IndexInHand].Suit];
            }

            string maxAppearedSuit = suits[0];
            int maxSuitAppearTime = -1;
            foreach (KeyValuePair<string, int> i in suitCount)
            {
                if (i.Value > maxSuitAppearTime)
                {
                    maxSuitAppearTime = i.Value;
                    maxAppearedSuit = i.Key;
                }
            }

            for (int i = 0; i < boolResult.Length; ++i)
            {
                if (!boolResult[i]) continue;
                for (int j = 0; j < boolResult.Length; ++j)
                {
                    if (!boolResult[j] || i == j) continue;
                    if (hand[i].Value != hand[j].Value) continue;
                    if (hand[j].Suit != maxAppearedSuit)
                        boolResult[j] = false;
                }
            }

            int[] result = new int[hand.Length];
            int trueCount = boolResult.Count(t => t);

            for (int i = 0; i < result.Length; ++i)
            {
                if (boolResult[i]) result[i] = trueCount;
            }

            return result;
        }

        private float HandStrength(Card[] hand)
        {
            List<int> vals = new List<int>();
            List<string> suit = new List<string>();

            foreach (Card c in hand)
            {
                vals.Add(c.Value);
                suit.Add(c.Suit);
            }

            int highCard;
            int pairVal = 0;
            bool onePair = false;
            bool twoPair = false;
            bool threeKind = false;
            bool straight = false;
            bool flush = false;
            bool fullHouse = false;
            bool fourKind = false;

            highCard = vals.Max();

            vals.Sort((a, b) => a.CompareTo(b));

            int numSame = 0;
            int uniques = vals.Distinct().Count();
            for (int i = 0; i < vals.Count; i++)
            {
                for (int j = i + 1; j < vals.Count; j++)
                {
                    if (vals[i] == vals[j])
                    {
                        numSame++;
                        pairVal = vals[i];
                    }
                }
            }

            switch (numSame)
            {
                case 5:
                    fullHouse = true;
                    break;
                case 4: // can also be 2 pair
                    if (uniques == 3)
                    {
                        twoPair = true;
                    }
                    else
                    {
                        fourKind = true;
                    }
                    break;
                case 3:
                    threeKind = true;
                    break;
                case 2:
                    onePair = true;
                    break;
                default:
                    break;
            }

            if (vals[0] + 1 == vals[1] &&
                vals[1] + 1 == vals[2] &&
                vals[2] + 1 == vals[3] &&
                vals[3] + 1 == vals[4])
            {
                straight = true;
            }

            if (suit.Distinct().Count() == 1)
            {
                flush = true;
            }

            if (straight && flush)
            {
                if (highCard == 14)
                {
                    return 1.0f;
                }
                return 1.0f - 0.00000154f;
            }

            if (fourKind)
            {
                return 1.0f - 0.000015f;
            }

            if (fullHouse)
            {
                return 1.0f - 0.000256f;
            }

            if (flush)
            {
                return 1.0f - 0.0017f;
            }

            if (straight)
            {
                return 1.0f - 0.00367f;
            }

            if (threeKind)
            {
                return 1.0f - (0.0076f + (14.0f - (float)pairVal) * 0.00162523077f);
            }

            if (twoPair)
            {
                return 1.0f - (0.0287f + (14.0f - (float)pairVal) * 0.00365684615f);
            }

            if (onePair)
            {
                return 1.0f - (0.0762f + (14.0f - (float)pairVal) * 0.0325053077f);
            }

            return 1.0f - (0.499f + (14.0f - (float)highCard) * 0.0385520769f);
        }
    }



}


using System;
using System.Collections.Generic;
using System.Linq;

namespace ImperaPlus.Domain.Services.Scoring
{
    internal class Config
    {
        public const int DefaultRating = 1500;

        public const double DefaultRD = 350.0;

        public const double DefaultVol = 0.06;

        public const double Scale = 173.7177928;

        public const double Tau = 0.5;
    }

    public class ScoreTeam
    {
        public ScoreTeam()
        {
            Players = new List<ScorePlayer>();
        }

        public List<ScorePlayer> Players { get; set; }
    }

    public class ScorePlayer
    {
        public static ScorePlayer DefaultPlayer(string id)
        {
            return new ScorePlayer(id);
        }

        public ScorePlayer()
            : this(null, Config.DefaultRating, Config.DefaultRD, Config.DefaultVol)
        {
        }

        public ScorePlayer(string id)
            : this(id, Config.DefaultRating, Config.DefaultRD, Config.DefaultVol)
        {
        }

        public ScorePlayer(string id, int rating, double rd, double vol)
        {
            Id = id;

            Rating = rating;
            Rd = rd;
            Vol = vol;
        }

        public string Id { get; private set; }

        public double Rating { get; set; }

        public double Rd { get; set; }

        public double Vol { get; set; }

        internal double RdScaled
        {
            get => Rd / Config.Scale;

            set => Rd = value * Config.Scale;
        }

        internal double RatingScaled
        {
            get => (Rating - Config.DefaultRating) / Config.Scale;

            set => Rating = value * Config.Scale + Config.DefaultRating;
        }

        public static ScorePlayer operator +(ScorePlayer lhs, ScorePlayer rhs)
        {
            return new ScorePlayer { Rd = lhs.Rd + rhs.Rd, Vol = lhs.Vol + rhs.Vol, Rating = lhs.Rating + rhs.Rating };
        }

        public static ScorePlayer operator /(ScorePlayer lhs, double divisor)
        {
            return new ScorePlayer { Rd = lhs.Rd / divisor, Vol = lhs.Vol / divisor, Rating = lhs.Rating / divisor };
        }
    }

    public class Glicko2
    {
        private double G(double rd)
        {
            return 1.0 / Math.Sqrt(1 + 3.0 * rd * rd / (Math.PI * Math.PI));
        }

        private double E(double rating1, double rating2, double rd2)
        {
            return 1.0 / (1.0 + Math.Exp(-G(rd2) * (rating1 - rating2)));
        }

        private double F(double delta, double a, double rd, double v, double x)
        {
            var ex = Math.Pow(Math.E, x);

            var t1 = ex * (delta * delta - rd * rd - v - ex) / Math.Pow(2 * (rd * rd + v + ex), 2);

            var t2 = (x - a) / (Config.Tau * Config.Tau);

            return t1 - t2;
        }

        /// <summary>
        /// Calculcate score, modify players inline
        /// </summary>
        /// <returns>
        /// List of updated teams. First entry is winning team
        /// </returns>
        public IEnumerable<ScoreTeam> Calculate(ScoreTeam winningTeam, params ScoreTeam[] others)
        {
            var winningTeamPlayer = winningTeam.Players.Aggregate((a, b) => a + b) / (double)winningTeam.Players.Count;
            var otherPlayers = others.Select(t => t.Players.Aggregate((a, b) => a + b) / (double)t.Players.Count);

            var updatedWinningTeam = new ScoreTeam();

            foreach (var winningPlayer in winningTeam.Players)
            {
                var updatedWinningPlayer = Calculate(winningPlayer, otherPlayers.Select(p => Tuple.Create(p, 1.0)),
                    1.0 / winningTeam.Players.Count);

                updatedWinningTeam.Players.Add(updatedWinningPlayer);
            }

            var updatedLosingTeams = new List<ScoreTeam>();
            foreach (var otherTeam in others)
            {
                var updatedLosingTeam = new ScoreTeam();

                foreach (var player in otherTeam.Players)
                {
                    var otherLosingTeamPlayers = others.Where(x => x != otherTeam)
                        .Select(t => t.Players.Aggregate((a, b) => a + b) / (double)t.Players.Count);
                    var opponents =
                        new[] { Tuple.Create(winningTeamPlayer, 0.0) }.Concat(
                            otherLosingTeamPlayers.Select(x => Tuple.Create(x, 0.5)));

                    var updatedOtherPlayer = Calculate(player, opponents, 1.0 / otherTeam.Players.Count);
                    updatedLosingTeam.Players.Add(updatedOtherPlayer);
                }

                updatedLosingTeams.Add(updatedLosingTeam);
            }

            return new[] { updatedWinningTeam }.Concat(updatedLosingTeams);
        }

        public ScorePlayer Calculate(ScorePlayer player, IEnumerable<Tuple<ScorePlayer, double>> opponents,
            double factor = 1.0)
        {
            // Step 3
            var v = 0.0;
            foreach (var opponent in opponents)
            {
                var e = E(player.RatingScaled, opponent.Item1.RatingScaled, opponent.Item1.RdScaled);
                v += Math.Pow(G(opponent.Item1.RdScaled), 2) * e * (1 - e);
            }

            v = Math.Pow(v, -1.0);

            // Step 4
            var delta = 0.0;
            foreach (var opponent in opponents)
            {
                delta += G(opponent.Item1.RdScaled) * (opponent.Item2 - E(player.RatingScaled,
                    opponent.Item1.RatingScaled, opponent.Item1.RdScaled));
            }

            delta *= v;

            // Step 5
            const double epsilon = 0.000001;

            double A, a, B;
            A = a = Math.Log(Math.Pow(player.Vol, 2));

            var delta2 = delta * delta;
            var rd2 = player.RdScaled * player.RdScaled;
            if (delta2 > rd2 + v)
            {
                B = Math.Log(delta2 - rd2 - v);
            }
            else
            {
                var k = 0;
                while (F(delta, a, player.RdScaled, v, a - k * Config.Tau) < 0)
                {
                    ++k;
                }

                B = a - k * Config.Tau;
            }

            // Step 5.3
            var fa = F(delta, a, player.RdScaled, v, A);
            var fb = F(delta, a, player.RdScaled, v, B);

            // Step 5.4
            while (Math.Abs(B - A) > epsilon)
            {
                var C = A + (A - B) * fa / (fb - fa);
                var fc = F(delta, a, player.RdScaled, v, C);

                if (fc * fb < 0)
                {
                    A = B;
                    fa = fb;
                }
                else
                {
                    fa = fa / 2.0;
                }

                B = C;
                fb = fc;
            }

            // Step 5.5
            var vol1 = Math.Pow(Math.E, A / 2.0);

            // Step 6
            var rdstar = Math.Sqrt(rd2 + vol1 * vol1);

            // Step 7
            var rdnew = 1.0 / Math.Sqrt(1 / (rdstar * rdstar) + 1 / v);
            var ratingnew = 0.0;
            foreach (var opponent in opponents)
            {
                ratingnew += rdnew * rdnew * G(opponent.Item1.RatingScaled) * (opponent.Item2 -
                                                                               E(player.RatingScaled,
                                                                                   opponent.Item1.RatingScaled,
                                                                                   opponent.Item1.RdScaled));
            }

            // Step 8
            var result = new ScorePlayer(player.Id);

            result.RatingScaled = player.RatingScaled + ratingnew * factor;
            result.RdScaled = (rdnew - player.RdScaled) * factor + player.RdScaled;

            return result;
        }
    }
}

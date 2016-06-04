using ImperaPlus.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ImperaPlus.Domain.Games.History
{
    public class HistoryGameTurn
    {
        public long TurnNo { get; set; }

        public Game Game { get; set; }

        public IEnumerable<HistoryEntry> Actions { get; set; }
    }
   
    public class GameHistory
    {
        private GameHistory()
        {
            
        }

        public GameHistory(Game game)
            : this()
        {
            this.Game = game;
        }

        public long GameId { get; set; }
        public virtual Game Game { get; private set; }

        public void RecordPlace(Player player, string countryIdentifier, int units)
        {
            this.AddEntry(new HistoryEntry(this.Game, player, HistoryAction.PlaceUnits, this.Game.TurnCounter)
            {
                OriginIdentifier = countryIdentifier,
                Units = units
            });
        }

        public void RecordAttack(
            Player attackingPlayer, Player defendingPlayer,
            string originCountryIdentifier, string destinationCountryIdentifier,
            int units, int unitsLost, int defendingUnitsLost, bool result)
        {
            this.AddEntry(new HistoryEntry(this.Game, attackingPlayer, HistoryAction.Attack, this.Game.TurnCounter)
            {
                OtherPlayer = defendingPlayer,
                OriginIdentifier = originCountryIdentifier,
                DestinationIdentifier = destinationCountryIdentifier,
                Units = units,
                UnitsLost = unitsLost,
                UnitsLostOther = defendingUnitsLost,
                Result = result
            });
        }

        public void RecordMove(
            Player movingPlayer, string originCountryIdentifier, string destinationCountryIdentifier, int units)
        {
            this.AddEntry(new HistoryEntry(this.Game, movingPlayer, HistoryAction.Move, this.Game.TurnCounter)
            {
                OriginIdentifier = originCountryIdentifier,
                DestinationIdentifier = destinationCountryIdentifier,
                Units = units
            });
        }

        public void RecordStart()
        {
            this.AddEntry(new HistoryEntry(this.Game, null, HistoryAction.StartGame, this.Game.TurnCounter));
        }

        public void RecordEnd()
        {
            this.AddEntry(new HistoryEntry(this.Game, null, HistoryAction.EndGame, this.Game.TurnCounter));
        }

        public void RecordEndTurn()
        {
            this.AddEntry(new HistoryEntry(this.Game, this.Game.CurrentPlayer, HistoryAction.EndTurn, this.Game.TurnCounter));
        }

        public void RecordTimeout()
        {
            this.AddEntry(new HistoryEntry(this.Game, this.Game.CurrentPlayer, HistoryAction.PlayerTimeout, this.Game.TurnCounter));
        }

        public void RecordCardExchange(Player exchangingPlayer, int unitsReceived)
        {
            this.AddEntry(new HistoryEntry(this.Game, exchangingPlayer, HistoryAction.ExchangeCards, this.Game.TurnCounter)
            {
                Units = unitsReceived
            });
        }

        public void RecordPlayerDefeated(Player player)
        {
            this.AddEntry(new HistoryEntry(this.Game, player, HistoryAction.PlayerLost, this.Game.TurnCounter));
        }

        public void RecordPlayerWon(Player player)
        {
            this.AddEntry(new HistoryEntry(this.Game, player, HistoryAction.PlayerWon, this.Game.TurnCounter));
        }

        public Map GetMapForTurn(long turnNo)
        {
            if (turnNo < 0 || turnNo > this.Game.TurnCounter)
            {
                throw new DomainException(ErrorCode.TurnDoesNotExist, "Invalid turn requested");
            }

            var currentMap = this.Game.Map.Clone();

            var actions = this.Game.HistoryEntries.Where(x => x.TurnNo > turnNo).OrderByDescending(x => x.DateTime).OrderByDescending(x => x.Id);

            ApplyActionsToMap(currentMap, actions);

            return currentMap;
        }

        private static void ApplyActionsToMap(Map map, IEnumerable<HistoryEntry> actions)
        {
            foreach (var action in actions)
            {
                switch (action.Action)
                {
                    case HistoryAction.PlaceUnits:
                        {
                            map.GetCountry(action.OriginIdentifier).Units -= (int)action.Units;
                            break;
                        }

                    case HistoryAction.Attack:
                        {
                            var originCountry = map.GetCountry(action.OriginIdentifier);
                            var destinationCountry = map.GetCountry(action.DestinationIdentifier);

                            if (action.Result.HasValue && action.Result.Value)
                            {
                                // Attacker won, restore ownership
                                map.UpdateOwnership(action.OtherPlayer, destinationCountry);

                                originCountry.Units += (int)action.Units;
                                destinationCountry.Units = (int)action.UnitsLostOther;
                            }
                            else
                            {
                                // Defender won
                                originCountry.Units += (int)action.Units;
                                destinationCountry.Units += (int)action.UnitsLostOther;
                            }
                            break;
                        }

                    case HistoryAction.Move:
                        {
                            var originCountry = map.GetCountry(action.OriginIdentifier);
                            var destinationCountry = map.GetCountry(action.DestinationIdentifier);

                            originCountry.Units += (int)action.Units;
                            destinationCountry.Units -= (int)action.Units;

                            break;
                        }
                }
            }
        }

        public HistoryGameTurn GetTurn(long turnNo)
        {
            if (turnNo < 0 || turnNo > this.Game.TurnCounter)
            {
                throw new DomainException(ErrorCode.TurnDoesNotExist, "Invalid turn requested");
            }

            var actions = this.Game.HistoryEntries.Where(x => x.TurnNo > turnNo).OrderByDescending(x => x.DateTime).OrderByDescending(x => x.Id);

            foreach (var action in actions)
            {
                switch (action.Action)
                {
                    case HistoryAction.StartGame:
                        break;

                    case HistoryAction.EndGame:
                        break;

                    case HistoryAction.PlaceUnits:
                    case HistoryAction.Attack:
                    case HistoryAction.Move:
                        {
                            ApplyActionsToMap(this.Game.Map, new[] { action });
                            break;
                        }

                    case HistoryAction.ExchangeCards:
                        {
                            break;
                        }

                    case HistoryAction.PlayerLost:
                        {
                            var player = this.Game.GetPlayerById(action.Actor.Id);

                            player.State = Enums.PlayerState.Active;
                            player.Outcome = Enums.PlayerOutcome.None;

                            break;
                        }

                    case HistoryAction.PlayerWon:
                        {
                            var player = this.Game.GetPlayerById(action.Actor.Id);

                            player.State = Enums.PlayerState.Active;
                            player.Outcome = Enums.PlayerOutcome.None;

                            break;
                        }

                    case HistoryAction.PlayerTimeout:
                        {
                            var player = this.Game.GetPlayerById(action.Actor.Id);

                            --player.Timeouts;

                            break;
                        }

                    case HistoryAction.OwnerChange:
                        {
                            var country = this.Game.Map.GetCountry(action.OriginIdentifier);

                            this.Game.Map.UpdateOwnership(action.Actor, country);

                            break;
                        }

                    case HistoryAction.EndTurn:
                        {
                            break;
                        }

                    case HistoryAction.None:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return new HistoryGameTurn
            {
                TurnNo = turnNo,
                Game = this.Game,
                Actions = this.Game.HistoryEntries.Where(x => x.TurnNo == turnNo).OrderBy(x => x.DateTime)
            };
        }


        private void AddEntry(HistoryEntry historyEntry)
        {
            this.Game.HistoryEntries.Add(historyEntry);
        }
    }
}
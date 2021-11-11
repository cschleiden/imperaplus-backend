using ImperaPlus.Domain.Enums;
using ImperaPlus.Domain.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace ImperaPlus.Domain.Games.History
{
    public class HistoryGameTurn
    {
        public long TurnNo { get; set; }

        public virtual Game Game { get; set; }

        public IEnumerable<HistoryEntry> Actions { get; set; }
    }

    public class GameHistory
    {
        protected GameHistory()
        {
        }

        public GameHistory(Game game)
            : this()
        {
            Game = game;
        }

        public long GameId { get; set; }
        public virtual Game Game { get; private set; }

        public void RecordPlace(Player player, string countryIdentifier, int units)
        {
            AddEntry(new HistoryEntry(Game, player, HistoryAction.PlaceUnits, Game.TurnCounter)
            {
                OriginIdentifier = countryIdentifier, Units = units
            });
        }

        public void RecordAttack(
            Player attackingPlayer, Player defendingPlayer,
            string originCountryIdentifier, string destinationCountryIdentifier,
            int units, int unitsLost, int defendingUnitsLost, bool result)
        {
            AddEntry(new HistoryEntry(Game, attackingPlayer, HistoryAction.Attack, Game.TurnCounter)
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
            AddEntry(new HistoryEntry(Game, movingPlayer, HistoryAction.Move, Game.TurnCounter)
            {
                OriginIdentifier = originCountryIdentifier,
                DestinationIdentifier = destinationCountryIdentifier,
                Units = units
            });
        }

        public void RecordStart()
        {
            AddEntry(new HistoryEntry(Game, null, HistoryAction.StartGame, Game.TurnCounter));
        }

        public void RecordEnd()
        {
            AddEntry(new HistoryEntry(Game, null, HistoryAction.EndGame, Game.TurnCounter));
        }

        public void RecordEndTurn()
        {
            AddEntry(new HistoryEntry(Game, Game.CurrentPlayer, HistoryAction.EndTurn, Game.TurnCounter));
        }

        public void RecordTimeout()
        {
            AddEntry(new HistoryEntry(Game, Game.CurrentPlayer, HistoryAction.PlayerTimeout, Game.TurnCounter));
        }

        public void RecordCardExchange(Player exchangingPlayer, int unitsReceived)
        {
            AddEntry(new HistoryEntry(Game, exchangingPlayer, HistoryAction.ExchangeCards, Game.TurnCounter)
            {
                Units = unitsReceived
            });
        }

        public void RecordOwnershipChange(Player oldOwner, Player newOwner, string countryIdentifier)
        {
            AddEntry(new HistoryEntry(Game, oldOwner, HistoryAction.OwnerChange, Game.TurnCounter)
            {
                OriginIdentifier = countryIdentifier, OtherPlayer = newOwner
            });
        }

        public void RecordCapitalLost(Player player, string countryIdentifier)
        {
            AddEntry(new HistoryEntry(Game, player, HistoryAction.CapitalLost, Game.TurnCounter)
            {
                OriginIdentifier = countryIdentifier
            });
        }

        public void RecordPlayerDefeated(Player player)
        {
            AddEntry(new HistoryEntry(Game, player, HistoryAction.PlayerLost, Game.TurnCounter));
        }

        public void RecordPlayerSurrendered(Player player)
        {
            AddEntry(new HistoryEntry(Game, player, HistoryAction.PlayerSurrender, Game.TurnCounter));
        }

        public void RecordPlayerWon(Player player)
        {
            AddEntry(new HistoryEntry(Game, player, HistoryAction.PlayerWon, Game.TurnCounter));
        }

        public Map GetMapForTurn(long turnNo)
        {
            if (turnNo < 0 || turnNo > Game.TurnCounter)
            {
                throw new DomainException(ErrorCode.TurnDoesNotExist, "Invalid turn requested");
            }

            var currentMap = Game.Map.Clone();

            var actions = Game.HistoryEntries.Where(x => x.TurnNo > turnNo).OrderByDescending(x => x.DateTime)
                .OrderByDescending(x => x.Id);

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

                    case HistoryAction.OwnerChange:
                        {
                            var country = map.GetCountry(action.OriginIdentifier);
                            map.UpdateOwnership(action.Actor, country);

                            break;
                        }

                    case HistoryAction.CapitalLost:
                        {
                            var country = map.GetCountry(action.OriginIdentifier);
                            country.Flags |= CountryFlags.Capital;
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Applies general actions to the game
        /// </summary>
        /// <remarks>
        /// Does not apply any map changes, use <see cref="GameHistory.GetMapForTurn" /> instead
        /// </remarks>
        public HistoryGameTurn GetTurn(long turnNo)
        {
            if (turnNo < 0 || turnNo > Game.TurnCounter)
            {
                throw new DomainException(ErrorCode.TurnDoesNotExist, "Invalid turn requested");
            }

            var actions = Game.HistoryEntries.Where(x => x.TurnNo > turnNo).OrderByDescending(x => x.DateTime)
                .OrderByDescending(x => x.Id);

            foreach (var action in actions)
            {
                switch (action.Action)
                {
                    case HistoryAction.StartGame:
                        break;

                    case HistoryAction.EndGame:
                        break;

                    // Apply any action that affects the map
                    case HistoryAction.PlaceUnits:
                    case HistoryAction.Attack:
                    case HistoryAction.Move:
                    case HistoryAction.OwnerChange:
                    case HistoryAction.CapitalLost:
                        {
                            ApplyActionsToMap(Game.Map, new[] { action });
                            break;
                        }

                    case HistoryAction.ExchangeCards:
                        {
                            break;
                        }

                    case HistoryAction.PlayerLost:
                        {
                            var player = Game.GetPlayerById(action.Actor.Id);

                            player.State = PlayerState.Active;
                            player.Outcome = PlayerOutcome.None;

                            break;
                        }

                    case HistoryAction.PlayerWon:
                        {
                            var player = Game.GetPlayerById(action.Actor.Id);

                            player.State = PlayerState.Active;
                            player.Outcome = PlayerOutcome.None;

                            break;
                        }

                    case HistoryAction.PlayerTimeout:
                        {
                            var player = Game.GetPlayerById(action.Actor.Id);

                            --player.Timeouts;

                            break;
                        }

                    case HistoryAction.EndTurn:
                        {
                            break;
                        }
                }
            }

            return new HistoryGameTurn
            {
                TurnNo = turnNo,
                Game = Game,
                Actions = Game.HistoryEntries.Where(x => x.TurnNo == turnNo).OrderBy(x => x.DateTime)
            };
        }


        private void AddEntry(HistoryEntry historyEntry)
        {
            Game.HistoryEntries.Add(historyEntry);
        }
    }
}

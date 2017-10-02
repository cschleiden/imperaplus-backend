namespace ImperaPlus.Domain
{
    public enum ErrorCode
    {
        /// <summary>
        /// Default, no error
        /// </summary>
        None = 0,

        /// <summary>
        /// A generic error has occured
        /// </summary>
        GenericError,

        /// <summary>
        /// Cannot start the game
        /// </summary>
        CannotStartGame,

        /// <summary>
        /// This name is already in use by another game
        /// </summary>
        NameAlreadyTaken,

        /// <summary>
        /// Team has already the maximum number of players
        /// </summary>
        TeamAlreadyFull,

        /// <summary>
        /// Player has already joined this game
        /// </summary>
        PlayerAlreadyJoined,

        /// <summary>
        /// There are enough teams already
        /// </summary>
        TooManyTeams,

        /// <summary>
        /// Country cannot be found
        /// </summary>
        CountryNotFound,

        /// <summary>
        /// Actions cannot be performed with zero or negative units
        /// </summary>
        ZeroNegativeUnits,

        /// <summary>
        /// Attacking can only be done to countries not belonging to the current player
        /// </summary>
        AttackOwnCountries,

        /// <summary>
        /// Moving can only be done between own countries
        /// </summary>
        MoveOwnCountries,

        /// <summary>
        /// There are not enough units in the origin country for this action
        /// </summary>
        NotEnoughUnits,

        /// <summary>
        /// Game is no in the correct state to allow placing units
        /// </summary>
        PlacingNotAllowed,

        /// <summary>
        /// Placing units to countries belonging to another team is not allowed
        /// </summary>
        PlacingToForeignCountry,

        /// <summary>
        /// Placing more units than player has available is not allowed
        /// </summary>
        PlacingMoreUnitsThanAvailable,

        /// <summary>
        /// Placing less units than available is not allowed
        /// </summary>
        PlacingLessUnitsThanAvailable,

        /// <summary>
        /// Current user does not have enough slots available to create a new game with these settings
        /// </summary>
        NotEnoughSlots,

        /// <summary>
        /// Game is not in the correct state to allow attacking
        /// </summary>
        AttackingNotPossible,

        /// <summary>
        /// Origin country is not owned by the current player or team
        /// </summary>
        OriginCountryNotOwnedByTeam,

        /// <summary>
        /// Countries for action are not connected
        /// </summary>
        CountriesNotConnected,

        /// <summary>
        /// Game is not in the correct state to allow moving
        /// </summary>
        MovingNotPossible,

        /// <summary>
        /// Game is not active
        /// </summary>
        GameNotActive,

        /// <summary>
        /// News content cannot be found
        /// </summary>
        NewsContentNotFound,

        /// <summary>
        /// News content should be added with a language that does already exist
        /// </summary>
        DuplicateNewsContent,

        /// <summary>
        /// Requested history turn does not exist
        /// </summary>
        TurnDoesNotExist,

        /// <summary>
        /// Exchaning cards is currently now allowed
        /// </summary>
        ExchangingCardsNotAllowed,

        /// <summary>
        /// Player is not in this game
        /// </summary>
        PlayerNotInGame,

        /// <summary>
        /// Cannot hide game for player
        /// </summary>
        CannotHideGame,

        /// <summary>
        /// Cannot leave game
        /// </summary>
        /// 
        CannotLeaveGame,

        /// <summary>
        /// User cannot surrender in game
        /// </summary>
        CannotSurrender,

        /// <summary>
        /// User is already queued up for ladder
        /// </summary>
        LadderUserAlreadyQueue,

        /// <summary>
        /// No map templates passed to ladder
        /// </summary>
        LadderNoMapTemplates,

        /// <summary>
        /// Cannot activate the ladder due to invalid options
        /// </summary>
        LadderCannotActivate,

        /// <summary>
        /// Invalid options passed to tournament
        /// </summary>
        TournamentInvalidOption,

        /// <summary>
        /// Tournament cannot be started
        /// </summary>
        TournamentStart,

        /// <summary>
        /// No Map templates defined for tournament
        /// </summary>
        TournamentNoMapTemplates,

        /// <summary>
        /// Tournament has too many reams
        /// </summary>
        TournamentTooManyTeams,

        /// <summary>
        /// Too many players already in team
        /// </summary>
        TournamentTooManyPlayersInTeam,

        /// <summary>
        /// Password for team is incorrect
        /// </summary>
        TournamentIncorrectPassword,

        /// <summary>
        /// If tournament is a team tournament, users need to join a team explicitly
        /// </summary>
        TournamentRequiresExplicitTeam,

        /// <summary>
        /// Team with name already exists
        /// </summary>
        TournamentDuplicateTeamName,

        TournamentUserNoParticipant,

        TournamentCannotJoinLeave,

        TournamentTeamDeleteNoPermission,

        /// <summary>
        /// User wants to leave Ladder queue but is not in it
        /// </summary>
        LadderUserNotInQueue,

        /// <summary>
        /// Team creator has to delete team
        /// </summary>
        TournamentTeamCreatorHasToDelete,
        CannotFindGame,
        CannotDeleteGame,
        GameAlreadyScored
    }
}
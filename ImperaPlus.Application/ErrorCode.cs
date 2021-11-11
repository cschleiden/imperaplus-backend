namespace ImperaPlus.Application
{
    public enum ErrorCode
    {
        None = 0,

        GenericApplicationError,

        CannotFindGame,

        CannotFindMapTemplate,

        UserIsNotAllowedToPerformAction,

        /// <summary>
        /// Account is locked (after too many failed attemps)
        /// </summary>
        AccountIsLocked,

        /// <summary>
        /// Account is not confirmed
        /// </summary>
        AccountNotConfirmed,

        /// <summary>
        /// Username is not known or password is not correct
        /// </summary>
        UsernameOrPasswordNotCorrect,

        EmailAlreadyInUse,

        UserWithExternalLoginExists,

        UserIdNotFound,

        UsernameInvalid,

        EmailInvalid,

        PasswordInvalid,

        UserAlreadyInRole,

        UserNotInRole,

        UserDoesNotExist,

        InvalidToken,

        UsernameAlreadyInUse,

        ExternalLoginFailure,

        PasswordsDoNotMatch,

        CannotFindLadder,

        CannotAddMapTemplate,

        CannotFindMessage,

        TournamentNotFound,

        TournamentTeamNotFound,

        TournamentTeamDeleteNoPermission,

        CannotDeleteGame,

        CannotDeleteTournamentTeam,

        TournamentTeamInvalidPassword,

        CannotCreateTournament,

        MapTemplatesRequired,

        AccountIsDeleted
    }
}

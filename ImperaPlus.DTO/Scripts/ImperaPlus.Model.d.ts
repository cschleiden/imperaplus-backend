
 
 

 

declare module ImperaPlus.DTO {
	interface ErrorResponse {
		error: string;
		error_Description: string;
		parameter_Errors: System.Collections.Generic.KeyValuePair<string, string[]>[];
	}
}
declare module System.Collections.Generic {
	interface KeyValuePair<TKey, TValue> {
		key: TKey;
		value: TValue;
	}
}
declare module ImperaPlus.DTO.Users {
	interface UserReference {
		id: string;
		name: string;
	}
}
declare module ImperaPlus.DTO.Tournaments {
	interface Tournament extends ImperaPlus.DTO.Tournaments.TournamentSummary {
		teams: ImperaPlus.DTO.Tournaments.TournamentTeam[];
		groups: ImperaPlus.DTO.Tournaments.TournamentGroup[];
		pairings: ImperaPlus.DTO.Tournaments.TournamentPairing[];
		mapTemplates: string[];
		winner: ImperaPlus.DTO.Tournaments.TournamentTeam;
		phase: number;
	}
	interface TournamentSummary {
		id: string;
		name: string;
		state: ImperaPlus.DTO.Tournaments.TournamentState;
		options: ImperaPlus.DTO.Games.GameOptions;
		numberOfTeams: number;
		numberOfGroupGames: number;
		numberOfKnockoutGames: number;
		numberOfFinalGames: number;
		startOfRegistration: Date;
		startOfTournament: Date;
		endOfTournament: Date;
		completion: number;
	}
	interface TournamentTeam extends ImperaPlus.DTO.Tournaments.TournamentTeamSummary {
		participants: ImperaPlus.DTO.Users.UserReference[];
	}
	interface TournamentTeamSummary {
		id: string;
		name: string;
		groupOrder: number;
		state: ImperaPlus.DTO.Tournaments.TournamentTeamState;
	}
	interface TournamentGroup {
		id: string;
		teams: ImperaPlus.DTO.Tournaments.TournamentTeamSummary[];
	}
	interface TournamentPairing {
		teamA: ImperaPlus.DTO.Tournaments.TournamentTeamSummary;
		teamB: ImperaPlus.DTO.Tournaments.TournamentTeamSummary;
		teamAWon: number;
		teamBWon: number;
		numberOfGames: number;
		phase: number;
		order: number;
	}
}
declare module ImperaPlus.DTO.Games {
	interface GameOptions {
		numberOfPlayersPerTeam: number;
		numberOfTeams: number;
		minUnitsPerCountry: number;
		newUnitsPerTurn: number;
		attacksPerTurn: number;
		movesPerTurn: number;
		initialCountryUnits: number;
		mapDistribution: ImperaPlus.DTO.Games.MapDistribution;
		timeoutInSeconds: number;
		maximumTimeoutsPerPlayer: number;
		maximumNumberOfCards: number;
		victoryConditions: ImperaPlus.DTO.Games.VictoryConditionType[];
		visibilityModifier: ImperaPlus.DTO.Games.VisibilityModifierType[];
	}
	interface PlayerSummary {
		id: string;
		userId: string;
		name: string;
		state: ImperaPlus.DTO.Games.PlayerState;
		outcome: ImperaPlus.DTO.Games.PlayerOutcome;
		teamId: string;
		playOrder: number;
		timeouts: number;
	}
	interface Game {
		id: number;
		type: ImperaPlus.DTO.Games.GameType;
		name: string;
		mapTemplate: string;
		teams: ImperaPlus.DTO.Games.Team[];
		state: ImperaPlus.DTO.Games.GameState;
		playState: ImperaPlus.DTO.Games.PlayState;
		currentPlayer: ImperaPlus.DTO.Games.PlayerSummary;
		map: ImperaPlus.DTO.Games.Map.Map;
		options: ImperaPlus.DTO.Games.GameOptions;
		lastModifiedAt: Date;
		timeoutSecondsLeft: number;
		turnCounter: number;
		unitsToPlace: number;
		attacksInCurrentTurn: number;
		movesInCurrentTurn: number;
	}
	interface Team {
		id: string;
		playOrder: number;
		players: ImperaPlus.DTO.Games.Player[];
	}
	interface Player extends ImperaPlus.DTO.Games.PlayerSummary {
		cards: ImperaPlus.DTO.Games.BonusCard[];
		placedInitialUnits: boolean;
		numberOfUnits: number;
		numberOfCountries: number;
	}
	interface GameActionResult {
		id: number;
		teams: ImperaPlus.DTO.Games.Team[];
		state: ImperaPlus.DTO.Games.GameState;
		playState: ImperaPlus.DTO.Games.PlayState;
		countryUpdates: ImperaPlus.DTO.Games.Map.Country[];
		actionResult: ImperaPlus.DTO.Games.ActionResult;
		attacksInCurrentTurn: number;
		movesInCurrentTurn: number;
		cards: ImperaPlus.DTO.Games.BonusCard[];
		currentPlayer: ImperaPlus.DTO.Games.Player;
	}
	interface GameCreationOptions extends ImperaPlus.DTO.Games.GameOptions {
		name: string;
		addBot: boolean;
		mapTemplate: string;
	}
	interface GameSummary {
		id: number;
		type: ImperaPlus.DTO.Games.GameType;
		name: string;
		ladderId: string;
		ladderName: string;
		options: ImperaPlus.DTO.Games.GameOptions;
		createdByUserId: string;
		createdByName: string;
		startedAt: Date;
		lastActionAt: Date;
		timeoutSecondsLeft: number;
		mapTemplate: string;
		state: ImperaPlus.DTO.Games.GameState;
		currentPlayer: ImperaPlus.DTO.Games.PlayerSummary;
		teams: ImperaPlus.DTO.Games.TeamSummary[];
	}
	interface TeamSummary {
		id: string;
		playOrder: number;
		players: ImperaPlus.DTO.Games.PlayerSummary[];
	}
}
declare module ImperaPlus.DTO.Notifications {
	interface GameChatMessageNotification extends ImperaPlus.DTO.Notifications.Notification {
		gameId: number;
		message: ImperaPlus.DTO.Games.Chat.GameChatMessage;
	}
	interface Notification {
		type: ImperaPlus.DTO.Notifications.NotificationType;
	}
	interface NewMessageNotification extends ImperaPlus.DTO.Notifications.Notification {
		fromUserName: string;
		subject: string;
	}
	interface NotificationSummary {
		numberOfGames: number;
		numberOfMessages: number;
	}
	interface PlayersTurnNotification extends ImperaPlus.DTO.Notifications.Notification {
		gameId: number;
	}
	interface PlayerSurrenderedNotification extends ImperaPlus.DTO.Notifications.Notification {
		gameId: number;
		player: ImperaPlus.DTO.Games.PlayerSummary;
	}
	interface TurnEndedNotification extends ImperaPlus.DTO.Notifications.Notification {
		gameId: number;
		newPlayerId: string;
	}
}
declare module ImperaPlus.DTO.Games.Chat {
	interface GameChatMessage {
		id: number;
		gameId: number;
		user: ImperaPlus.DTO.Users.UserReference;
		teamId: string;
		dateTime: Date;
		text: string;
	}
}
declare module ImperaPlus.DTO.News {
	interface NewsContent {
		language: string;
		title: string;
		text: string;
	}
	interface NewsItem {
		dateTime: Date;
		postedBy: string;
		content: ImperaPlus.DTO.News.NewsContent[];
	}
}
declare module ImperaPlus.DTO.Messages {
	interface FolderInformation {
		folder: ImperaPlus.DTO.Messages.MessageFolder;
		count: number;
		unreadCount: number;
	}
	interface Message extends ImperaPlus.DTO.Messages.SendMessage {
		id: string;
		from: ImperaPlus.DTO.Users.UserReference;
		folder: ImperaPlus.DTO.Messages.MessageFolder;
		sentAt: Date;
		isRead: boolean;
	}
	interface SendMessage {
		to: ImperaPlus.DTO.Users.UserReference;
		subject: string;
		text: string;
	}
}
declare module ImperaPlus.DTO.Ladder {
	interface Ladder extends ImperaPlus.DTO.Ladder.LadderSummary {
		standings: ImperaPlus.DTO.Ladder.LadderStanding[];
		isActive: boolean;
	}
	interface LadderSummary {
		id: string;
		name: string;
		options: ImperaPlus.DTO.Games.GameOptions;
		standing: ImperaPlus.DTO.Ladder.LadderStanding;
		isQueued: boolean;
		queueCount: number;
		mapTemplates: string[];
	}
	interface LadderStanding {
		userId: string;
		userName: string;
		position: number;
		gamesPlayed: number;
		gamesWon: number;
		gamesLost: number;
		rating: number;
		lastGame: Date;
	}
}
declare module ImperaPlus.DTO.Ladder.Admin {
	interface CreationOptions {
		name: string;
		numberOfTeams: number;
		numberOfPlayers: number;
	}
	interface MapTemplateUpdate {
		mapTemplateNames: string[];
	}
}
declare module ImperaPlus.DTO.Games.Map {
	interface Map {
		countries: ImperaPlus.DTO.Games.Map.Country[];
	}
	interface Country {
		identifier: string;
		playerId: string;
		teamId: string;
		units: number;
	}
	interface Connection {
		origin: string;
		destination: string;
	}
	interface Continent {
		id: number;
		name: string;
		bonus: number;
		countries: string[];
	}
	interface CountryTemplate {
		identifier: string;
		name: string;
		x: number;
		y: number;
	}
	interface MapTemplate {
		name: string;
		image: string;
		countries: ImperaPlus.DTO.Games.Map.CountryTemplate[];
		connections: ImperaPlus.DTO.Games.Map.Connection[];
		continents: ImperaPlus.DTO.Games.Map.Continent[];
	}
	interface MapTemplateSummary {
		id: number;
		name: string;
		image: string;
	}
}
declare module ImperaPlus.DTO.Games.Play {
	interface AttackOptions {
		originCountryIdentifier: string;
		destinationCountryIdentifier: string;
		numberOfUnits: number;
	}
	interface MoveOptions {
		originCountryIdentifier: string;
		destinationCountryIdentifier: string;
		numberOfUnits: number;
	}
	interface PlaceUnitsOptions {
		countryIdentifier: string;
		numberOfUnits: number;
	}
}
declare module ImperaPlus.DTO.Games.History {
	interface HistoryEntry {
		id: number;
		turnNo: number;
		dateTime: Date;
		actorId: string;
		otherPlayerId: string;
		action: ImperaPlus.DTO.Games.History.HistoryAction;
		originIdentifier: string;
		destinationIdentifier: string;
		units: number;
		unitsLost: number;
		unitsLostOther: number;
		result: boolean;
	}
	interface HistoryTurn {
		gameId: number;
		turnId: number;
		actions: ImperaPlus.DTO.Games.History.HistoryEntry[];
		game: ImperaPlus.DTO.Games.Game;
	}
}
declare module ImperaPlus.DTO.Chat {
	interface ChannelInformation {
		identifier: string;
		title: string;
		messages: ImperaPlus.DTO.Chat.Message[];
		users: ImperaPlus.DTO.Chat.User[];
		persistant: boolean;
	}
	interface Message {
		dateTime: Date;
		userName: string;
		text: string;
		channelIdentifier: string;
	}
	interface User {
		type: ImperaPlus.DTO.Chat.UserType;
		name: string;
	}
	interface ChatInformation {
		channels: ImperaPlus.DTO.Chat.ChannelInformation[];
	}
	interface UserChangeEvent {
		userName: string;
		channelIdentifier: string;
	}
}
declare module ImperaPlus.DTO.Account {
	interface AddExternalLoginBindingModel {
		externalAccessToken: string;
	}
	interface ChangePasswordBindingModel {
		oldPassword: string;
		newPassword: string;
		confirmPassword: string;
	}
	interface ResetPasswordViewModel {
		userId: string;
		password: string;
		confirmPassword: string;
		code: string;
	}
	interface RegisterBindingModel {
		userName: string;
		password: string;
		confirmPassword: string;
		email: string;
		language: string;
		callbackUrl: string;
	}
	interface ForgotPasswordViewModel {
		callbackUrl: string;
		userName: string;
		email: string;
		language: string;
	}
	interface ResendConfirmationModel {
		callbackUrl: string;
		userName: string;
		password: string;
		language: string;
	}
	interface ConfirmationModel {
		userId: string;
		code: string;
	}
	interface RegisterExternalBindingModel {
		userName: string;
	}
	interface RemoveLoginBindingModel {
		loginProvider: string;
		providerKey: string;
	}
	interface SetPasswordBindingModel {
		newPassword: string;
		confirmPassword: string;
	}
	interface LanguageModel {
		language: string;
	}
	interface ExternalLoginViewModel {
		name: string;
		url: string;
		state: string;
	}
	interface ManageInfoViewModel {
		localLoginProvider: string;
		userName: string;
		logins: ImperaPlus.DTO.Account.UserLoginInfoViewModel[];
		externalLoginProviders: ImperaPlus.DTO.Account.ExternalLoginViewModel[];
	}
	interface UserLoginInfoViewModel {
		loginProvider: string;
		providerKey: string;
	}
	interface UserInfoViewModel {
		userId: string;
		userName: string;
		hasRegistered: boolean;
		loginProvider: string;
	}
	interface LoginResponseModel {
		access_token: string;
		userId: string;
		userName: string;
		language: string;
		roles: string;
	}
	interface UserInfo {
		userId: string;
		userName: string;
		hasRegistered: boolean;
		loginProvider: string;
		language: string;
		roles: string[];
	}
}



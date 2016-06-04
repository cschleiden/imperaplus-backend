module ImperaPlus.DTO.Tournaments {
	export enum TournamentState {
		Open = 0,
		Groups = 1,
		Knockout = 2,
		Closed = 3
	}
	export enum TournamentTeamState {
		Open = 0,
		Active = 1,
		InActive = 2
	}
}
module ImperaPlus.DTO.Games {
	export enum VictoryConditionType {
		Survival = 0,
		ControlContinent = 1
	}
	export enum VisibilityModifierType {
		None = 0,
		Fog = 1
	}
	export enum MapDistribution {
		Default = 0,
		Malibu = 1,
		TeamCluster = 2
	}
	export enum PlayerState {
		None = 0,
		Active = 1,
		InActive = 2
	}
	export enum PlayerOutcome {
		None = 0,
		Won = 1,
		Defeated = 2,
		Surrendered = 3,
		Timeout = 4
	}
	export enum BonusCard {
		A = 0,
		B = 1,
		C = 2
	}
	export enum GameType {
		Fun = 0,
		Ranking = 1,
		Tournament = 2
	}
	export enum GameState {
		None = 0,
		Open = 1,
		Active = 2,
		Ended = 3
	}
	export enum PlayState {
		None = 0,
		PlaceUnits = 1,
		Attack = 2,
		Move = 3,
		Done = 4
	}
	export enum ActionResult {
		None = 0,
		Successful = 1,
		NotSuccessful = 2
	}
}
module ImperaPlus.DTO.Notifications {
	export enum NotificationType {
		PlayerTurn = 0,
		EndTurn = 1,
		PlayerSurrender = 2,
		GameChatMessage = 3,
		NewMessage = 4
	}
}
module ImperaPlus.DTO.Messages {
	export enum MessageFolder {
		None = 0,
		Inbox = 1,
		Sent = 2
	}
}
module ImperaPlus.DTO.Games.History {
	export enum HistoryAction {
		None = 0,
		StartGame = 1,
		EndGame = 2,
		PlaceUnits = 3,
		Attack = 4,
		Move = 5,
		ExchangeCards = 6,
		PlayerLost = 7,
		PlayerWon = 8,
		PlayerTimeout = 9,
		OwnerChange = 10,
		EndTurn = 11
	}
}
module ImperaPlus.DTO.Chat {
	export enum UserType {
		None = 0,
		Admin = 1,
		Developer = 2,
		Owner = 3
	}
}


import {useGameSessionWS} from "./webSocket";

export enum WSActions {
    newMessage = 'NewMessage',
    joinGameSession = 'JoinGameSession',
    leaveGameSession = 'LeaveGameSession',
    rollDice = 'RollDice',
    rollSaveDice = 'RollSaveDice',
    lockCharacter = 'LockCharacter',
    unlockCharacter = 'UnlockCharacter',
    startGame = 'StartGame',
    kick = 'Kick',
    requestSaveThrow = 'RequestSaveThrow',
    changeCharacter = 'ChangeCharacter',
    moveCharacter = 'MoveCharacter',
}

export enum WSEvents {
    //messages
    historyReceived = 'historyReceived',
    messageReceived = 'messageReceived',


    //global info
    playerInfoReceived = 'playerInfoReceived',
    gameSessionInfoReceived = "gameSessionInfoReceived",


    //characters
    characterLocked = 'characterLocked',
    characterUnlocked = 'characterUnlocked',


    //player
    playerSaveThrow = "playerSaveThrow",
    playerLeft = 'playerLeft',
    playerSaveThrowPassed = 'playerSaveThrowPassed',
}

export type WSApi = ReturnType<typeof useGameSessionWS>;

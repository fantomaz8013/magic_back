import * as signalR from "@microsoft/signalr";
import {baseProxy} from "../env";
import React, {useEffect, useMemo, useRef, useState} from "react";
import {AppDispatch, RootState,} from "../redux";
import {useDispatch, useSelector} from "react-redux";
import {GameSessionInfo} from "../models/websocket/gameStartedInfo";
import {BaseGameSessionMessage, CubeTypeEnum} from "../models/websocket/ChatMessage";
import {PlayerInfo} from "../components/gameSession";
import {
    addMessage, characterLocked, characterUnlocked, playerLeft,
    setGameSessionInfo,
    setMessages,
    setPlayerInfos
} from "../redux/toolkit/slices/gameSessionSlice";

function createSignalRConnection(
    url: string,
    tokenRef: React.MutableRefObject<string | null>,
    loggingLevel = signalR.LogLevel.None): signalR.HubConnection {
    return new signalR.HubConnectionBuilder()
        .withUrl(
            baseProxy + url,
            {
                accessTokenFactory: async () => {
                    return tokenRef.current || '';
                },
                transport: signalR.HttpTransportType.WebSockets | signalR.HttpTransportType.LongPolling
            }
        )
        .withAutomaticReconnect()
        .configureLogging(loggingLevel)
        .build();
}

const wsPath = 'ws';

export enum WSActions {
    newMessage = 'NewMessage',
    joinGameSession = 'JoinGameSession',
    leaveGameSession = 'LeaveGameSession',
    rollDice = 'RollDice',
    lockCharacter = 'LockCharacter',
    unlockCharacter = 'UnlockCharacter',
    startGame = 'StartGame',
}

export enum WSEvents {
    historyReceived = 'historyReceived',
    messageReceived = 'messageReceived',
    characterLocked = 'characterLocked',
    characterUnlocked = 'characterUnlocked',
    playerLeft = 'playerLeft',
    playerInfoReceived = 'playerInfoReceived',
    gameSessionInfoReceived = "gameSessionInfoReceived",
    gameStarted = "gameStarted",
}


function useSignalR(wsPath: string) {
    const token = useSelector((state: RootState) => state.auth.token)
    const tokenRef = useRef(token);
    const ws = useMemo(() => createSignalRConnection(wsPath, tokenRef), [wsPath]);
    const [state, setState] = useState(ws.state);

    useEffect(() => {
        tokenRef.current = token;
    }, [token])

    useEffect(() => {
        ws.start()
            .catch(() => {
            })
            .then(() => {
                setState(ws.state);
            });

        return () => {
            if (isConnected()) {
                ws.stop()
                    .then(() => {
                        setState(ws.state);
                    });
            }
        }
    }, [wsPath])

    return {ws, state};

    function isConnected() {
        return state === 'Connected';
    }
}

type apiType = ReturnType<typeof useGameSessionWS>;
export let socket: apiType | null = null;

export function useGameSessionWS(logsEnabled?: boolean) {
    logsEnabled ??= true;
    const {ws, state} = useSignalR(wsPath);
    const dispatch = useDispatch<AppDispatch>();

    useEffect(() => {
        ws.on(WSEvents.gameSessionInfoReceived, onGameSessionInfoReceived);
        ws.on(WSEvents.gameStarted, onGameStarted);
        ws.on(WSEvents.messageReceived, onMessageReceived);
        ws.on(WSEvents.historyReceived, onHistoryReceived);
        ws.on(WSEvents.playerInfoReceived, onPlayerInfoReceived);
        ws.on(WSEvents.characterLocked, onCharacterLocked);
        ws.on(WSEvents.characterUnlocked, onCharacterUnlocked);
        ws.on(WSEvents.playerLeft, onPlayerLeft);

        return () => {
            ws.off(WSEvents.gameSessionInfoReceived);
            ws.off(WSEvents.gameStarted);
            ws.off(WSEvents.messageReceived);
            ws.off(WSEvents.historyReceived);
            ws.off(WSEvents.playerInfoReceived);
            ws.off(WSEvents.characterLocked);
            ws.off(WSEvents.characterUnlocked);
            ws.off(WSEvents.playerLeft);
        }
    }, [ws])

    const toReturn = {
        joinGameSession,
        leaveGameSession,
        lockCharacter,
        newMessage,
        rollDice,
        startGame,
        state,
        unlockCharacter
    };

    socket = toReturn;


    return toReturn;

    async function startGame() {
        logsEnabled && console.log(WSActions.startGame);
        await ws.invoke(WSActions.startGame);
    }

    async function joinGameSession(gameSessionId: string) {
        logsEnabled && console.log(WSActions.joinGameSession, gameSessionId);
        await ws.invoke(WSActions.joinGameSession, gameSessionId);
    }

    async function leaveGameSession() {
        logsEnabled && console.log(WSActions.leaveGameSession);
        await ws.invoke(WSActions.leaveGameSession);
    }

    async function newMessage(message: string) {
        logsEnabled && console.log(WSActions.newMessage, message);
        await ws.invoke(WSActions.newMessage, message);
    }

    async function lockCharacter(characterId: string) {
        logsEnabled && console.log(WSActions.lockCharacter, characterId);
        await ws.invoke(WSActions.lockCharacter, characterId)
    }

    async function unlockCharacter() {
        logsEnabled && console.log(WSActions.unlockCharacter);
        await ws.invoke(WSActions.unlockCharacter)
    }

    async function rollDice(cubeType: CubeTypeEnum) {
        logsEnabled && console.log(WSActions.rollDice, CubeTypeEnum[cubeType]);
        await ws.invoke(WSActions.rollDice, cubeType);
    }

    function onPlayerInfoReceived(playerInfos: PlayerInfo[]) {
        logsEnabled && console.log(WSEvents.playerInfoReceived, playerInfos);
        dispatch(setPlayerInfos(playerInfos.reduce((pv, cv) => {
            pv[cv.id] = cv;
            return pv;
        }, {} as Record<string, PlayerInfo>)));
    }

    function onCharacterLocked(userId: string, lockedCharacterTemplateId: string) {
        const data = {userId, lockedCharacterTemplateId};
        logsEnabled && console.log(WSEvents.characterLocked, data);
        dispatch(characterLocked(data));
    }

    function onCharacterUnlocked(userId: string) {
        logsEnabled && console.log(WSEvents.characterUnlocked, userId);
        dispatch(characterUnlocked(userId));
    }

    function onPlayerLeft(userId: string) {
        logsEnabled && console.log(WSEvents.playerLeft, userId);
        dispatch(playerLeft(userId));
    }

    function onGameSessionInfoReceived(data: GameSessionInfo) {
        logsEnabled && console.log(WSEvents.gameSessionInfoReceived, data)
        dispatch(setGameSessionInfo(data));
    }

    function onGameStarted(data: GameSessionInfo) {
        logsEnabled && console.log(WSEvents.gameStarted, data)
        dispatch(setGameSessionInfo(data));
    }

    function onMessageReceived(message: BaseGameSessionMessage) {
        logsEnabled && console.log(WSEvents.messageReceived, message)
        dispatch(addMessage(message));
    }

    function onHistoryReceived(newMessages: BaseGameSessionMessage[]) {
        logsEnabled && console.log(WSEvents.historyReceived, newMessages)
        dispatch(setMessages(newMessages));
    }
}



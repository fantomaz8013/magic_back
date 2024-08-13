import * as signalR from "@microsoft/signalr";
import {baseProxy} from "../env";
import React, {useEffect, useMemo, useRef, useState} from "react";
import {AppDispatch, RootState,} from "../redux/redux";
import {useDispatch, useSelector} from "react-redux";
import {GameSessionCharacter, GameSessionInfo} from "../models/websocket/gameStartedInfo";
import {BaseGameSessionMessage, CubeTypeEnum} from "../models/websocket/chatMessage";
import {PlayerInfo} from "../components/gameSession/GameSession";
import {
    addMessage,
    characterLocked,
    characterUnlocked,
    playerLeft,
    RequestedSaveThrow,
    setRequestSaveThrow,
    setGameSessionInfo,
    setMessages,
    setPlayerInfos,
    RequestedSaveThrowPassed,
    setRequestSaveThrowPassed
} from "../redux/slices/gameSessionSlice";
import {CharacterCharacteristicIds} from "../models/response/characterTemplateResponse";
import {WSApi, WSEvents, WSActions} from "./webSocket.types";
import {UnknownAction} from "redux";
import {LocationRequest} from "../redux/slices/moveSlice";

const wsPath = 'ws';
export let socket: WSApi | null = null;

export function useGameSessionWS(logsEnabled?: boolean) {
    logsEnabled ??= true;
    const {ws, state} = useSignalR(wsPath);
    const dispatch = useDispatch<AppDispatch>();

    useEffect(() => {
        ws.on(WSEvents.gameSessionInfoReceived, onGameSessionInfoReceived);
        ws.on(WSEvents.messageReceived, onMessageReceived);
        ws.on(WSEvents.historyReceived, onHistoryReceived);
        ws.on(WSEvents.playerInfoReceived, onPlayerInfoReceived);
        ws.on(WSEvents.characterLocked, onCharacterLocked);
        ws.on(WSEvents.characterUnlocked, onCharacterUnlocked);
        ws.on(WSEvents.playerLeft, onPlayerLeft);
        ws.on(WSEvents.playerSaveThrow, onPlayerSaveThrow);
        ws.on(WSEvents.playerSaveThrowPassed, onPlayerSaveThrowPassed);

        return () => {
            ws.off(WSEvents.gameSessionInfoReceived);
            ws.off(WSEvents.messageReceived);
            ws.off(WSEvents.historyReceived);
            ws.off(WSEvents.playerInfoReceived);
            ws.off(WSEvents.characterLocked);
            ws.off(WSEvents.characterUnlocked);
            ws.off(WSEvents.playerLeft);
            ws.off(WSEvents.playerSaveThrow);
            ws.off(WSEvents.playerSaveThrowPassed);
        }
    }, [ws])

    const toReturn = {
        joinGameSession,
        leaveGameSession,
        lockCharacter,
        newMessage,
        rollDice,
        startGame,
        kick,
        requestSaveThrow,
        unlockCharacter,
        rollSaveDice,
        changeCharacter,
        moveCharacter,
        state,
    };

    socket = toReturn;


    return toReturn;

    async function _invoke(action: WSActions, ...args: any) {
        logsEnabled && console.log(action, ...args);
        if (ws.state === "Connected")
            await ws.invoke(action, ...args);
        else
            logsEnabled && console.warn('Called invoke on not connected')
    }

    async function startGame() {
        await _invoke(WSActions.startGame);
    }

    async function joinGameSession(gameSessionId: string) {
        await _invoke(WSActions.joinGameSession, gameSessionId);
    }

    async function leaveGameSession() {
        await _invoke(WSActions.leaveGameSession);
    }

    async function newMessage(message: string) {
        await _invoke(WSActions.newMessage, message);
    }

    async function lockCharacter(characterId: string) {
        await _invoke(WSActions.lockCharacter, characterId)
    }

    async function unlockCharacter() {
        await _invoke(WSActions.unlockCharacter)
    }

    async function rollDice(cubeType: CubeTypeEnum) {
        await _invoke(WSActions.rollDice, cubeType);
    }

    async function rollSaveDice() {
        await _invoke(WSActions.rollSaveDice);
    }

    async function kick(userId: string) {
        await _invoke(WSActions.kick, userId);
    }

    async function requestSaveThrow(userId: string, characteristicId: CharacterCharacteristicIds, value: number) {
        await _invoke(WSActions.requestSaveThrow, userId, characteristicId, value);
    }

    async function changeCharacter(characterId: string, changedFields: Partial<GameSessionCharacter>) {
        await _invoke(WSActions.changeCharacter, characterId, changedFields);
    }

    async function moveCharacter(characterId: string, path: LocationRequest[]) {
        await _invoke(WSActions.moveCharacter, characterId, path);
    }

    function _onEvent(event: WSEvents, func: () => UnknownAction, ...args: any) {
        logsEnabled && console.log(event, ...args);
        dispatch(func());
    }

    function onPlayerInfoReceived(playerInfos: PlayerInfo[]) {
        _onEvent(
            WSEvents.playerInfoReceived,
            () => setPlayerInfos(playerInfos.reduce((pv, cv) => {
                pv[cv.id] = cv;
                return pv;
            }, {} as Record<string, PlayerInfo>)),
            playerInfos
        );
    }

    function onCharacterLocked(userId: string, lockedCharacterTemplateId: string) {
        const data = {userId, lockedCharacterTemplateId};

        _onEvent(
            WSEvents.characterLocked,
            () => characterLocked(data),
            data
        );
    }

    function onCharacterUnlocked(userId: string) {
        _onEvent(
            WSEvents.characterUnlocked,
            () => characterUnlocked(userId),
            userId
        );
    }

    function onPlayerLeft(userId: string) {
        _onEvent(
            WSEvents.playerLeft,
            () => playerLeft(userId),
            userId
        );
    }

    function onPlayerSaveThrow(data: RequestedSaveThrow) {
        _onEvent(
            WSEvents.playerSaveThrow,
            () => setRequestSaveThrow(data),
            data
        );
    }

    function onPlayerSaveThrowPassed(data: RequestedSaveThrowPassed) {
        _onEvent(
            WSEvents.playerSaveThrowPassed,
            () => setRequestSaveThrowPassed(data),
            data
        );
    }

    function onGameSessionInfoReceived(data: GameSessionInfo) {
        _onEvent(
            WSEvents.gameSessionInfoReceived,
            () => setGameSessionInfo(data),
            data
        );
    }

    function onMessageReceived(message: BaseGameSessionMessage) {
        _onEvent(
            WSEvents.messageReceived,
            () => addMessage(message),
            message
        );
    }

    function onHistoryReceived(newMessages: BaseGameSessionMessage[]) {
        _onEvent(
            WSEvents.historyReceived,
            () => setMessages(newMessages),
            newMessages
        );
    }
}

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
            if (ws.state !== 'Disconnected' && ws.state !== 'Disconnecting') {
                ws.stop()
                    .then(() => {
                        setState(ws.state);
                    });
            }
        }
    }, [])

    return {ws, state};
}





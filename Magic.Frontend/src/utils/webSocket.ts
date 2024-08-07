import * as signalR from "@microsoft/signalr";
import {baseProxy} from "../env";
import React, {useEffect, useMemo, useRef} from "react";
import {RootState,} from "../redux";
import {useSelector} from "react-redux";

export interface WithWS {
    ws: signalR.HubConnection;
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

export interface SignalRProps {
    beforeStart: (ws: signalR.HubConnection) => void;
    afterStart: (ws: signalR.HubConnection) => void;
    beforeStop: (ws: signalR.HubConnection) => Promise<void>;
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


export function useSignalR(props: SignalRProps) {
    const token = useSelector((state: RootState) => state.auth.token)
    const tokenRef = useRef(token);

    const ws = useMemo(() => {
        const connection = createSignalRConnection(wsPath, tokenRef)
        props.beforeStart(connection);
        return connection;
    }, []);

    useEffect(() => {
        tokenRef.current = token;
    }, [token])

    useEffect(() => {
        ws.start()
            .catch(() => {
            })
            .then(() => {
                if (isConnected())
                    props.afterStart(ws)
            });

        return () => {
            if (isConnected())
                props.beforeStop(ws)
                    .then(async () => {
                        await ws.stop();
                    })
        }
    }, [])

    return ws;

    function isConnected() {
        return ws.state === 'Connected';
    }
}


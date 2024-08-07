import React, {useState} from "react";
import {SignalRProps, useSignalR, WSActions, WSEvents} from "../../utils/webSocket";
import Box from "@mui/material/Box";
import Dice from "../dice/Dice";
import Chat from "./chat";
import CharactersList from "./charactersList/CharactersList";
import {useParams} from "react-router-dom";
import {GameSessionInfo} from "../../models/websocket/gameStartedInfo";
import {LinearProgress} from "@mui/material";
import {GameSessionStatusTypeEnum} from "../../models/websocket/gameSessionStatus";
import CharacterCard from "./charactersList/CharacterCard";

export interface PlayerInfo {
    id: string;
    login: string;
    isMaster: boolean | null;
    lockedCharacterId: string | null;
}

export default function GameSession() {
    const ws = useSignalR(createSignalRConfig());
    const {gameSessionId} = useParams();
    const [gameSessionInfo, setGameSessionInfo] = useState<GameSessionInfo | null>(null);

    return renderGameSessionPage();

    function renderGameSessionPage() {
        if (!gameSessionInfo)
            return (
                <LinearProgress color="inherit"/>
            )

        switch (gameSessionInfo.gameSessionStatus) {
            case GameSessionStatusTypeEnum.WaitingForStart:
                return (
                    <Box sx={{
                        marginTop: 8,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                    }}>
                        <CharactersList ws={ws}/>
                        <Chat ws={ws}/>
                    </Box>
                );
            case GameSessionStatusTypeEnum.InGame:
                return (
                    <Box sx={{
                        marginTop: 8,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                    }}>
                        НИХУЯ СЕ ИГРА НАЧАЛАСЬ
                        {gameSessionInfo.characters && gameSessionInfo.characters.map(c => {
                            return (
                                <CharacterCard key={c.id} template={{...c, name: `${c.name} (${c.ownerId})`}}/>
                            );
                        })}
                        <Chat ws={ws}/>
                        <Dice ws={ws}/>
                    </Box>
                );
            case GameSessionStatusTypeEnum.Finished:
                break;
        }
    }

    function createSignalRConfig(): SignalRProps {
        return {
            beforeStart: (ws) => {
                ws.on(WSEvents.gameSessionInfoReceived, gameSessionInfoReceived);
                ws.on(WSEvents.gameStarted, (data) => console.log(WSEvents.gameStarted, data));
            },
            afterStart: async (ws) => {
                await ws.invoke(WSActions.joinGameSession, gameSessionId);
            },
            beforeStop: async (ws) => {
                ws.off(WSEvents.gameSessionInfoReceived);
                ws.off(WSEvents.gameStarted);
                await ws.invoke(WSActions.leaveGameSession);
            }
        }
    }

    function gameSessionInfoReceived(data: GameSessionInfo) {
        console.log(WSEvents.gameSessionInfoReceived, data)
        setGameSessionInfo(data);
    }

    function gameStarted(data: GameSessionInfo) {
        console.log(WSEvents.gameStarted, data)
        setGameSessionInfo(data);
    }
}

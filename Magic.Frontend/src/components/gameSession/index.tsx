import React, {useEffect} from "react";
import {useGameSessionWS} from "../../utils/webSocket";
import Box from "@mui/material/Box";
import Dice from "../dice/Dice";
import Chat from "./chat";
import CharactersList from "./charactersList/CharactersList";
import {useParams} from "react-router-dom";
import {LinearProgress} from "@mui/material";
import {GameSessionStatusTypeEnum} from "../../models/websocket/gameSessionStatus";
import CharacterCard from "./charactersList/CharacterCard";
import {useSelector} from "react-redux";
import {RootState} from "../../redux";
import {GameSessionCharacter} from "../../models/websocket/gameStartedInfo";

export interface PlayerInfo {
    id: string;
    login: string;
    isMaster: boolean | null;
    lockedCharacterId: string | null;
    isOnline?: boolean;
}


export default function GameSession() {
    const {state, ...api} = useGameSessionWS();
    const {gameSessionId} = useParams();
    const gameSessionFullState = useSelector((state: RootState) => state.gameSession)

    useEffect(() => {
        if (gameSessionId && state === "Connected")
            api.joinGameSession(gameSessionId);
        return () => {
            if (state === "Connected")
                api.leaveGameSession();
        }
    }, [gameSessionId, state])

    return renderGameSessionPage();

    function renderGameSessionPage() {
        if (!gameSessionFullState || !gameSessionFullState.gameSessionInfo)
            return (
                <LinearProgress color="inherit"/>
            )

        switch (gameSessionFullState.gameSessionInfo.gameSessionStatus) {
            case GameSessionStatusTypeEnum.WaitingForStart:
                return (
                    <Box sx={{
                        marginTop: 8,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                    }}>
                        <CharactersList/>
                        <Chat/>
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
                        {gameSessionFullState.gameSessionInfo.characters?.map((c: GameSessionCharacter) => {
                            return (
                                <CharacterCard key={c.id} template={{...c, name: `${c.name} (${c.ownerId})`}}/>
                            );
                        })}
                        <Chat/>
                        <Dice/>
                    </Box>
                );
            case GameSessionStatusTypeEnum.Finished:
                break;
        }
    }
}

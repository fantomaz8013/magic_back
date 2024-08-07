import React, {useEffect} from "react";
import {useGameSessionWS} from "../../utils/webSocket";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Dice from "../dice/Dice";
import Chat from "./chat";
import CharactersList from "./charactersList/CharactersList";
import CharacterLeftMenu from "./charactersList/CharacterLeftMenu";
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

    return (
        <Box sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
        }}>
            {renderGameSessionPage()}
        </Box>
    )

    function renderGameSessionPage() {
        if (!gameSessionFullState || !gameSessionFullState.gameSessionInfo)
            return (
                <LinearProgress color="inherit"/>
            )

        switch (gameSessionFullState.gameSessionInfo.gameSessionStatus) {
            case GameSessionStatusTypeEnum.WaitingForStart:
                return (
                    <>
                        <CharactersList/>
                        <Box sx={{
                            width: '95%',
                            display: 'flex',
                            flexDirection: ' row-reverse',
                        }}>
                            <Chat/>
                        </Box>
                    </>
                );
            case GameSessionStatusTypeEnum.InGame:
                return (
                    <>
                    <Grid container spacing={2}>
                        <Grid item xs={1}>
                            <Box sx={{display: 'flex', flexDirection: 'column'}}>
                                {gameSessionFullState.gameSessionInfo.characters?.map((c: GameSessionCharacter) => {
                                    return (
                                        <CharacterLeftMenu key={c.id} template={{...c, name: `${c.name} (${c.ownerId})`}}/>
                                    );
                                })}
                            </Box>
                        </Grid>
                        <Grid item xs={11}>
                            <Box sx={{
                                height: '85vh',
                                display: 'flex',
                                alignItems: 'end'
                            }}>
                                <Grid container>
                                    <Grid item xs={1} sx={{
                                        display: 'flex',
                                        alignItems: 'end'
                                    }}>
                                        <Box >
                                            <Dice/>
                                        </Box>
                                    </Grid>
                                    <Grid item xs={11}>
                                        <Box sx={{
                                            width: '95%',
                                            display: 'flex',
                                            flexDirection: ' row-reverse',
                                        }}>
                                            <Chat/>
                                        </Box>
                                    </Grid>
                                </Grid>
                            </Box>
                        </Grid>
                    </Grid>
                    </>
                );
            case GameSessionStatusTypeEnum.Finished:
                break;
        }
    }
}

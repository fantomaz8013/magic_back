import React from "react";
import {SignalRProps, useSignalR, WSActions} from "../../utils/webSocket";
import Grid from '@mui/material/Unstable_Grid2';
import Box from "@mui/material/Box";
import {
    CubeTypeEnum,
} from "../../models/websocket/ChatMessage";
import Dice from "../dice/Dice";
import Chat from "./chat";
import CharacterCards from "./charactersList";

export interface GameSessionProps {
    gameSessionId: string;
}

export default function GameSession(props: GameSessionProps) {
    const ws = useSignalR(createSignalRConfig());

    return (
        <Box sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
        }}>
            <Grid container spacing={10} sx={{ maxWidth: '100%' }}>
                <Grid xs={3}>
                    <CharacterCards/>
                </Grid>
                <Grid xs={3}>
                    <CharacterCards/>
                </Grid>
                <Grid xs={3}>
                    <CharacterCards/>
                </Grid>
                <Grid xs={3}>
                    <CharacterCards/>
                </Grid>
            </Grid>

            <Chat ws={ws}/>
            <Dice onDiceRoll={rollDice}/>
        </Box>
    );

    async function rollDice() {
        await ws.invoke(WSActions.rollDice, CubeTypeEnum.D6);
    }

    function createSignalRConfig(): SignalRProps {
        return {
            beforeStart: (ws) => {
            },
            afterStart: async (ws) => {
                await ws.invoke(WSActions.joinGameSession, props.gameSessionId);
            },
            beforeStop: async (ws) => {
                await ws.invoke(WSActions.leaveGameSession, props.gameSessionId);
            }
        }
    }
}

import React from "react";
import {SignalRProps, useSignalR, WSActions, WSEvents} from "../../utils/webSocket";
import Grid from '@mui/material/Unstable_Grid2';
import Box from "@mui/material/Box";
import {
    CubeTypeEnum,
} from "../../models/websocket/ChatMessage";
import Dice from "../dice/Dice";
import Chat from "./chat";
import CharacterCards from "./charactersList";
import {useGetCharacteristicsQuery, useGetCharacterTemplatesQuery} from "../../redux/toolkit/api/characterApi";
import Button from "@mui/material/Button";

export interface GameSessionProps {
    gameSessionId: string;
}

export default function GameSession(props: GameSessionProps) {
    const ws = useSignalR(createSignalRConfig());
    const {data: characterTemplates} = useGetCharacterTemplatesQuery();
    const {data: characteristics} = useGetCharacteristicsQuery();

    return (
        <Box sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
        }}>
            <Grid container spacing={10} sx={{maxWidth: '100%'}}>
                {
                    characteristics?.data && characterTemplates?.data?.map(t =>
                        <Grid key={t.name} xs={3}>
                            <CharacterCards template={t} characteristics={characteristics.data!}/>
                            <Button sx={{marginTop: 4,}}>LOCK</Button>
                        </Grid>)
                }
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
                ws.on(WSEvents.playerInfoReceived, (...data) => console.log(data));
                ws.on(WSEvents.characterLocked, (...data) => console.log(data));
                ws.on(WSEvents.playerLeft, (...data) => console.log(data));
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

import React, {useState} from "react";
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

export interface PlayerInfo {
    id: string;
    login: string;
    isMaster: boolean | null;
    lockedCharacterId: string | null;
}

export default function GameSession(props: GameSessionProps) {
    const ws = useSignalR(createSignalRConfig());
    const {data: characterTemplates} = useGetCharacterTemplatesQuery();
    const {data: characteristics} = useGetCharacteristicsQuery();
    const [playerInfos, setPlayerInfos] = useState<Record<string, PlayerInfo>>();

    return (
        <Box sx={{
            marginTop: 8,
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
        }}>
            <Grid container spacing={10} sx={{maxWidth: '100%'}}>
                {
                    playerInfos && characteristics?.data && characterTemplates?.data?.map(t => {
                        const isLocked = Object
                            .values(playerInfos)
                            .filter(p => p.lockedCharacterId === t.id)
                            ?.[0];
                        return (
                            <Grid key={t.name} xs={3}>
                                <CharacterCards template={t} characteristics={characteristics.data!}/>
                                <Button
                                    disabled={!!isLocked} id={t.id} sx={{marginTop: 4,}}
                                    onClick={lockCharacter}>
                                    LOCK{isLocked && `ED BY ${isLocked.login}`}
                                </Button>
                            </Grid>
                        );
                    })
                }
            </Grid>
            <Chat ws={ws}/>
            <Dice onDiceRoll={rollDice}/>
        </Box>
    );

    async function lockCharacter(e: React.MouseEvent<HTMLButtonElement>) {
        await ws.invoke(WSActions.lockCharacter, e.currentTarget.id)
    }

    async function rollDice() {
        await ws.invoke(WSActions.rollDice, CubeTypeEnum.D6);
    }

    function createSignalRConfig(): SignalRProps {
        return {
            beforeStart: (ws) => {
                ws.on(WSEvents.playerInfoReceived, playerInfoReceived);
                ws.on(WSEvents.characterLocked, characterLocked);
                ws.on(WSEvents.playerLeft, playerLeft);
            },
            afterStart: async (ws) => {
                await ws.invoke(WSActions.joinGameSession, props.gameSessionId);
            },
            beforeStop: async (ws) => {
                ws.off(WSEvents.playerInfoReceived);
                ws.off(WSEvents.characterLocked);
                ws.off(WSEvents.playerLeft);
                await ws.invoke(WSActions.leaveGameSession);
            }
        }
    }

    function playerInfoReceived(playerInfos: PlayerInfo[]) {
        console.log(WSEvents.playerInfoReceived, playerInfos);
        setPlayerInfos(playerInfos.reduce((pv, cv) => {
            pv[cv.id] = cv;
            return pv;
        }, {} as Record<string, PlayerInfo>));
    }

    function characterLocked(userId: string, lockedCharacterTemplateId: string) {
        setPlayerInfos(prevState => {
            const newState = {...prevState} || {};
            newState[userId] = {...newState[userId], lockedCharacterId: lockedCharacterTemplateId};
            console.log(WSEvents.characterLocked, newState);
            return newState;
        });
    }

    function playerLeft(userId: string) {
        setPlayerInfos(prevState => {
            const newState = {...prevState} || {};
            delete newState[userId];
            console.log(WSEvents.playerLeft, newState);
            return newState;
        });
    }
}

import * as React from 'react';
import Grid from "@mui/material/Unstable_Grid2";
import CharacterCard from "./CharacterCard";
import Button from "@mui/material/Button";
import {WithWS, WSActions, WSEvents} from "../../../utils/webSocket";
import {useGetCharacterTemplatesQuery} from "../../../redux/toolkit/api/characterApi";
import {useGetCurrentUserQuery} from "../../../redux/toolkit/api/userApi";
import {useEffect, useState} from "react";
import {PlayerInfo} from "../index";
import {CharacterTemplate} from "../../../models/response/characterTemplateResponse";
import Box from "@mui/material/Box";

export default function CharacterList({ws}: WithWS) {
    const {data: characterTemplates} = useGetCharacterTemplatesQuery();
    const {data: currentUser} = useGetCurrentUserQuery();
    const [playerInfos, setPlayerInfos] = useState<Record<string, PlayerInfo>>();

    useEffect(() => {
        ws.on(WSEvents.playerInfoReceived, playerInfoReceived);
        ws.on(WSEvents.characterLocked, characterLocked);
        ws.on(WSEvents.characterUnlocked, characterUnlocked);
        ws.on(WSEvents.playerLeft, playerLeft);
        return () => {
            ws.off(WSEvents.playerInfoReceived);
            ws.off(WSEvents.characterLocked);
            ws.off(WSEvents.characterUnlocked);
            ws.off(WSEvents.playerLeft);
        }
    }, []);

    const isDataLoaded = currentUser && currentUser.data && playerInfos && characterTemplates?.data != null;
    const isGameMaster = isDataLoaded && playerInfos[currentUser.data!.id].isMaster;
    const isAnyCharacterLocked = isDataLoaded && Object
        .values(playerInfos)
        .filter(p => p.lockedCharacterId !== null)
        .length > 0;

    return (
        <Box sx={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
        }}>
            <Grid container spacing={10} sx={{maxWidth: '100%', maxHeight: '80%'}}>
                {
                    isDataLoaded && characterTemplates.data?.map(renderCharacterTemplate)
                }
            </Grid>
            {isGameMaster && <Button onClick={startGame} disabled={!isAnyCharacterLocked}>START GAME</Button>}
        </Box>
    );

    async function startGame() {
        await ws.invoke(WSActions.startGame);
    }

    function renderCharacterTemplate(t: CharacterTemplate) {
        const lockInfo = Object
            .values(playerInfos!)
            .filter(p => p.lockedCharacterId === t.id)
            ?.[0];
        const isLocked = isGameMaster || (lockInfo && lockInfo.id !== currentUser!.data!.id);
        const text = lockInfo
            ? isLocked
                ? `LOCKED BY ${lockInfo.login}`
                : `UNLOCK`
            : isGameMaster
                ? 'WAITING TO BO LOCKED'
                : `LOCK`
        const lockFunc = lockInfo
            ? isLocked
                ? lockCharacter
                : unlockCharacter
            : lockCharacter
        return (
            <Grid key={t.name} xs={3}>
                <CharacterCard template={t}/>
                <Button
                    disabled={isLocked} id={t.id} sx={{marginTop: 4,}}
                    onClick={lockFunc}>
                    {text}
                </Button>
            </Grid>
        );
    }

    async function lockCharacter(e: React.MouseEvent<HTMLButtonElement>) {
        await ws.invoke(WSActions.lockCharacter, e.currentTarget.id)
    }

    async function unlockCharacter() {
        await ws.invoke(WSActions.unlockCharacter)
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

    function characterUnlocked(userId: string) {
        setPlayerInfos(prevState => {
            const newState = {...prevState} || {};
            newState[userId] = {...newState[userId], lockedCharacterId: null};
            console.log(WSEvents.characterUnlocked, newState);
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

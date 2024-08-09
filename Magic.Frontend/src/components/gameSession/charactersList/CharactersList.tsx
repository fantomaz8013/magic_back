import * as React from 'react';
import Grid from "@mui/material/Unstable_Grid2";
import CharacterCard from "./CharacterCard";
import Button from "@mui/material/Button";
import {useGetCharacterTemplatesQuery} from "../../../redux/api/characterApi";
import {useGetCurrentUserQuery} from "../../../redux/api/userApi";
import {CharacterTemplateResponse} from "../../../models/response/characterTemplateResponse";
import Box from "@mui/material/Box";
import {useSelector} from "react-redux";
import {RootState} from "../../../redux/redux";
import {socket} from "../../../webSocket/webSocket";

export default function CharacterList() {
    const {data: characterTemplates} = useGetCharacterTemplatesQuery();
    const {data: currentUser} = useGetCurrentUserQuery();
    const playerInfos = useSelector((state: RootState) => state.gameSession?.playerInfos);

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
            {isGameMaster && <Button onClick={socket?.startGame} disabled={!isAnyCharacterLocked}>START GAME</Button>}
        </Box>
    );

    function renderCharacterTemplate(t: CharacterTemplateResponse) {
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
                ? 'WAITING TO BE LOCKED'
                : `LOCK`

        return (
            <Grid key={t.name} xs={3}>
                <CharacterCard template={t}/>
                <Button
                    disabled={isLocked} id={t.id} sx={{marginTop: 4,}}
                    onClick={_lock}>
                    {text}
                </Button>
            </Grid>
        );
    }

    async function _lock(e: React.MouseEvent<HTMLButtonElement>) {
        const characterId = e.currentTarget.id;
        const lockInfo = Object
            .values(playerInfos!)
            .filter(p => p.lockedCharacterId === characterId)
            ?.[0];
        const isLocked = isGameMaster || (lockInfo && lockInfo.id !== currentUser!.data!.id);

        const lockFunc = lockInfo
            ? isLocked
                ? socket!.lockCharacter
                : socket!.unlockCharacter
            : socket!.lockCharacter;

        await lockFunc(characterId);
    }
}

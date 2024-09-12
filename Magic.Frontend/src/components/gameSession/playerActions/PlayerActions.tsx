import Paper from "@mui/material/Paper";
import Grid from "@mui/material/Grid";
import Dice from "../dice/Dice";
import React from "react";
import {setMoving} from "../../../redux/slices/moveSlice";
import Button from "@mui/material/Button";
import {useDispatch, useSelector} from "react-redux";
import {AppDispatch, RootState} from "../../../redux/redux";
import {useGetCurrentUserQuery} from "../../../redux/api/userApi";
import {socket} from "../../../webSocket/webSocket";
import {useGetAbilitiesQuery} from "../../../redux/api/characterApi";

export function PlayerActions() {
    const dispatch = useDispatch<AppDispatch>();
    const characters = useSelector((state: RootState) => state.gameSession.gameSessionInfo?.characters);
    const {data: currentUser,} = useGetCurrentUserQuery();
    const {data: abilities,} = useGetAbilitiesQuery();
    const moveState = useSelector((state: RootState) => state.move);
    const playerInfos = useSelector((state: RootState) => state.gameSession.playerInfos);

    const isGameMaster = (currentUser && currentUser.data && playerInfos[currentUser.data.id]?.isMaster) || false;

    return (
        <Paper>
            <Grid
                item
                sx={{
                    display: 'flex',
                    flexDirection: "column",
                }}>
                <Dice/>
                <Button onClick={onEndTurnClick}>Закончить ход</Button>
                {renderStartMoving()}
                {renderAbilities()}
                {renderMasterActions()}
            </Grid>
        </Paper>
    );

    function renderStartMoving() {
        const text = moveState.movingCharacterId ? 'Закончить перемещение' : 'Начать перемещение';
        return (
            <Button onClick={onStartMovingClick}>{text}</Button>
        )
    }

    function renderAbilities() {
        if (!characters || !abilities || !abilities.data) return;
        const userCharacter = characters.find(c => c.ownerId === currentUser!.data!.id);
        if (!userCharacter) return;
        const _abilities = abilities.data.filter(a => userCharacter.abilitieIds.includes(a.id));
        return _abilities.map(a => (
            <Button id={a.id.toString()} onClick={onApplyAbilityClick}>Применить {a.title}</Button>
        ));
    }

    //todo move to MasterActions
    function renderMasterActions() {
        if (!isGameMaster) return;
        return <>
            <Button onClick={onStartTurnBasedClick}>Начать пошаговый режим</Button>
            <Button onClick={onEndTurnBasedClick}>Закончить пошаговый режим</Button>
        </>
    }

    async function onStartMovingClick() {
        const newMovingUserId = moveState.movingCharacterId
            ? null
            : characters!.find(c => c.ownerId === currentUser!.data!.id)!.id;
        if (moveState.path.length > 0) {
            await socket!.moveCharacter(moveState.movingCharacterId!, moveState.path);
            dispatch(setMoving(null));
        } else {
            dispatch(setMoving(newMovingUserId));
        }
    }

    async function onApplyAbilityClick(event: React.MouseEvent<HTMLButtonElement>) {
        if (!characters || !abilities || !abilities.data) return;
        const userCharacter = characters.find(c => c.ownerId === currentUser!.data!.id);
        if (!userCharacter) return;
        const abilityId = parseInt(event.currentTarget.id);

        await socket!.useAbility(abilityId, userCharacter.id, 5, 5);
    }

    async function onStartTurnBasedClick() {
        await socket!.startTurnBased('1850beb4-ed84-4c7f-1234-cd7bce35e5d4');
    }

    async function onEndTurnBasedClick() {
        await socket!.endTurnBased();
    }

    async function onEndTurnClick() {
        if (!characters || !abilities || !abilities.data) return;
        const userCharacter = characters.find(c => c.ownerId === currentUser!.data!.id);
        if (!userCharacter) return;
        await socket!.endTurn(userCharacter.id);
    }
}

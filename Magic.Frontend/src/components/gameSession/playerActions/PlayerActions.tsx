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

export function PlayerActions() {
    const dispatch = useDispatch<AppDispatch>();
    const characters = useSelector((state: RootState) => state.gameSession.gameSessionInfo?.characters);
    const {data: currentUser,} = useGetCurrentUserQuery();
    const moveState = useSelector((state: RootState) => state.move);

    return (
        <Paper>
            <Grid
                item
                sx={{
                    display: 'flex',
                    flexDirection: "column",
                }}>
                <Dice/>
                {renderStartMoving()}
            </Grid>
        </Paper>
    );

    function renderStartMoving() {
        const text = moveState.movingCharacterId ? 'Закончить перемещение' : 'Начать перемещение';
        return (
            <Button onClick={onStartMovingClick}>{text}</Button>
        )
    }

    async function onStartMovingClick() {
        const newMovingUserId = moveState.movingCharacterId ? null : characters!.find(c => c.ownerId === currentUser!.data!.id)!.id;
        if (moveState.path.length > 0) {
            await socket!.moveCharacter(moveState.movingCharacterId!, moveState.path);
            dispatch(setMoving(null));
        } else {
            dispatch(setMoving(newMovingUserId));
        }
    }
}

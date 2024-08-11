import Paper from "@mui/material/Paper";
import Grid from "@mui/material/Grid";
import Dice from "../dice/Dice";
import React from "react";
import {setMoving} from "../../../redux/slices/moveSlice";
import Button from "@mui/material/Button";
import {useDispatch, useSelector} from "react-redux";
import {AppDispatch, RootState} from "../../../redux/redux";
import {useGetCurrentUserQuery} from "../../../redux/api/userApi";

export function PlayerActions() {
    const dispatch = useDispatch<AppDispatch>();
    const {data: currentUserQuery} = useGetCurrentUserQuery();
    const movingUserId = useSelector((state: RootState) => state.move.movingUserId);

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
        const text = movingUserId ? 'Закончить перемещение' : 'Начать перемещение';
        return (
            <Button onClick={onStartMovingClick}>{text}</Button>
        )
    }

    function onStartMovingClick() {
        const newMovingUserId = movingUserId ? null : currentUserQuery?.data?.id || null;
        dispatch(setMoving(newMovingUserId));
    }
}

import React from "react";
import {Modal} from "@mui/material";
import Box from "@mui/material/Box";
import {ModalStyle, SavingThrowEnumMapper} from "../GameSession.utils";
import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import {socket} from "../../../webSocket/webSocket";
import {setRequestSaveThrow} from "../../../redux/slices/gameSessionSlice";
import {useGetCharacteristicsQuery} from "../../../redux/api/characterApi";
import {useDispatch, useSelector} from "react-redux";
import {AppDispatch, RootState} from "../../../redux/redux";

export function UserUtils() {
    const {data: characteristics} = useGetCharacteristicsQuery();
    const dispatch = useDispatch<AppDispatch>();
    const requestedSaveThrow = useSelector((state: RootState) => state.gameSession.requestedSaveThrow);

    if (!requestedSaveThrow) {
        return (
            <React.Fragment/>
        )
    }


    return (
        <Modal open={true}>
            <Box sx={ModalStyle}>
                <Typography variant="h6" component="h2">
                    Сделайте спас-бросок
                </Typography>
                <Typography>
                    Характеристика: {getCharacteristicTitle(requestedSaveThrow.characterCharacteristicId)}
                    <br/>
                    Сложность: {getCharacteristicValue(requestedSaveThrow.value)}
                </Typography>
                <Button sx={{mt: 2}} onClick={onSaveRollDiceClick}>Сделать бросок</Button>
            </Box>
        </Modal>
    );

    async function onSaveRollDiceClick() {
        await socket!.rollSaveDice();
        dispatch(setRequestSaveThrow(null));
    }

    function getCharacteristicTitle(characterCharacteristicId: number) {
        return characteristics!.data!.find(c => c.id === characterCharacteristicId)!.title;
    }

    function getCharacteristicValue(characterCharacteristicValue: number) {
        return SavingThrowEnumMapper[characterCharacteristicValue] || characterCharacteristicValue;
    }
}

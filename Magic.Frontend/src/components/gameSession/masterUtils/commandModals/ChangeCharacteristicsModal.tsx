import Typography from "@mui/material/Typography";
import {fillableFields, getUserLoginById} from "../MasterUtils.utils";
import {FormControl, InputLabel, Select, SelectChangeEvent, TextField} from "@mui/material";
import MenuItem from "@mui/material/MenuItem";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import React, {useState} from "react";
import {GameSessionCharacter} from "../../../../models/websocket/gameStartedInfo";
import {socket} from "../../../../webSocket/webSocket";
import {ChangingField} from "../MasterUtils.types";
import {useSelector} from "react-redux";
import {RootState} from "../../../../redux/redux";
import {ModalProps} from "./commandModals.types";

export function ChangeCharacteristicsModal({userId, onCloseModal}: ModalProps) {
    const [changingParameter, setChangingParameter] = useState<ChangingField | null>(null);
    const {playerInfos, gameSessionInfo} = useSelector((state: RootState) => state.gameSession);

    return (
        <>
            <Typography variant="h6" component="h2">
                Изменить параметры у игрока {getUserLoginById(playerInfos, userId)}
            </Typography>
            {renderParameterTypeSelect()}
            {renderParameterValueInput()}
        </>
    );

    function renderParameterTypeSelect() {
        return (
            <FormControl sx={{mt: 2}} fullWidth>
                <InputLabel>Параметр</InputLabel>
                <Select
                    value={changingParameter?.name || ''}
                    label={"Параметр"}
                    onChange={onParameterNameSelectChange}
                >
                    {Object.entries(fillableFields).map(([key, value]) => {
                        return (
                            <MenuItem
                                key={key}
                                value={key}>
                                {value.name}
                            </MenuItem>
                        );
                    })}
                </Select>
            </FormControl>
        );
    }

    function renderParameterValueInput() {
        if (!changingParameter || changingParameter?.value === null) {
            return <React.Fragment/>
        }

        return (
            <FormControl sx={{mt: 2}} fullWidth>
                {changingParameter.initialValue &&
                    <Typography>
                        Начальное значение
                        <br/>
                        {changingParameter.initialValue}
                    </Typography>}
                <TextField
                    value={changingParameter.value.toString()}
                    onChange={onParameterValueChange}
                />
                <Box>
                    <Button sx={{mt: 2}} onClick={saveChangedParameter}>Изменить</Button>
                </Box>
            </FormControl>
        );
    }

    async function saveChangedParameter() {
        const character = getCharacterByCommandTarget();
        const newChar: Record<string, string> = {};
        newChar[changingParameter!.name] = changingParameter!.value;
        await socket!.changeCharacter(character.id, newChar);
        setChangingParameter(null);
        onCloseModal();
    }

    function onParameterNameSelectChange(event: SelectChangeEvent) {
        const parameterName = event.target.value as keyof GameSessionCharacter;
        const character = getCharacterByCommandTarget();
        setChangingParameter({
            name: parameterName,
            initialValue: character[parameterName]?.toString()!,
            value: character[parameterName]?.toString()! || '',
        });
    }

    function onParameterValueChange(event: React.ChangeEvent<HTMLTextAreaElement>) {
        const parameterValue = event.currentTarget.value;
        setChangingParameter({...changingParameter!, value: parameterValue});
    }

    function getCharacterByCommandTarget() {
        return gameSessionInfo?.characters?.find(c => c.ownerId === userId)!;
    }
}

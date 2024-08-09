import Typography from "@mui/material/Typography";
import {FormControl, InputLabel, Select, SelectChangeEvent} from "@mui/material";
import MenuItem from "@mui/material/MenuItem";
import {SavingThrowEnum, SavingThrowEnumMapper} from "../../GameSession.utils";
import Button from "@mui/material/Button";
import React from "react";
import {CharacterCharacteristicIds} from "../../../../models/response/characterTemplateResponse";
import {CharacteristicChangeDetails} from "../MasterUtils.types";
import {defaultCharacteristic} from "../MasterUtils.utils";
import {useGetCharacteristicsQuery} from "../../../../redux/toolkit/api/characterApi";
import {socket} from "../../../../webSocket/webSocket";
import {ModalProps} from "./commandModals.types";


export function RequestSaveThrowModal({onCloseModal, userId}: ModalProps) {
    const [characteristic, setCharacteristic] = React.useState<CharacteristicChangeDetails>(defaultCharacteristic);
    const {data: characteristics} = useGetCharacteristicsQuery();

    return (
        <>
            <Typography variant="h6" component="h2">
                Запросить спас-бросок
            </Typography>
            <FormControl sx={{mt: 2}} fullWidth>
                <InputLabel>Характеристика</InputLabel>
                <Select
                    value={characteristic.type.toString()}
                    label="Характеристика"
                    onChange={onCharacteristicTypeChange}
                >
                    {characteristics!.data!.map(({id, title}) => {
                        return (
                            <MenuItem
                                key={id}
                                value={id}>
                                {title}
                            </MenuItem>
                        );
                    })}
                </Select>
            </FormControl>
            <FormControl sx={{mt: 2}} fullWidth>
                <InputLabel>Сложность спас-броска</InputLabel>
                <Select
                    value={characteristic.value.toString()}
                    label="Сложность спас-броска"
                    onChange={onCharacteristicValueChange}
                >
                    {Object.entries(SavingThrowEnum).map(([key, value]) => {
                        return (
                            <MenuItem
                                key={key}
                                value={value}>
                                {SavingThrowEnumMapper[value]}
                            </MenuItem>
                        );
                    })}
                </Select>
            </FormControl>
            <Button sx={{mt: 2}} onClick={onTestPlayerClick}>Запросить</Button>
        </>
    );

    async function onTestPlayerClick() {
        await socket!.requestSaveThrow(userId, characteristic.type, characteristic.value);
        onCloseModal();
    }

    function onCharacteristicValueChange(e: SelectChangeEvent) {
        const value = parseInt(e.target.value);
        setCharacteristic({...characteristic, value});
    }

    function onCharacteristicTypeChange(event: SelectChangeEvent) {
        const type = event.target.value as unknown as CharacterCharacteristicIds;
        setCharacteristic({...characteristic, type});
    }
}

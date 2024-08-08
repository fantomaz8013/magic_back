import React from "react";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import {CharacterCharacteristicIds} from "../../../models/response/characterTemplateResponse";

export interface MasterToPlayerCommandProps {
    anchorEl: HTMLElement | null;
    onClose: () => void;
    onStartCommandClick: (type: MasterCommands) => void;
}

export enum MasterCommands {
    Kick = 'Kick',
    Test = 'Test',
    ChangeHp = 'ChangeHp',
}

export function MasterToPlayerCommand({anchorEl, onStartCommandClick, onClose}: MasterToPlayerCommandProps) {
    return (
        <Menu
            id="basic-menu"
            anchorEl={anchorEl}
            open={!!anchorEl}
            onClose={onClose}
            MenuListProps={{
                'aria-labelledby': 'basic-button',
            }}
        >
            <MenuItem id={MasterCommands.Kick} onClick={_onStartCommandClick}>Выгнать</MenuItem>
            <MenuItem id={MasterCommands.Test} onClick={_onStartCommandClick}>Запросить спас-бросок</MenuItem>
            <MenuItem id={MasterCommands.Test} onClick={_onStartCommandClick}>Изменить здоровье</MenuItem>
        </Menu>
    );

    function _onStartCommandClick(e: React.MouseEvent<HTMLLIElement>) {
        onStartCommandClick(e.currentTarget.id as MasterCommands);
    }
}

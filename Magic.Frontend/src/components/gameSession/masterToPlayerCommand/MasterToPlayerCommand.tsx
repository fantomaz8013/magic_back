import React from "react";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";

export interface MasterToPlayerCommandProps {
    anchorEl: HTMLElement | null;
    onClose: () => void;
    onStartCommandClick: (type: MasterCommands) => void;
}

export enum MasterCommands {
    Kick = 'Kick',
    RequestSaveThrow = 'RequestSaveThrow',
    ChangeCharacteristics = 'ChangeCharacteristics',
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
            <MenuItem id={MasterCommands.RequestSaveThrow} onClick={_onStartCommandClick}>Запросить спас-бросок</MenuItem>
            <MenuItem id={MasterCommands.ChangeCharacteristics} onClick={_onStartCommandClick}>Изменить характеристики</MenuItem>
        </Menu>
    );

    function _onStartCommandClick(e: React.MouseEvent<HTMLLIElement>) {
        onStartCommandClick(e.currentTarget.id as MasterCommands);
    }
}

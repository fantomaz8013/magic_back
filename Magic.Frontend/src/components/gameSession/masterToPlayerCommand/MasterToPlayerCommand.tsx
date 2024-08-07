import React from "react";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";

export interface MasterToPlayerCommandProps {
    anchorEl: HTMLElement | null;
    setAnchorEl: (target: HTMLElement | null) => void;
}

export function MasterToPlayerCommand(props: MasterToPlayerCommandProps) {
    return (
        <Menu
            id="basic-menu"
            anchorEl={props.anchorEl}
            open={!!props.anchorEl}
            onClose={handleClose}
            MenuListProps={{
                'aria-labelledby': 'basic-button',
            }}
        >
            <MenuItem onClick={handleClose}>Выгнать</MenuItem>
            <MenuItem onClick={handleClose}>Запросить спас-бросок</MenuItem>
        </Menu>
    );

    function handleClick(event: React.MouseEvent<HTMLButtonElement>) {
        props.setAnchorEl(event.currentTarget);
    }

    function handleClose() {
        props.setAnchorEl(null);
    }
}

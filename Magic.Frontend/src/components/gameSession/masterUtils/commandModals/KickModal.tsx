import Typography from "@mui/material/Typography";
import Button from "@mui/material/Button";
import React from "react";
import {socket} from "../../../../webSocket/webSocket";
import {getUserLoginById} from "../MasterUtils.utils";
import {useSelector} from "react-redux";
import {RootState} from "../../../../redux/redux";
import {ModalProps} from "./commandModals.utils";


export function KickModal({userId, onCloseModal}: ModalProps) {
    const playerInfos = useSelector((state: RootState) => state.gameSession.playerInfos);

    return (<>
        <Typography variant="h6" component="h2">
            Вы уверены, что хотите выгнать
            игрока {getUserLoginById(playerInfos, userId)}?
        </Typography>
        <Button sx={{mt: 2}} onClick={onKickPlayerClick}>Выгнать</Button>
    </>);

    async function onKickPlayerClick() {
        await socket!.kick(userId);
        onCloseModal();
    }
}

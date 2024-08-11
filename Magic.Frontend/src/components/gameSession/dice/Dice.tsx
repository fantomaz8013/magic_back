import * as React from "react";
import Button from "@mui/material/Button";
import {CubeTypeEnum} from "../../../models/websocket/chatMessage";
import {socket} from "../../../webSocket/webSocket";

export default function Dice() {
    return (
        <Button onClick={_rollDice}>
            Кинуть кубик
        </Button>
    );

    async function _rollDice() {
        await socket!.rollDice(CubeTypeEnum.D6);
    }
}

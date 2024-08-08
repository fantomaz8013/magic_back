import * as React from "react";
import Button from "@mui/material/Button";
import {CubeTypeEnum} from "../../../models/websocket/ChatMessage";
import {socket} from "../../../utils/webSocket";

export default function Dice() {
    return (
        <Button
            onClick={_rollDice}
        >
            ROLL
        </Button>
    );

    async function _rollDice() {
        await socket!.rollDice(CubeTypeEnum.D6);
    }
}

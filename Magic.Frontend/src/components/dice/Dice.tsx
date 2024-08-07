import * as React from "react";
import Button from "@mui/material/Button";
import {CubeTypeEnum} from "../../models/websocket/ChatMessage";
import {socket} from "../../utils/webSocket";

export default function Dice() {
    return (
        <Button
            onClick={_rollDice}
            sx={{
                position: 'fixed',
                bottom: 0,
                width: '25%',
                height: 40,
                right: '75%',
                textAlign: 'center'
            }}
        >
            ROLL
        </Button>
    );

    async function _rollDice() {
        await socket!.rollDice(CubeTypeEnum.D6);
    }
}

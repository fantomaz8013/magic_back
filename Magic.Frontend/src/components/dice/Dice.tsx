import * as React from "react";
import Button from "@mui/material/Button";
import {WithWS, WSActions} from "../../utils/webSocket";
import {CubeTypeEnum} from "../../models/websocket/ChatMessage";

export default function Dice({ws}: WithWS) {
    return (
        <Button
            onClick={rollDice}
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

    async function rollDice() {
        await ws.invoke(WSActions.rollDice, CubeTypeEnum.D6);
    }
}

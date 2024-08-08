import * as React from "react";
import Button from "@mui/material/Button";
import {CubeTypeEnum} from "../../../models/websocket/ChatMessage";
import {socket} from "../../../utils/webSocket";
import {Accordion, AccordionDetails, AccordionSummary} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import Typography from "@mui/material/Typography";

export default function Dice() {
    return (
        <Accordion sx={{
            width: '400px',
        }}>
            <AccordionSummary
                expandIcon={<ExpandMoreIcon/>}
            >
                <Typography mb={4} component="h1" variant="h5">
                    Actions
                </Typography>
            </AccordionSummary>
            <AccordionDetails>
                <Button
                    onClick={_rollDice}
                >
                    ROLL
                </Button>
            </AccordionDetails>
        </Accordion>
    );

    async function _rollDice() {
        await socket!.rollDice(CubeTypeEnum.D6);
    }
}

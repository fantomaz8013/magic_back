import Typography from "@mui/material/Typography";
import * as React from "react";
import Button from "@mui/material/Button";

export interface DiceProps {
    onDiceRoll: () => void;
}

export default function Dice(props: DiceProps) {
    return (
        <Button
            onClick={props.onDiceRoll}
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
}

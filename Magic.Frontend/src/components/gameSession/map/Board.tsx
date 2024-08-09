import Paper from "@mui/material/Paper";
import React from "react";
import {GameSessionCharacter} from "../../../models/websocket/gameStartedInfo";
import {Row} from "./Row";

interface BoardProps {
    board: number[][];
    charPositions: CharPositions;
}

export type CharPositions = Record<string, GameSessionCharacter>


export const Board = ({board, charPositions}: BoardProps) => {
    return (
        <Paper sx={{
            maxWidth: '100%',
            zIndex: '0',
            position: 'absolute',
            mt: '2%',
            ml: '20%',
            display: "flex",
            flexDirection: "column",
            gridArea: "2 / 2 / span 8 / span 8",
        }} elevation={4}>
            {
                board.map((row, rowIdx) => {
                    return (
                        <Row key={rowIdx} row={row} rowIdx={rowIdx} charPositions={charPositions}/>
                    );
                })
            }
        </Paper>
    )
}

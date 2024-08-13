import Paper from "@mui/material/Paper";
import React from "react";
import {Row} from "./Row";

interface BoardProps {
    board: number[][];
}

export const Board = ({board}: BoardProps) => {
    return (
        <Paper id={'Board'} sx={{
            maxWidth: '100%',
            position: 'absolute',
            flexDirection: "column",
        }} elevation={4}>
            {
                board.map((row, rowIdx) => {
                    return (
                        <Row key={rowIdx} row={row} rowIdx={rowIdx}/>
                    );
                })
            }
        </Paper>
    )
}

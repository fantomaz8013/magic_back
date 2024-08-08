import React from "react";
import {Stack} from "@mui/material";
import Paper from "@mui/material/Paper";
import {useSelector} from "react-redux";
import {RootState} from "../../../redux/redux";

export function Map() {
    const gameSessionInfo = useSelector((state: RootState) => state.gameSession.gameSessionInfo);

    if (!gameSessionInfo?.map || !gameSessionInfo.characters) {
        return (
            <React.Fragment/>
        );
    }

    const map = JSON.parse(JSON.stringify(gameSessionInfo.map.tiles)) as number[][];

    for (const character of gameSessionInfo.characters) {
        if (Number.isFinite(character.positionY) && Number.isFinite(character.positionX))
            map[character.positionY!][character.positionX!] = 1;
    }

    return (
        <Board board={map}/>
    );
}

const rowKey = (rowIdx: number) => `${rowIdx + 1}`;

const cellKey = (rowIdx: number, colIdx: number) => `${rowKey(rowIdx)}:${colIdx}`;

const Row = ({row, rowIdx}: { row: number[], rowIdx: number }) => {
    const darkCellBackgroundColor = "rgba(82, 103, 8, 0.9)";
    const lightCellBackgroundColor = "rgba(230, 233, 220, 0.9)";
    const cellSize = "min(7vw, 7vh)";
    return (
        <Stack direction="row" sx={{
            display: "flex",
            maxHeight: '50px',
            flex: `1 1 ${cellSize}`,
            mb: '1px',
        }}>
            {
                row.map((piece, colIdx) => {
                    return (
                        <Paper key={cellKey(rowIdx, colIdx)} sx={{
                            flex: "1 1 min(9vw, 9vh)",
                            width: '50px',
                            height: '50px',
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "center",
                            mr: '1px',
                            bgcolor: piece === 1 ? lightCellBackgroundColor : darkCellBackgroundColor,
                            borderRadius: 0,
                            fontSize: cellSize,
                        }} elevation={0}>
                        </Paper>
                    );
                })
            }
        </Stack>
    );
}

export const Board = ({board}: { board: number[][] }) => {
    return (
        <Paper sx={{
            maxWidth: '100%',
            zIndex: '0',
            position: 'absolute',
            mt: '2%',
            ml: '20%',
            display: "flex",
            flexDirection: "column-reverse",
            gridArea: "2 / 2 / span 8 / span 8",
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

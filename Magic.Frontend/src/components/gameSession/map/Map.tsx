import React from "react";
import {Stack} from "@mui/material";
import Paper from "@mui/material/Paper";
import {useSelector} from "react-redux";
import {RootState} from "../../../redux/redux";
import {GameSessionCharacter} from "../../../models/websocket/gameStartedInfo";
import {useGetTilePropertiesQuery} from "../../../redux/toolkit/api/mapApi";
import {baseProxy} from "../../../env";

export function Map() {
    const gameSessionInfo = useSelector((state: RootState) => state.gameSession.gameSessionInfo);

    if (!gameSessionInfo?.map || !gameSessionInfo.characters) {
        return (
            <React.Fragment/>
        );
    }

    const charPositions = gameSessionInfo.characters
        .reduce((pv, cv) => {
            if (Number.isFinite(cv.positionY) && Number.isFinite(cv.positionX)) {
                pv[cellKey(cv.positionY!, cv.positionX!)] = cv;
            }
            return pv;
        }, {} as CharPositions)

    return (
        <Board board={gameSessionInfo.map.tiles} charPositions={charPositions}/>
    );
}

const rowKey = (rowIdx: number) => `${rowIdx + 1}`;

const cellKey = (rowIdx: number, colIdx: number) => `${rowKey(rowIdx)}:${colIdx}`;

interface RowProps {
    row: number[];
    rowIdx: number;
    charPositions: CharPositions;
}

const Row = ({row, rowIdx, charPositions,}: RowProps) => {
    const {data: tileProperties} = useGetTilePropertiesQuery();
    const darkCellBackgroundColor = "rgba(82, 103, 8, 0.9)";
    const lightCellBackgroundColor = "rgba(230, 233, 220, 0.9)";
    const cellSize = "min(7vw, 7vh)";
    return (
        <Stack
            direction="row"
            sx={{
                display: "flex",
                maxHeight: '50px',
                flex: `1 1 ${cellSize}`,
                mb: '1px',
            }}>
            {
                row.map((piece, colIdx) => {
                    const character = charPositions[cellKey(rowIdx, colIdx)];
                    const property = tileProperties && tileProperties.data && tileProperties.data[piece];
                    return (
                        <Paper key={cellKey(rowIdx, colIdx)} sx={{
                            flex: "1 1 min(9vw, 9vh)",
                            width: '50px',
                            height: '50px',
                            display: "flex",
                            alignItems: "center",
                            justifyContent: "center",
                            backgroundImage: property && `url(${baseProxy}${property.image})`,
                            mr: '1px',
                            bgcolor: character ? lightCellBackgroundColor : darkCellBackgroundColor,
                            borderRadius: 0,
                            fontSize: cellSize,
                        }} elevation={0}
                        />
                    );
                })
            }
        </Stack>
    );
}

type CharPositions = Record<string, GameSessionCharacter>

interface BoardProps {
    board: number[][];
    charPositions: CharPositions;
}

export const Board = ({board, charPositions}: BoardProps) => {
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
                        <Row key={rowIdx} row={row} rowIdx={rowIdx} charPositions={charPositions}/>
                    );
                })
            }
        </Paper>
    )
}

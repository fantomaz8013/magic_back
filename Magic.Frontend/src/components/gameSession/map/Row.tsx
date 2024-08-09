import {useGetTilePropertiesQuery} from "../../../redux/api/mapApi";
import {Avatar, Stack} from "@mui/material";
import Paper from "@mui/material/Paper";
import {baseProxy} from "../../../env";
import React from "react";
import {CharPositions} from "./Board";
import {cellKey} from "./map.utils";

interface RowProps {
    row: number[];
    rowIdx: number;
    charPositions: CharPositions;
}

export const Row = ({row, rowIdx, charPositions,}: RowProps) => {
    const {data: tileProperties} = useGetTilePropertiesQuery();
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
                            backgroundSize: 'cover',
                            backgroundRepeat: 'no-repeat',
                            backgroundPosition: "center",
                            backgroundImage: property && `url(${baseProxy}${property.image})`,
                            mr: '1px',
                            borderRadius: 0,
                            fontSize: cellSize,
                        }} elevation={0}>
                            {character && <Avatar src={character.avatarUrL}/>}
                        </Paper>
                    );
                })
            }
        </Stack>
    );
}

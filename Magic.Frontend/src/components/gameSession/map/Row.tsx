import {Avatar, Stack} from "@mui/material";
import Paper from "@mui/material/Paper";
import {baseProxy} from "../../../env";
import React, {useContext} from "react";
import {cellKey} from "./map.utils";
import {MapContext} from "./Map";
import {LocationOn} from "@mui/icons-material";

interface RowProps {
    row: number[];
    rowIdx: number;
}

export const Row = ({row, rowIdx,}: RowProps) => {
    const cellSize = "min(7vw, 7vh)";
    const context = useContext(MapContext);

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
                    const key = cellKey(rowIdx, colIdx);
                    // const character = context!.charPositions[key];
                    const property = context!.tileProperties[piece];
                    //const move = context!.move?.pathHashSet[piece];

                    return (
                        <Paper key={key} sx={{
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
                            {context!.drawExtra(piece, rowIdx, colIdx)}
                            {/*{character && <Avatar src={character.avatarUrL}/>}*/}
                            {/*{!character && move && <LocationOn/>}*/}
                        </Paper>
                    );
                })
            }
        </Stack>
    );
}

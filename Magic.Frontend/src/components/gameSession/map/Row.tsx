import {Stack} from "@mui/material";
import Paper from "@mui/material/Paper";
import {baseProxy} from "../../../env";
import React, {useContext} from "react";
import {cellKey} from "./map.utils";
import {MapContext, ShadowTypeEnum} from "./Map";

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
                    const property = context!.tileProperties[piece];
                    const shadow = context!.shadows[key];
                    let color = '#FFFFFF';
                    switch (shadow) {
                        case ShadowTypeEnum.Green:
                            color = '#13E14AE5';
                            break;
                        case ShadowTypeEnum.Red:
                            color = '#E11313E5';
                            break;
                        case ShadowTypeEnum.White:
                            color = '#FFFFFF';
                            break;
                        case ShadowTypeEnum.Blue:
                            color = '#00308F';
                            break;
                    }

                    //const move = context!.move?.pathHashSet[piece];

                    return (
                        <Paper
                            data-x={colIdx}
                            data-y={rowIdx}
                            onClick={context?.onClick}
                            elevation={0}
                            key={key}
                            id={key}
                            sx={{
                                flex: "1 1 min(9vw, 9vh)",
                                width: '50px',
                                maxWidth: '50px',
                                height: '50px',
                                display: "flex",
                                alignItems: "center",
                                justifyContent: "center",
                                backgroundSize: 'cover',
                                backgroundRepeat: 'no-repeat',
                                backgroundPosition: "center",
                                boxShadow: shadow && ('inset  0px 0px 10px 0px ' + color),
                                backgroundImage: property && `url(${baseProxy}${property.image})`,
                                mr: '1px',
                                borderRadius: 0,
                                fontSize: cellSize,
                            }}
                        />
                    );
                })
            }
        </Stack>
    );
}

import React, {createContext, useEffect} from "react";
import {useDispatch, useSelector} from "react-redux";
import {AppDispatch, RootState} from "../../../redux/redux";
import {useGetTilePropertiesQuery} from "../../../redux/api/mapApi";
import {TileProperty, TilePropertyCollisionTypeEnum} from "../../../models/response/tileProperty";
import {Avatar} from "@mui/material";
import {addMove, backTrackLastMove} from "../../../redux/slices/moveSlice";
import Paper from "@mui/material/Paper";
import {Row} from "./Row";
import {cellKey} from "./map.utils";
import {baseProxy} from "../../../env";

export interface MapContextType {
    tileProperties: Record<string, TileProperty>;
    shadows: Record<string, ShadowTypeEnum>;
    onClick: (e: React.MouseEvent<HTMLDivElement>) => void;
}

export const MapContext = createContext<MapContextType | null>(null);

export enum ShadowTypeEnum {
    Green = 1,
    Red,
    White,
    Blue,
}

export function Map() {
    const gameSessionInfo = useSelector((state: RootState) => state.gameSession.gameSessionInfo);
    const moveState = useSelector((state: RootState) => state.move);
    const dispatch = useDispatch<AppDispatch>();
    const {data: tileProperties} = useGetTilePropertiesQuery();

    if (!gameSessionInfo?.map || !gameSessionInfo.characters || !tileProperties) {
        return (
            <React.Fragment/>
        );
    }

    const shadows: Record<string, ShadowTypeEnum> = {};

    const movingCharacter = gameSessionInfo.characters
        .find(c => c.id === moveState.movingCharacterId)!
    let movingCharacterPosition: { x: number, y: number } | null = null;
    if (moveState.movingCharacterId) {
        let baseX;
        let baseY;
        if (moveState.path.length > 0) {
            const move = moveState.path[moveState.path.length - 1];
            baseX = move.x;
            baseY = move.y;
        } else {
            baseX = movingCharacter.positionX!;
            baseY = movingCharacter.positionY!;
        }
        movingCharacterPosition = {x: baseX, y: baseY};
    }

    if (movingCharacterPosition && moveState.path.length < movingCharacter.speed) {
        for (let y = -1; y <= 1; y++) {
            for (let x = -1; x <= 1; x++) {
                const tileId = gameSessionInfo?.map.tiles[movingCharacterPosition.y + y][movingCharacterPosition.x + x];
                const property = tileProperties[tileId];
                if ((x === 0 && y === 0) || property.collisionType !== TilePropertyCollisionTypeEnum.None) continue;
                shadows[cellKey(movingCharacterPosition.y + y, movingCharacterPosition.x + x)] = ShadowTypeEnum.Green;
            }
        }
    }

    moveState.path
        .slice(0, moveState.path.length - 1)
        .reduce((pv, cv) => {
            pv[cellKey(cv.y, cv.x)] = ShadowTypeEnum.White;
            return pv;
        }, shadows);

    return (
        <MapContext.Provider
            value={{
                tileProperties,
                shadows,
                onClick: _addMove
            }}>
            <Paper
                onContextMenu={backTrackMove}
                id={'Board'}
                elevation={4}
                sx={{
                    maxWidth: '100%',
                    position: 'relative',
                    flexDirection: "column",
                }}>
                {
                    gameSessionInfo.map.tiles
                        .map((row, rowIdx) => {
                            return (
                                <Row key={rowIdx} row={row} rowIdx={rowIdx}/>
                            );
                        })
                }
                {
                    gameSessionInfo
                        .characters
                        .filter(c => Number.isFinite(c.positionX))
                        .map(c => {
                            const {x, y} = convertToRealPosition(c.positionX!, c.positionY!);
                            return <Avatar
                                key={c.id}
                                src={baseProxy + c!.avatarUrL}
                                sx={{
                                    position: 'absolute',
                                    top: y,
                                    left: x,
                                    opacity: movingCharacter?.id === c.id && moveState.path.length > 0 ? 0.5 : 1,
                                    transition: 'all 400ms ease'
                                }}
                            />;
                        })
                }
                {
                    movingCharacter &&
                    movingCharacterPosition &&
                    moveState.path.length > 0 &&
                    renderMovingCharacterStartPosition()
                }
            </Paper>
        </MapContext.Provider>
    );

    function backTrackMove(e: React.MouseEvent<HTMLElement>) {
        if (moveState.path.length > 0) {
            e.preventDefault();
            dispatch(backTrackLastMove());
        }
    }

    function renderMovingCharacterStartPosition() {
        const {x, y} = convertToRealPosition(movingCharacterPosition!.x, movingCharacterPosition!.y);
        return (
            <Avatar
                key={movingCharacter.id + '_temp'}
                src={baseProxy + movingCharacter.avatarUrL}
                sx={{
                    position: 'absolute',
                    top: y,
                    left: x,
                    transition: 'all 400ms ease'
                }}
            />
        );
    }

    function convertToRealPosition(x: number, y: number) {
        return {x: x * 50 + 5 + x, y: y * 50 + 5 + y};
    }

    function _addMove(e: React.MouseEvent<HTMLDivElement>) {
        const x = parseInt(e.currentTarget.dataset.x!);
        const y = parseInt(e.currentTarget.dataset.y!);
        const tileId = gameSessionInfo?.map.tiles[y][x];
        const property = tileProperties![tileId!];

        if (movingCharacterPosition &&
            property.collisionType === TilePropertyCollisionTypeEnum.None &&
            moveState.path.length < movingCharacter!.speed &&
            Math.abs(movingCharacterPosition.x - x) <= 1 &&
            Math.abs(movingCharacterPosition.y - y) <= 1
        )

            dispatch(addMove({x: x, y: y}));
    }
}


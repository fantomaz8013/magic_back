import React, {createContext, ReactNode} from "react";
import {useDispatch, useSelector} from "react-redux";
import {AppDispatch, RootState} from "../../../redux/redux";
import {Board} from "./Board";
import {cellKey} from "./map.utils";
import {useGetTilePropertiesQuery} from "../../../redux/api/mapApi";
import {TileProperty, TilePropertyCollisionTypeEnum} from "../../../models/response/tileProperty";
import {GameSessionCharacter} from "../../../models/websocket/gameStartedInfo";
import {Avatar} from "@mui/material";
import {AddLocationAlt, LocationOn} from "@mui/icons-material";
import {addMove} from "../../../redux/slices/moveSlice";
import Button from "@mui/material/Button";

export interface MapContextType {
    tileProperties: TileProperty[];
    drawExtra: (value: number, y: number, x: number) => ReactNode;
}

export const MapContext = createContext<MapContextType | null>(null);
export type CharPositions = Record<string, GameSessionCharacter>


export function Map() {
    const gameSessionInfo = useSelector((state: RootState) => state.gameSession.gameSessionInfo);
    const move = useSelector((state: RootState) => state.move);
    const dispatch = useDispatch<AppDispatch>();
    const {data: tileProperties} = useGetTilePropertiesQuery();

    if (!gameSessionInfo?.map || !gameSessionInfo.characters || !tileProperties?.data) {
        return (
            <React.Fragment/>
        );
    }

    let movingCharacter: GameSessionCharacter | undefined;
    const charPositions = gameSessionInfo.characters
        .reduce((pv, cv) => {
            if (cv.ownerId === move.movingUserId) {
                movingCharacter = cv;
            }
            if (Number.isFinite(cv.positionY) && Number.isFinite(cv.positionX)) {
                pv[cellKey(cv.positionY!, cv.positionX!)] = cv;
            }
            return pv;
        }, {} as CharPositions)

    const drawExtra = (value: number, y: number, x: number) => {
        const key = cellKey(y, x);
        const character = charPositions[key];
        const movingCharacterLocation = move.path[move.path.length - 1] || (movingCharacter && {
            x: movingCharacter.positionX,
            y: movingCharacter.positionY
        });

        if (movingCharacterLocation && movingCharacterLocation.x === x && movingCharacterLocation.y === y)
            return <Avatar src={movingCharacter!.avatarUrL}/>;

        if (character)
            return <Avatar
                src={character.avatarUrL}
                sx={{opacity: character.ownerId === move.movingUserId ? 0.5 : 1}}
            />;

        const containsMove = move.pathHashSet[key];
        if (containsMove)
            return <LocationOn/>;

        if (tileProperties!.data![value].collisionType === TilePropertyCollisionTypeEnum.None &&
            movingCharacter && movingCharacter.speed >= move.path.length && movingCharacterLocation &&
            Math.abs(movingCharacterLocation.x! - x) <= 1 &&
            Math.abs(movingCharacterLocation.y! - y) <= 1) {

            return <Button data-x={x} data-y={y} onClick={_addMove}> <AddLocationAlt/></Button>
        }

        return undefined;
    }

    return (
        <MapContext.Provider
            value={{
                tileProperties: tileProperties.data,
                drawExtra,
            }}>
            <Board board={gameSessionInfo.map.tiles}/>
        </MapContext.Provider>
    );

    function _addMove(e: React.MouseEvent<HTMLButtonElement>) {
        const x = e.currentTarget.dataset.x;
        const y = e.currentTarget.dataset.y;
        dispatch(addMove({x: parseInt(x!), y: parseInt(y!)}))
    }
}


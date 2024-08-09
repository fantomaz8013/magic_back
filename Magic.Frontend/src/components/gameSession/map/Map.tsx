import React from "react";
import {useSelector} from "react-redux";
import {RootState} from "../../../redux/redux";
import {Board, CharPositions} from "./Board";
import {cellKey} from "./map.utils";

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


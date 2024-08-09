import React from "react";
import {useGetCurrentUserQuery} from "../../redux/api/userApi";
import {useSelector} from "react-redux";
import {RootState} from "../../redux/redux";
import {Navigate} from "react-router-dom";
import paths from "../../consts/paths";

export function KickHandler() {
    const {data: currentUser} = useGetCurrentUserQuery();
    const playerInfos = useSelector((state: RootState) => state.gameSession.playerInfos);

    if (playerInfos && currentUser && currentUser.data) {
        const isOnline = playerInfos[currentUser.data.id]?.isOnline;
        if (isOnline !== undefined && !isOnline) {
            console.warn('You\'ve been kicked');
            return <Navigate to={paths.home}/>
        }
    }

    return (
        <React.Fragment/>
    )
}

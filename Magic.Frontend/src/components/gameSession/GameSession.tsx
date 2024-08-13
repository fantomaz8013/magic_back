import React, {useEffect, useState} from "react";
import {useGameSessionWS} from "../../webSocket/webSocket";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Chat from "./chat/Chat";
import CharactersList from "./charactersList/CharactersList";
import CharacterLeftMenu from "./charactersList/CharacterLeftMenu";
import {useNavigate, useParams} from "react-router-dom";
import {
    LinearProgress,
} from "@mui/material";
import {GameSessionStatusTypeEnum} from "../../models/websocket/gameSessionStatus";
import {useSelector} from "react-redux";
import {RootState} from "../../redux/redux";
import {GameSessionCharacter, GameSessionInfo} from "../../models/websocket/gameStartedInfo";
import {MasterUtils} from "./masterUtils/MasterUtils";
import {Map} from "./map/Map";
import CssBaseline from "@mui/material/CssBaseline";
import {UserUtils} from "./playerUtils/userUtils";
import {EventSnackbar} from "./eventSnackbar/EventSnackbar";
import {KickHandler} from "./KickHandler";
import paths from "../../consts/paths";
import {PlayerActions} from "./playerActions/PlayerActions";
import className from "./gameSession.style";


export interface PlayerInfo {
    id: string;
    login: string;
    isMaster: boolean | null;
    lockedCharacterId: string | null;
    isOnline: boolean;
}


export default function GameSession() {
    const {state, joinGameSession, leaveGameSession} = useGameSessionWS();
    const {gameSessionId} = useParams();

    const [ref, setRef] = useState<HTMLElement | null>(null);

    const gameSessionInfo = useSelector((state: RootState) => state.gameSession.gameSessionInfo);

    useEffect(() => {
        if (gameSessionId && state === "Connected")
            joinGameSession(gameSessionId)
                .catch(err => {
                    console.warn(err);
                });
        return () => {
            leaveGameSession()
                .catch(err => console.warn(err));
        }
    }, [gameSessionId, state]);

    return (
        <Box
            sx={className.container}>
            <CssBaseline/>
            <Box sx={className.block}>
                {renderGameSessionPage()}
                <MasterUtils anchorEl={ref} onClose={closeMenu}/>
                <UserUtils/>
                <EventSnackbar/>
                <KickHandler/>
            </Box>
        </Box>
    )

    //region Render functions

    function renderGameSessionPage() {
        if (!gameSessionInfo)
            return (
                <LinearProgress color="inherit"/>
            )

        switch (gameSessionInfo.gameSessionStatus) {
            case GameSessionStatusTypeEnum.WaitingForStart:
                return renderWaitingForStartPage();
            case GameSessionStatusTypeEnum.InGame:
                //in game characters will be available
                return renderInGamePage(gameSessionInfo.characters!);
            case GameSessionStatusTypeEnum.Finished:
                break;
        }
    }

    function renderWaitingForStartPage() {
        return (
            <>
                <CharactersList/>
                <Box sx={className.chat}>
                    <Chat/>
                </Box>
            </>
        );
    }

    function renderInGamePage(characters: NonNullable<GameSessionInfo['characters']>) {
        return (
            <>
                <Map/>
                {/*<Chat/>*/}
                {renderUI(characters)}
            </>
        );
    }

    function renderUI(characters: NonNullable<GameSessionInfo['characters']>) {
        return (
            <>
                <Box sx={{display: 'flex', flexDirection: 'column', position: 'absolute', left: 0}}>
                    {characters.map((c: GameSessionCharacter) =>
                        <CharacterLeftMenu
                            key={c.id}
                            onClick={onClick}
                            character={{...c, name: `${c.name} (${c.ownerId})`}}
                        />
                    )}
                </Box>
                <PlayerActions/>
            </>
        );
    }

    //endregion

    //region Click handlers

    function closeMenu() {
        setRef(null);
    }

    function onClick(e: React.MouseEvent<HTMLDivElement>) {
        setRef(e.currentTarget);
    }

    //endregion
}
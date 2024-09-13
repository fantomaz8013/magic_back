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
import className from "./gameSession.style";
import Paper from "@mui/material/Paper";
import Avatar from "@mui/material/Avatar";
import {baseProxy} from "../../env";
import {useGetCurrentUserQuery} from "../../redux/api/userApi";
import HourglassEmptyIcon from '@mui/icons-material/HourglassEmpty';
import {PlayerActions} from "./playerActions/PlayerActions";


export interface PlayerInfo {
    id: string;
    login: string;
    isMaster: boolean | null;
    lockedCharacterId: string | null;
    isOnline: boolean;
}


export default function GameSession() {
    const {state, joinGameSession, leaveGameSession} = useGameSessionWS();
    const {data: currentUser} = useGetCurrentUserQuery();
    const {gameSessionId} = useParams();

    const [ref, setRef] = useState<HTMLElement | null>(null);

    const gameSessionInfo = useSelector((state: RootState) => state.gameSession.gameSessionInfo);
    const playerInfos = useSelector((state: RootState) => state.gameSession.playerInfos);
    const gameMasterUserId = Object.values(playerInfos ?? {}).find(i => i.isMaster)?.id;

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
            <Box sx={className.page}>
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
            <Box style={{
                display: 'flex',
                width: '100%'
            }}>
                <Box
                    style={{
                        position: 'relative',
                        display: 'flex',
                        width: '100%',
                        justifyContent: 'center',
                        alignItems: 'center',
                    }}>
                    <Map/>
                    {renderUI(characters)}
                </Box>

                <Box sx={{...className.chat, justifySelf: 'end'}}>
                    <Chat/>
                </Box>
            </Box>
        );
    }

    function renderUI(characters: NonNullable<GameSessionInfo['characters']>) {
        return (
            <>
                {renderCharactersList(characters)}
                {renderCharacterControlPanel(characters)}
                {/*<PlayerActions/>*/}
            </>
        );
    }

    function renderCharactersList(characters: NonNullable<GameSessionInfo['characters']>) {
        return (
            <Paper elevation={3} style={{
                position: 'absolute', // Make it overlap the map
                top: '10px',
                left: '10px',
                zIndex: 10, // Higher z-index to be above the map
                overflowY: 'auto',
                display: "flex",
                flexDirection: 'column',
                gap: '10px',
                background: 'transparent'
            }}>
                {characters
                    .filter(c => c.ownerId !== gameMasterUserId)
                    .map((c: GameSessionCharacter) =>
                        <CharacterLeftMenu
                            key={c.id}
                            onClick={onClick}
                            character={{...c, name: `${c.name} (${c.ownerId})`}}
                        />
                    )}
            </Paper>
        )
    }

    function renderCharacterControlPanel(characters: NonNullable<GameSessionInfo['characters']>) {
        const userCharacter = characters.find(c => c.ownerId === currentUser!.data!.id);

        return (
            <Box style={{
                position: 'absolute', // Make it overlap the map
                bottom: '0',
                width: '500px',
                height: '100px',
                left: 'calc(50% - 380px + 250px)',
                zIndex: 10, // Higher z-index to be above the map
            }}>
                <Avatar
                    src={baseProxy + userCharacter!.avatarUrL}
                    sx={{
                        position: 'relative',
                        zIndex: 10,
                        float: 'left',
                        left: '-50px',
                        width: '100px',
                        height: '100px'
                    }}
                />
                <Paper style={{
                    position: 'relative',
                    width: '412px',
                    height: '64px',
                    bottom: '64px',
                    overflowY: 'auto',
                }}>
                    <PlayerActions/>
                </Paper>
                <Avatar
                    sx={{
                        position: 'relative',
                        zIndex: 10,
                        bottom: "164px",
                        float: 'right',
                        right: '50px',
                        width: '100px',
                        height: '100px',
                        background: '#23211B',
                        color:'white',
                        boxShadow: '0px 0px 40px 10px #CA5C40 inset',
                    }}>
                    <HourglassEmptyIcon/>
                </Avatar>
            </Box>
        )
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

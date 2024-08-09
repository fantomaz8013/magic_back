import React, {useEffect, useState} from "react";
import {useGameSessionWS} from "../../webSocket/webSocket";
import Box from "@mui/material/Box";
import Grid from "@mui/material/Grid";
import Dice from "./dice/Dice";
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
    const navigate = useNavigate();

    const [ref, setRef] = useState<HTMLElement | null>(null);

    const gameSessionInfo = useSelector((state: RootState) => state.gameSession.gameSessionInfo);

    useEffect(() => {
        if (gameSessionId && state === "Connected")
            joinGameSession(gameSessionId)
                .catch(err => {
                    console.warn(err);
                    navigate(paths.home);
                });
        return () => {
            if (state === "Connected")
                leaveGameSession()
                    .catch(err => console.warn(err));
        }
    }, [gameSessionId, state]);

    return (
        <Box
            sx={{
                backgroundSize: 'cover',
                backgroundRepeat: 'no-repeat',
                backgroundPosition: 'center',
                backgroundImage: `url(https://www.wargamer.com/wp-content/sites/wargamer/2021/09/dnd-backgrounds-5e-hermit.jpg)`,
            }}>
            <CssBaseline/>
            <Box sx={{
                marginTop: 8,
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
            }}>
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
                <Box sx={{
                    width: '95%',
                    display: 'flex',
                    flexDirection: ' row-reverse',
                }}>
                    <Chat/>
                </Box>
            </>
        );
    }

    function renderInGamePage(characters: NonNullable<GameSessionInfo['characters']>) {
        return (
            <Grid container spacing={2}>
                <Grid item sx={{zIndex: 1}} xs={1}>
                    <Box sx={{display: 'flex', flexDirection: 'column'}}>
                        {characters.map((c: GameSessionCharacter) =>
                            <CharacterLeftMenu
                                key={c.id}
                                onClick={onClick}
                                character={{...c, name: `${c.name} (${c.ownerId})`}}
                            />
                        )}
                    </Box>
                </Grid>
                <Grid item sx={{zIndex: 1}} xs={11}>
                    <Box sx={{
                        height: '85vh',
                        display: 'flex',
                        alignItems: 'end'
                    }}>
                        <Grid container>
                            <Grid item xs={1} sx={{
                                display: 'flex',
                                alignItems: 'end'
                            }}>
                                <Dice/>
                            </Grid>
                            <Grid item xs={11}>
                                <Box sx={{
                                    width: '95%',
                                    display: 'flex',
                                    flexDirection: ' row-reverse',
                                }}>
                                    <Chat/>
                                </Box>
                            </Grid>
                        </Grid>
                    </Box>
                </Grid>
                <Map/>
            </Grid>
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

import {useLocation, useNavigate} from "react-router-dom";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import AccessibleForwardIcon from "@mui/icons-material/AccessibleForward";
import Typography from "@mui/material/Typography";
import {Card, CardActions, CardContent} from "@mui/material";
import Button from "@mui/material/Button";
import paths from "../../consts/paths";
import * as React from "react";
import {useEffect} from "react";
import {
    useCreateMutation,
    useDeleteRequestMutation,
    useEnterMutation,
    useListQuery
} from "../../redux/toolkit/api/gameSessionApi";
import {GameSessionResponse} from "../../models/response/gameSessionResponse";
import {useGetCurrentUserQuery} from "../../redux/toolkit/api/userApi";
import {useDispatch} from "react-redux";
import {AppDispatch} from "../../redux/redux";
import {clearGameSessionData} from "../../redux/toolkit/slices/gameSessionSlice";

export default function HomePage() {
    const navigate = useNavigate();
    const location = useLocation();
    const {data: list} = useListQuery();
    const [enterSession] = useEnterMutation();
    const [createSession] = useCreateMutation();
    const [deleteRequest] = useDeleteRequestMutation();
    const {data: currentUser} = useGetCurrentUserQuery();
    const dispatch = useDispatch<AppDispatch>();

    useEffect(() => {
        if (location.pathname !== paths.home) {
            navigate(paths.home, {replace: true});
        }
    }, [location, navigate]);

    useEffect(() => {
        dispatch(clearGameSessionData());
    }, [])

    return (
        <Grid container spacing={4}>
            <Grid item xs={12}>
                <Box
                    sx={{
                        marginTop: 8,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                    }}
                >
                    <Avatar sx={{m: 1, bgcolor: 'secondary.main'}}>
                        <AccessibleForwardIcon/>
                    </Avatar>
                    <Typography component="h1" variant="h5">
                        Campaigns
                    </Typography>
                </Box>
            </Grid>
            <Grid item xs={12}>
                <Card>
                    <CardContent>
                        <Typography variant="h5" component="div">
                            Start your first game!
                        </Typography>
                        <Typography sx={{mb: 1.5}} color="text.secondary">
                            short campaign designed for 4 players (30 mins.)
                        </Typography>
                        <Typography variant="body2">
                            Start an adventure in DND stylistics.
                            <br/>
                            Explore secrets and enjoy battle mechanics.
                            <br/>
                            Save Halsin from prisoning in Crimson Castle.
                        </Typography>
                    </CardContent>
                    <CardActions>
                        <Button onClick={onCreateClick} size="small">
                            Save Halsin!
                        </Button>
                    </CardActions>
                </Card>
            </Grid>

            {list?.data?.map((r: GameSessionResponse) => {
                return (
                    <Grid key={r.id} item xs={12}>
                        <Card>
                            <CardContent>
                                <Typography variant="h5" component="div">
                                    {r.title}
                                </Typography>
                                <Typography sx={{mb: 1.5}} color="text.secondary">
                                    For {r.maxUserCount} player(s)
                                </Typography>
                                <Typography variant="body2">
                                    {r.description}
                                </Typography>
                            </CardContent>
                            <CardActions>
                                {
                                    currentUser?.data?.id === r.creatorUserId
                                        ? <>
                                            <Button id={r.id} onClick={onJoinClick} size="small">
                                                JOIN
                                            </Button>
                                            <Button id={r.id} onClick={onDeleteClick} size="small">
                                                DELETE
                                            </Button>
                                        </>
                                        : <Button id={r.id} onClick={onAskToJoinClick} size="small">
                                            ASK TO JOIN
                                        </Button>
                                }
                            </CardActions>
                        </Card>
                    </Grid>
                )
            })}
        </Grid>
    );

    function onAskToJoinClick(e: React.MouseEvent<HTMLButtonElement>) {
        const gameSessionId = e.currentTarget.id;
        enterSession({gameSessionId})
            .then(r => {
                r.data?.data && navigate(`${paths.game}/${gameSessionId}`);
            })
    }

    function onJoinClick(e: React.MouseEvent<HTMLButtonElement>) {
        const gameSessionId = e.currentTarget.id;
        navigate(`${paths.game}/${gameSessionId}`);
    }

    function onDeleteClick(e: React.MouseEvent<HTMLButtonElement>) {
        const gameSessionId = e.currentTarget.id;
        deleteRequest({gameSessionId});
    }

    function onCreateClick() {
        createSession({
            startDt: new Date().toJSON(),
            title: 'Save Halsin!',
            description: 'default game',
            maxUserCount: 4
        }).then(r => {
            if (r.data?.isSuccess)
                navigate(`${paths.game}/${r.data!.data!.id}`);
        })
    }
}

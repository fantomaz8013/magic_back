import {useNavigate} from "react-router-dom";
import Grid from "@mui/material/Grid";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import AccessibleForwardIcon from "@mui/icons-material/AccessibleForward";
import Typography from "@mui/material/Typography";
import {Card, CardActions, CardContent} from "@mui/material";
import Button from "@mui/material/Button";
import paths from "../../consts/paths";
import * as React from "react";

export default function HomePage() {
    const navigate = useNavigate();
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
                        <Button onClick={onStartClick} size="small">Save Halsin!</Button>
                    </CardActions>
                </Card>
            </Grid>
        </Grid>
    );

    function onStartClick() {
        navigate(paths.chat)
    }
}

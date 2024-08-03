import Container from "@mui/material/Container";
import CssBaseline from "@mui/material/CssBaseline";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import * as React from "react";
import {useGetCurrentUserQuery} from "../../redux/toolkit/api/userApi";
import {baseProxy} from "../../env";
import {CircularProgress, Paper} from "@mui/material";

export default function Profile() {
    const {isLoading, data} = useGetCurrentUserQuery();
    if (!data || isLoading)
        return (<Container component="main" maxWidth="xs">
                <CssBaseline/>
                <Paper
                    sx={{
                        position: 'relative',
                        backgroundColor: 'grey.800',
                        color: '#fff',
                        mb: 4,
                        backgroundSize: 'cover',
                        backgroundRepeat: 'no-repeat',
                        backgroundPosition: 'center',
                        backgroundImage: `url(${baseProxy}storage/character/avatar/1.png)`,
                    }}
                >
                    <Box
                        sx={{
                            marginTop: 8,
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'center',
                        }}
                    >
                        <Typography component="h1" variant="h5">
                            Profile
                        </Typography>
                        <CircularProgress/>
                    </Box>
                </Paper>
            </Container>
        );

    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline/>
            <Typography component="h1" variant="h3" color="inherit">
                Profile {data.login}
            </Typography>
            <Paper
                sx={{
                    position: 'relative',
                    backgroundColor: 'grey.800',
                    color: '#fff',
                    mb: 4,
                    backgroundSize: 'cover',
                    backgroundRepeat: 'no-repeat',
                    backgroundPosition: 'center',
                    backgroundImage: `url(${baseProxy}storage/character/avatar/1.png)`,
                }}
            >
                <Box
                    sx={{
                        marginTop: 8,
                        display: 'flex',
                        flexDirection: 'column',
                        alignItems: 'center',
                    }}
                >
                    <Typography variant="subtitle1" color="inherit" paragraph>
                        LOGIN: {data.login} <td/>
                        NAME: {data.name || "EMPTY"}<td/>
                        EMAIL: {data.email || "EMPTY"}<td/>
                        PHONE NUMBER: {data.phoneNumber || "EMPTY"}
                    </Typography>
                </Box>
            </Paper>
        </Container>
    );
}

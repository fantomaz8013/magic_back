import Container from "@mui/material/Container";
import CssBaseline from "@mui/material/CssBaseline";
import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import * as React from "react";
import {useGetCurrentUserQuery} from "../../redux/toolkit/api/userApi";
import {baseProxy} from "../../env";
import Paper from "@mui/material/Paper";
import CircularProgress from "@mui/material/CircularProgress";
import Button from "@mui/material/Button";
import ProfileField from "../common/FieldWithValidation";
import UserUpdateRequest from "../../models/requests/userUpdateRequest";

export default function Profile() {
    const {isLoading, data: currentUser} = useGetCurrentUserQuery();

    if (!currentUser || isLoading)
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
                        <CircularProgress/>
                    </Box>
                </Paper>
            </Container>
        );

    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline/>
            <Typography component="h1" variant="h3" color="inherit">
                Profile {currentUser.data.login}
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
                <Box component="form" noValidate onSubmit={handleSubmit} sx={{mt: 1}}>
                    <ProfileField name={'Name'} type={'text'} defaultValue={currentUser.data.name}/>
                    <ProfileField name={'Email'} type={'email'} defaultValue={currentUser.data.email}/>
                    <ProfileField name={'PhoneNumber'} type={'tel'} defaultValue={currentUser.data.phoneNumber}/>
                    <Button
                        type="submit"
                        fullWidth
                        variant="contained"
                        sx={{mt: 3, mb: 2}}
                    >
                        Save changes
                    </Button>
                </Box>
            </Paper>
        </Container>
    );

    function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const data = new FormData(event.currentTarget);
        const name = (data.get('Name') || '').toString();
        const email = (data.get('Email') || '').toString();
        const phoneNumber = (data.get('PhoneNumber') || '').toString();
        const userUpdateRequest: UserUpdateRequest = {name, email, phoneNumber}
    }
}

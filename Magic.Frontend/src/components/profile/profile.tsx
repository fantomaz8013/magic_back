import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import * as React from "react";
import {useGetCurrentUserQuery} from "../../redux/toolkit/api/userApi";
import {baseProxy} from "../../env";
import Paper from "@mui/material/Paper";
import Button from "@mui/material/Button";
import ProfileField from "../common/FieldWithValidation";
import UserUpdateRequest from "../../models/request/userUpdateRequest";
import {LinearProgress} from "@mui/material";

export default function Profile() {
    const {data: currentUser, isLoading} = useGetCurrentUserQuery();

    return (
        <>
            {isLoading && <LinearProgress color="inherit"/>}
            <Box sx={{
                marginTop: 8,
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
            }}>
                <Typography component="h1" variant="h3" color="inherit">
                    Profile {currentUser?.data?.login || 'loading'}
                </Typography>
                <Paper
                    sx={{
                        backgroundSize: 'cover',
                        backgroundRepeat: 'no-repeat',
                        backgroundPosition: 'center',
                        backgroundImage: `url(${baseProxy}storage/character/avatar/1.png)`,
                    }}
                >
                    <Box p={2} component="form" noValidate onSubmit={handleSubmit}>
                        <ProfileField name={'Name'} type={'text'} defaultValue={currentUser?.data?.name || ''}/>
                        <ProfileField name={'Email'} type={'email'} defaultValue={currentUser?.data?.email || ''}/>
                        <ProfileField name={'PhoneNumber'} type={'tel'}
                                      defaultValue={currentUser?.data?.phoneNumber || ''}/>
                    </Box>
                </Paper>
                <Button
                    type="submit"
                    fullWidth
                    variant="contained"
                    sx={{mt: 3, mb: 2}}
                >
                    Save changes
                </Button>
            </Box>
        </>
    );

    function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();

        const data = new FormData(event.currentTarget);
        const name = (data.get('Name') || '').toString();
        const email = (data.get('Email') || '').toString();
        const phoneNumber = (data.get('PhoneNumber') || '').toString();
        const userUpdateRequest: UserUpdateRequest = {name, email, phoneNumber}
        console.log(userUpdateRequest);
    }
}

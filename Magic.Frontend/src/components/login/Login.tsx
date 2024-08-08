import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import {useLocation, useNavigate} from "react-router-dom";
import {useDispatch, useSelector} from "react-redux";
import {useEffect, useState} from "react";
import {getToken as loginRequest, resetError} from "../../redux/toolkit/slices/tokenSlice";
import paths from "../../consts/paths";
import {AppDispatch, RootState} from "../../redux/redux";
import {Alert} from "@mui/material";

export default function Login() {
    const navigate = useNavigate();
    const location = useLocation();
    const dispatch = useDispatch<AppDispatch>()
    const [login, setLogin] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const error = useSelector((state: RootState) => state.auth.error);

    useEffect(() => {
        if (error)
            dispatch(resetError());
    }, [])

    return (
        <Box
            sx={{
                marginTop: 8,
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
            }}
        >
            <Avatar sx={{m: 1, bgcolor: 'secondary.main'}}>
                <LockOutlinedIcon/>
            </Avatar>
            <Typography component="h1" variant="h5">
                Login
            </Typography>
            <Box component="form" onSubmit={handleSubmit} noValidate sx={{mt: 1}}>
                <TextField
                    margin="normal"
                    required
                    fullWidth
                    id="login"
                    label="Login"
                    name="login"
                    autoComplete="login"
                    autoFocus
                    value={login}
                    onChange={onLoginChange}
                />
                <TextField
                    margin="normal"
                    required
                    fullWidth
                    name="password"
                    label="Password"
                    type="password"
                    id="password"
                    value={password}
                    autoComplete="current-password"
                    onChange={onPasswordChange}
                />
                <Button
                    type="submit"
                    fullWidth
                    variant="contained"
                    sx={{mt: 3, mb: 2}}
                >
                    Login
                </Button>
                {error && <Alert severity="error" sx={{mt: 1, mb: 2}}>{error}</Alert>}
                <Grid container justifyContent="flex-end">
                    <Grid item>
                        <Link href={paths.register} onClick={onRegisterClick} variant="body2">
                            {"Don't have an account? Register"}
                        </Link>
                    </Grid>
                </Grid>
            </Box>
        </Box>
    );

    function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        dispatch(loginRequest({login: login, password: password}))
    }

    function onRegisterClick(e: React.MouseEvent) {
        e.preventDefault();
        navigate(paths.register, location.state?.from?.pathname && {state: {from: location.state.from.pathname}});
    }

    function onLoginChange(e: React.ChangeEvent<HTMLTextAreaElement>) {
        const value = e.target.value;
        setLogin(value);
    }

    function onPasswordChange(e: React.ChangeEvent<HTMLTextAreaElement>) {
        const value = e.target.value;
        setPassword(value)
    }
}

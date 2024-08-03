import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';
import Copyright from "../copyright";
import {useNavigate} from "react-router-dom";
import {useDispatch} from "react-redux";
import {useState} from "react";
import {login as loginRequest} from "../../redux/toolkit/slices/authSlice";
import paths from "../../consts/paths";
import {AppDispatch} from "../../redux";

export default function Login() {
    const navigate = useNavigate();
    const dispatch = useDispatch<AppDispatch>()
    const [login, setLogin] = useState<string | null>(null);
    const [password, setPassword] = useState<string | null>(null);

    return (
        <Container component="main" maxWidth="xs">
            <CssBaseline/>
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
                    Sign in
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
                    <Grid container justifyContent="flex-end">
                        <Grid item>
                            <Link href={paths.register} onClick={onRegisterClick} variant="body2">
                                {"Don't have an account? Sign Up"}
                            </Link>
                        </Grid>
                    </Grid>
                </Box>
            </Box>
            <Copyright sx={{mt: 8, mb: 4}}/>
        </Container>
    );

    function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        event.preventDefault();
        dispatch(loginRequest({login: login!, password: password!}));
        navigate(paths.home);
    }

    function onRegisterClick(e: React.MouseEvent) {
        e.preventDefault();
        navigate(paths.register)
    }

    function onLoginChange(e: React.ChangeEvent<HTMLTextAreaElement>) {
        const value = e.target.value;
        setLogin(value);
    }

    function onPasswordChange(e: React.ChangeEvent<HTMLTextAreaElement>) {
        const value = e.target.value;
        setPassword(e.target.value)
    }
}

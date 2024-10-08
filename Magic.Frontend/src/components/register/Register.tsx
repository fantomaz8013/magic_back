import * as React from 'react';
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import Typography from '@mui/material/Typography';
import {useNavigate} from "react-router-dom";
import paths from "../../consts/paths";
import {useEffect, useState} from "react";
import {ValidationResult} from "../validation";
import {validateLogin, validatePassword} from "../validation/userValidation";
import {register, resetError} from "../../redux/slices/tokenSlice";
import {useDispatch, useSelector} from "react-redux";
import {AppDispatch, RootState} from "../../redux/redux";
import {Alert} from "@mui/material";


export default function Register() {
    const navigate = useNavigate();
    const dispatch = useDispatch<AppDispatch>()
    const error = useSelector((state: RootState) => state.auth.error);
    const [login, setLogin] = useState<string | null>(null);
    const [loginError, setLoginError] = useState<ValidationResult | null>(null);

    const [password, setPassword] = useState<string | null>(null);
    const [passwordError, setPasswordError] = useState<ValidationResult | null>(null);

    const [runValidation, setRunValidation] = useState<boolean>(false);

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
                Register
            </Typography>
            <Box component="form" noValidate onSubmit={handleSubmit} sx={{mt: 3}}>
                <Grid container spacing={2}>
                    <Grid item xs={12}>
                        <TextField
                            required
                            fullWidth
                            id="login"
                            label="Login"
                            name="login"
                            autoComplete="login"
                            onChange={onLoginChange}
                            {...loginError}
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <TextField
                            required
                            fullWidth
                            name="password"
                            label="Password"
                            type="password"
                            id="password"
                            autoComplete="new-password"
                            onChange={onPasswordChange}
                            {...passwordError}
                        />
                    </Grid>
                </Grid>
                <Button
                    type="submit"
                    fullWidth
                    variant="contained"
                    sx={{mt: 3, mb: 2}}
                >
                    Register
                </Button>
                {error && <Alert severity="error" sx={{mt: 1, mb: 2}}>{error}</Alert>}
                <Grid container justifyContent="flex-end">
                    <Grid item>
                        <Link href={paths.login} onClick={onLoginClick} variant="body2">
                            Already have an account? Login
                        </Link>
                    </Grid>
                </Grid>
            </Box>
        </Box>
    );

    async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
        const isValid = validate();
        event.preventDefault();
        if (!isValid)
            return;
        await dispatch(register({login: login!, password: password!}));
    }

    function validate() {
        setRunValidation(true);
        const loginValidationResult = validateLogin(login);
        const passwordValidationResult = validatePassword(password);
        setLoginError(loginValidationResult);
        setPasswordError(passwordValidationResult);
        return !loginValidationResult.error && !passwordValidationResult.error;
    }

    function onLoginClick(e: React.MouseEvent) {
        e.preventDefault();
        navigate(paths.login)
    }

    function onLoginChange(e: React.ChangeEvent<HTMLTextAreaElement>) {
        const value = e.target.value;
        setLogin(value);
        if (runValidation)
            setLoginError(validateLogin(value));
    }

    function onPasswordChange(e: React.ChangeEvent<HTMLTextAreaElement>) {
        const value = e.target.value;
        setPassword(e.target.value)
        if (runValidation)
            setPasswordError(validatePassword(value));
    }
}

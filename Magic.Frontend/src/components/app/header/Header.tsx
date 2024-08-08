import * as React from 'react';
import AppBar from '@mui/material/AppBar';
import Box from '@mui/material/Box';
import Toolbar from '@mui/material/Toolbar';
import Typography from '@mui/material/Typography';
import Button from '@mui/material/Button';
import {useNavigate} from "react-router-dom";
import paths from "../../../consts/paths";
import {useSelector} from "react-redux";
import {RootState} from "../../../redux/redux";
import Account from "./account/Account";
import {PaletteMode} from "@mui/material";
import ToggleColorMode from "./ToggleColorMode";

const logoStyle = {
    width: '140px',
    height: 'auto',
    cursor: 'pointer',
};

interface HeaderProps {
    mode: PaletteMode;
    toggleColorMode: () => void;
}

export default function Header({mode, toggleColorMode}: HeaderProps) {
    const navigate = useNavigate();
    const isLoggedIn = useSelector((state: RootState) => state.auth.token !== null);

    return (
        <Box sx={{flexGrow: 1}}>
            <AppBar position="static" color="inherit">
                <Toolbar>
                    <Box component="div" sx={{flexGrow: 1}}>
                        <Typography variant="h6" component="div" style={logoStyle} onClick={onHomeClick}>
                            Magic
                        </Typography>
                    </Box>
                    <ToggleColorMode mode={mode} toggleColorMode={toggleColorMode}/>
                    {isLoggedIn
                        ? <Account/>
                        : <>
                            <Button onClick={onRegisterClick} color="inherit">Register</Button>
                            <Button onClick={onLoginClick} color="inherit">Login</Button>
                        </>
                    }

                </Toolbar>
            </AppBar>
        </Box>
    );

    function onHomeClick() {
        navigate(paths.home)
    }

    function onLoginClick() {
        navigate(paths.login)
    }

    function onRegisterClick() {
        navigate(paths.register)
    }
}

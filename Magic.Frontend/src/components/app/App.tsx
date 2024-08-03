import * as React from "react";
import {
    Routes,
    Route,
    Outlet,
} from "react-router-dom";
import Header from "../header";
import paths from "../../consts/paths";
import Login from "../login";
import Register from "../register";
import RequireAuth from "../common/RequireAuth";
import RequireAnonym from "../common/RequireAnonym";
import {createTheme, Link, PaletteMode, ThemeProvider} from "@mui/material";
import {getPaletteMode, setPaletteMode} from "../../utils/localStorage";
import Container from "@mui/material/Container";
import CssBaseline from "@mui/material/CssBaseline";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import AccessibleForwardIcon from '@mui/icons-material/AccessibleForward';
import Typography from "@mui/material/Typography";
import Copyright from "../copyright";
import Profile from "../profile/profile";

export default function App() {
    return (
        <Routes>
            <Route element={<Layout/>}>
                <Route path={paths.home} element={<HomePage/>}/>
                <Route element={<RequireAnonym/>}>
                    <Route path={paths.login} element={<Login/>}/>
                    <Route path={paths.register} element={<Register/>}/>
                </Route>
                <Route element={<RequireAuth/>}>
                    <Route path={paths.profile} element={<Profile/>}/>
                </Route>
            </Route>
        </Routes>
    );
}

function Layout() {
    const [mode, setMode] = React.useState<PaletteMode>(getPaletteMode() || 'light');
    const defaultTheme = createTheme({palette: {mode}});

    const toggleColorMode = () => {
        setMode((prev) => {
            const newMode = prev === 'dark' ? 'light' : 'dark';
            setPaletteMode(newMode)
            return newMode;
        });
    };

    return (
        <ThemeProvider theme={defaultTheme}>
            <Header mode={mode} toggleColorMode={toggleColorMode}/>
            <Outlet/>
        </ThemeProvider>
    );
}


function HomePage() {
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
                    <AccessibleForwardIcon />
                </Avatar>
                <Typography component="h1" variant="h5">
                    Magic
                </Typography>
                <Typography variant="body1" color="text.secondary">
                    Хуй засунь в пизду Андрея, получишь ты Альберта <br />
                    И, хоть, бывает, может, там, денег нам давай.
                </Typography>
            </Box>
            <Copyright sx={{mt: 8, mb: 4}}/>
        </Container>);
}

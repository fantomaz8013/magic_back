import * as React from "react";
import {createTheme, PaletteMode, ThemeProvider} from "@mui/material";
import {getPaletteMode, setPaletteMode} from "../../utils/localStorage";
import Header from "../header";
import Page from "../common/Page";
import {Outlet} from "react-router-dom";

export default function Layout() {
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
            <Page>
                <Outlet/>
            </Page>
        </ThemeProvider>
    );
}

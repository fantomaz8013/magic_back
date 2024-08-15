import * as React from "react";
import {createTheme, PaletteMode, ThemeProvider} from "@mui/material";
import Header from "./header/Header";
import {Outlet} from "react-router-dom";

export default function Layout() {
    const mode = 'dark' as PaletteMode;
    const defaultTheme = createTheme({
        palette: {mode},
        typography: {
            "fontFamily": `"Geist", sans-serif, "Arial"`,
        }
    });
    return (
        <ThemeProvider theme={defaultTheme}>
            <Header/>
            <Outlet/>
        </ThemeProvider>
    );
}

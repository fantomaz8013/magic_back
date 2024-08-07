import Container from "@mui/material/Container";
import CssBaseline from "@mui/material/CssBaseline";
import * as React from "react";
import {Outlet} from "react-router-dom";

export default function Page() {
    return (<>
        <Container component="main" maxWidth="xs">
            <CssBaseline/>
            <Outlet/>
        </Container>
        {/*<Copyright sx={{*/}
        {/*    position: 'fixed',*/}
        {/*    bottom: 0,*/}
        {/*    width: '100%',*/}
        {/*    height: 40,*/}
        {/*    textAlign: 'center'*/}
        {/*}}/>*/}
    </>);
}

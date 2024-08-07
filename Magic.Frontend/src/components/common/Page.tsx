import Container from "@mui/material/Container";
import CssBaseline from "@mui/material/CssBaseline";
import * as React from "react";

export default function Page({children}: { children: JSX.Element }) {
    return (<>
        <Container component="main" maxWidth="xs">
            <CssBaseline/>
            {children}
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

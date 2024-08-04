import Container from "@mui/material/Container";
import CssBaseline from "@mui/material/CssBaseline";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import AccessibleForwardIcon from "@mui/icons-material/AccessibleForward";
import Copyright from "../copyright";
import * as React from "react";
import Chat from "./index";

export default function ChatPage() {
    return (<Container component="main" maxWidth="xs">
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
                <AccessibleForwardIcon/>
            </Avatar>
            <Chat/>
        </Box>
        <Copyright sx={{mt: 8, mb: 4}}/>
    </Container>);

}

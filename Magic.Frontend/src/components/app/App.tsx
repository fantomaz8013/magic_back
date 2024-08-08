import * as React from "react";
import {
    Routes,
    Route,
} from "react-router-dom";
import paths from "../../consts/paths";
import Login from "../login/Login";
import Register from "../register/Register";
import RequireAuth from "../common/RequireAuth";
import RequireAnonym from "../common/RequireAnonym";
import Profile from "../profile/Profile";
import Layout from "./Layout";
import HomePage from "./HomePage";
import GameSession from "../gameSession/GameSession";
import Page from "../common/Page";
import CssBaseline from "@mui/material/CssBaseline";

export default function App() {
    return (
        <Routes>
            <Route element={<Layout/>}>
                <Route element={<Page/>}>
                    <Route element={<RequireAnonym/>}>
                        <Route path={paths.login} element={<Login/>}/>
                        <Route path={paths.register} element={<Register/>}/>
                    </Route>
                    <Route element={<RequireAuth/>}>
                        <Route path={paths.profile} element={<Profile/>}/>
                    </Route>
                    <Route path={paths.default} element={<HomePage/>}/>
                </Route>
                <Route element={<RequireAuth/>}>
                    <Route path={paths.game + "/:gameSessionId"}
                           element={
                               <>
                                   <CssBaseline/>
                                   <GameSession/>
                               </>}
                    />
                </Route>
            </Route>
        </Routes>
    );
}

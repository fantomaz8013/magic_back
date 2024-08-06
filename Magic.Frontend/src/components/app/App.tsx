import * as React from "react";
import {
    Routes,
    Route,
} from "react-router-dom";
import paths from "../../consts/paths";
import Login from "../login";
import Register from "../register";
import RequireAuth from "../common/RequireAuth";
import RequireAnonym from "../common/RequireAnonym";
import Profile from "../profile/profile";
import Layout from "./Layout";
import HomePage from "./HomePage";
import GameSession from "../gameSession";

export default function App() {
    return (
        <Routes>
            <Route element={<Layout/>}>
                <Route element={<RequireAnonym/>}>
                    <Route path={paths.login} element={<Login/>}/>
                    <Route path={paths.register} element={<Register/>}/>
                </Route>
                <Route element={<RequireAuth/>}>
                    <Route path={paths.profile} element={<Profile/>}/>
                    <Route path={paths.game}
                           element={<GameSession gameSessionId={'945da2d0-a0ac-4257-9f9e-10b31e3955d3'}/>}/>
                </Route>
                <Route path={paths.default} element={<HomePage/>}/>
            </Route>
        </Routes>
    );
}

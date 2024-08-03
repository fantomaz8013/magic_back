import {useSelector} from "react-redux";
import {RootState} from "../../redux";
import {Navigate, Outlet, useLocation} from "react-router-dom";
import paths from "../../consts/paths";
import * as React from "react";

export default function RequireAuth() {
    const token = useSelector((state: RootState) => state.auth.token)
    let location = useLocation();

    if (!token) {
        return <Navigate to={paths.login} state={{from: location}} replace/>;
    }

    return <Outlet/>;
}

import {useSelector} from "react-redux";
import {RootState} from "../../redux/redux";
import {Navigate, Outlet, useLocation} from "react-router-dom";
import paths from "../../consts/paths";
import * as React from "react";

export default function RequireAnonym() {
    const token = useSelector((state: RootState) => state.auth.token)
    const location = useLocation();

    if (token) {
        if(location.state?.from?.pathname)
            return <Navigate to={location.state.from.pathname} state={{}} replace/>;
        return <Navigate to={paths.home} state={{from: location}} replace/>;
    }

    return <Outlet/>;
}

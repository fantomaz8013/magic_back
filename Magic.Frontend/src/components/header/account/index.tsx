import {useGetCurrentUserQuery} from "../../../redux/toolkit/api/userApi";
import React from "react";
import {Avatar, Box, CircularProgress, IconButton, Menu, MenuItem, Tooltip, Typography} from "@mui/material";
import {logout} from "../../../redux/toolkit/slices/authSlice";
import {useDispatch} from "react-redux";
import {AppDispatch} from "../../../redux";
import {useNavigate} from "react-router-dom";
import paths from "../../../consts/paths";

export default function Account() {
    const {isLoading, data} = useGetCurrentUserQuery();
    const dispatch = useDispatch<AppDispatch>();
    const navigate = useNavigate();
    const [anchorElUser, setAnchorElUser] = React.useState<null | HTMLElement>(null);
    const settings = [{
        text: 'Profile',
        action: () => {
            navigate(paths.profile);
            handleCloseUserMenu();
        }
    }, {
        text: 'Logout',
        action: () => {
            dispatch(logout());
            handleCloseUserMenu();
            navigate(paths.login)
        }
    }];
    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElUser(event.currentTarget);
    };

    const handleCloseUserMenu = () => {
        setAnchorElUser(null);
    };

    if (!data || isLoading)
        return (<Box sx={{display: 'flex'}}>
            <CircularProgress/>
        </Box>);

    return (
        <Box sx={{flexGrow: 0}}>
            <Tooltip title="Open settings">
                <IconButton onClick={handleOpenUserMenu} sx={{p: 0}}>
                    <Avatar sx={{m: 1, bgcolor: 'secondary.main'}}>
                        {data!.login.slice(0, 1)}
                    </Avatar>
                </IconButton>
            </Tooltip>
            <Menu
                sx={{mt: '45px'}}
                id="menu-appbar"
                anchorEl={anchorElUser}
                anchorOrigin={{
                    vertical: 'top',
                    horizontal: 'right',
                }}
                keepMounted
                transformOrigin={{
                    vertical: 'top',
                    horizontal: 'right',
                }}
                open={Boolean(anchorElUser)}
                onClose={handleCloseUserMenu}
            >
                {settings.map((setting) => (
                    <MenuItem key={setting.text} onClick={setting.action}>
                        <Typography textAlign="center">{setting.text}</Typography>
                    </MenuItem>
                ))}
            </Menu>
        </Box>

    );
}

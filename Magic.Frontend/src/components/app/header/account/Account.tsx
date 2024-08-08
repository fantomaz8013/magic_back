import React from "react";
import {useDispatch} from "react-redux";
import {useNavigate} from "react-router-dom";
import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import Tooltip from "@mui/material/Tooltip";
import IconButton from "@mui/material/IconButton";
import Avatar from "@mui/material/Avatar";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import Typography from "@mui/material/Typography";
import {useGetCurrentUserQuery} from "../../../../redux/toolkit/api/userApi";
import {AppDispatch} from "../../../../redux/redux";
import paths from "../../../../consts/paths";
import {resetToken} from "../../../../redux/toolkit/slices/tokenSlice";

export default function Account() {
    const {isLoading, data: currentUser} = useGetCurrentUserQuery();
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
            dispatch(resetToken());
            handleCloseUserMenu();
        }
    }];
    const handleOpenUserMenu = (event: React.MouseEvent<HTMLElement>) => {
        setAnchorElUser(event.currentTarget);
    };

    const handleCloseUserMenu = () => {
        setAnchorElUser(null);
    };

    return (
        <Box sx={{flexGrow: 0}}>
            <Tooltip title="Open settings">
                <IconButton onClick={handleOpenUserMenu} sx={{p: 0}}>
                    <Avatar sx={{m: 1, bgcolor: 'secondary.main'}}>
                        {currentUser?.data?.login.slice(0, 1) || ''}
                    </Avatar>
                    {isLoading && (
                        <CircularProgress
                            size={48}
                            sx={{
                                position: 'absolute',
                                top: 4,
                                left: 4,
                                zIndex: 1,
                            }}
                        />
                    )}
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

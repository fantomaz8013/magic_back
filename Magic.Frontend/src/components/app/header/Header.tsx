import * as React from "react";
import AppBar from "@mui/material/AppBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import Button from "@mui/material/Button";
import { Link, useNavigate } from "react-router-dom";
import paths from "../../../consts/paths";
import { useSelector } from "react-redux";
import { RootState } from "../../../redux/redux";
import Account from "./account/Account";
import { Container, } from "@mui/material";
import { Logo } from "../../../assets/vector/Logo";
import className from "./header.styles";


export default function Header() {
  const navigate = useNavigate();
  const isLoggedIn = useSelector(
    (state: RootState) => state.auth.token !== null
  );

  return (
    <AppBar position="sticky" color="inherit">
      <Box style={className.headerContainer}>
        <Container maxWidth="xl">
          <Toolbar style={className.header}>
            <Link to={paths.home} style={className.logo}>
              <Logo />
              <span>Magic</span>
            </Link>
            <Box style={className.rightController}>
              {isLoggedIn ? (
                <Account />
              ) : (
                <>
                  <Button onClick={onRegisterClick} color="inherit">
                    Register
                  </Button>
                  <Button onClick={onLoginClick} color="inherit">
                    Login
                  </Button>
                </>
              )}
            </Box>
          </Toolbar>
        </Container>
      </Box>
    </AppBar>
  );

  function onHomeClick() {
    navigate(paths.home);
  }

  function onLoginClick() {
    navigate(paths.login);
  }

  function onRegisterClick() {
    navigate(paths.register);
  }
}

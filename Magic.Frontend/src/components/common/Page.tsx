import styled from "@emotion/styled";
import { Box } from "@mui/material";
import Container from "@mui/material/Container";
import CssBaseline from "@mui/material/CssBaseline";
import * as React from "react";
import { Outlet } from "react-router-dom";
import bg from "../../assets/pictures/fonMain.jpg";
const style = {
  box: {
    padding: "3.75em 0",
  },
  fon: {
    backgroundImage: `url(${bg})`,
    backgroundSize: `cover`,
    height: 'calc(100vh - 5.0625em)'
  }
};

export default function Page() {
  return (
    <div style={style.fon}>
      <Container component="main" maxWidth="xl">
        <Box style={style.box}>
          <CssBaseline />
          <Outlet />
        </Box>
      </Container>
    </div>
  );
}

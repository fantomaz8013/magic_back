import { useLocation, useNavigate } from "react-router-dom";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import {
  Card,
  CardActions,
  CardContent,
  Container,
  Divider,
  List,
  ListItem,
  ListItemButton,
  ListItemIcon,
  ListItemText,
} from "@mui/material";
import Button from "@mui/material/Button";
import paths from "../../consts/paths";
import * as React from "react";
import { useEffect } from "react";
import {
  useCreateMutation,
  useDeleteRequestMutation,
  useEnterMutation,
  useListQuery,
} from "../../redux/api/gameSessionApi";
import { GameSessionResponse } from "../../models/response/gameSessionResponse";
import { useGetCurrentUserQuery } from "../../redux/api/userApi";
import { useDispatch } from "react-redux";
import { AppDispatch } from "../../redux/redux";
import { clearGameSessionData } from "../../redux/slices/gameSessionSlice";
import className from "./home.style";
import { MenuBorder } from "../../assets/vector/MenuBorder";

export default function HomePage() {
  const dispatch = useDispatch<AppDispatch>();

  // useEffect(() => {
  //   if (location.pathname !== paths.home) {
  //     navigate(paths.home, { replace: true });
  //   }
  // }, [location, navigate]);

  useEffect(() => {
    dispatch(clearGameSessionData());
  }, []);

  return (
    <Grid item xs={9}>
      главная
    </Grid>
  );
}

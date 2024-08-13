import { Grid, Button } from "@mui/material";
import { MenuBorder } from "../../assets/vector/MenuBorder";
import className from "./main.style";
import { NavLink, Outlet } from "react-router-dom";
import { IconKeyframe, IconSwords } from "@tabler/icons-react";
import {useDispatch} from "react-redux";
import {AppDispatch} from "../../redux/redux";
import {useEffect} from "react";
import {clearGameSessionData} from "../../redux/slices/gameSessionSlice";

export default function Main() {
  const dispatch = useDispatch<AppDispatch>();

  useEffect(() => {
    dispatch(clearGameSessionData());
  }, []);

  return (
    <Grid container spacing={4}>
      <Grid item xs={3}>
        <nav aria-label="main-menu" style={className.menu}>
          <div>
            <MenuBorder />
          </div>
          <ul style={className.menuList}>
            <li style={className.menuItem}>
              <NavLink to="/" style={{ color: "inherit" }}>
                {({ isActive, isPending, isTransitioning }) => (
                  <Button
                    sx={{
                      ...className.menuItemBtn,
                      color: isActive ? "#CA5C40" : "inherit",
                      borderColor: isActive ? "#CA5C40" : "inherit",
                    }}
                  >
                    <IconKeyframe stroke={2} />
                    Главная
                  </Button>
                )}
              </NavLink>
            </li>
            <li style={className.menuItem}>
              <NavLink to="/session" style={{ color: "inherit" }}>
                {({ isActive, isPending, isTransitioning }) => (
                  <>
                    <Button
                      sx={{
                        ...className.menuItemBtn,
                        color: isActive ? "#CA5C40" : "inherit",
                        borderColor: isActive ? "#CA5C40" : "inherit",
                      }}
                    >
                      <IconSwords stroke={2} />
                      <span>Сессии</span>
                    </Button>
                  </>
                )}
              </NavLink>
            </li>
          </ul>
          <div style={{ transform: "rotate(180deg)" }}>
            <MenuBorder />
          </div>
        </nav>
      </Grid>
      <Outlet />
    </Grid>
  );
}

import styled from "@emotion/styled";
import { TableCell, tableCellClasses, TableContainer, TableRow } from "@mui/material";

export const Cell = styled(TableCell)(({ theme }) => ({
  [`&.${tableCellClasses.head}`]: {
    background: "#191819",
    borderColor: "#313437",
    fontFamily: "Geist",
    textTransform: "uppercase",
    fontWeight: 600,
    fontSize: ".9em",
    padding: "1em",
  },
  [`&.${tableCellClasses.body}`]: {
    fontSize: 14,
  },
}));
export const ContainerTable = styled(TableContainer)(({ theme }) => ({
  [`&::-webkit-scrollbar`]: {
    display: 'none'
  },
  [`&`]: {
    ['-ms-overflow-style']: 'none',
    scrollbarWidth: 'none'
  },
}));
export const Row = styled(TableRow)(({ theme }) => ({
  "&": {
    backgroundColor: "rgba(0, 0, 0, 0.2)",
    transition: ".4s",
    cursor: "pointer",
    borderColor: "#222222",
  },
  "&:hover": {
    backgroundColor: "rgba(255, 255, 255, 0.05)",
  },
  "&:last-child td, &:last-child th": {
    border: 0,
  },
  "& td, & th": {
    color: "#B9B9B9",
    borderColor: "#222222",
    fontFamily: "Geist",
    fontWeight: 400,
  },
}));

const columns = [
  {
    dataKey: "title",
    label: "Название игры",
  },
  {
    dataKey: "description",
    label: "Описание",
  },
  {
    dataKey: "3",
    center: "center",
    label: "Игроки",
  },
  {
    dataKey: "4",
    center: "center",
    label: "Статус",
  },
  {
    dataKey: "5",
    center: "center",
    label: "Пароль",
  },
];
export function HeaderTable() {
  return (
    <TableRow>
      {columns.map((column) => (
        <Cell
          key={column.dataKey}
          variant="head"
          align={column.center || false ? "center" : "left"}
          sx={{
            backgroundColor: "background.paper",
          }}
        >
          {column.label}
        </Cell>
      ))}
    </TableRow>
  );
}
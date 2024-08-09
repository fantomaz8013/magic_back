export const rowKey = (rowIdx: number) => `${rowIdx + 1}`;

export const cellKey = (rowIdx: number, colIdx: number) => `${rowKey(rowIdx)}:${colIdx}`;

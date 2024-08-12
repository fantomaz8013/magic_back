const className = {
  block: {
    padding: '2.5em 1.375em',
    height: 'calc(100vh - 12.5625em)',
    display: 'flex',
    flexDirection: 'column' as 'column',
    gap: '.6em',
    background: "rgba(13, 13, 14, 0.9)",
    border: "1px solid rgba(255, 255, 255, 0.3)",
    alignItems: 'flex-start',
    borderRadius: ".5em",
  },
  longTxt: {
    maxWidth: '200px',
    whiteSpace: 'nowrap',
    overflow: 'hidden',
    textOverflow: 'ellipsis'
  },
  tableWrapper: {
    width: '100%', 
    maxHeight: '100%',
  },
  tableIconWrapper: {
    background: 'rgba(255, 255, 255, 0.05)',
    borderRadius: '4px',
    fontFamily: "Geist",
    fontWeight: 600,
    padding: '.6em .8em',
    display: 'flex',
    width: 'fit-content',
    margin: 'auto'
  }
};

export default className;
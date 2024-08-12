const className = {
  menuItem: {
    width: '100%',
    cursor: 'pointer',
    background: '#202021',
    borderRadius: '.5em',
  },
  menuItemBtn: {
    gap: '.4em',
    alignItems: 'center',
    width: '100%',
    color: 'inherit',
    fontSize: '1em',
    fontFamily: 'Geist',
    justifyContent: 'flex-start',
    borderRadius: '.5em',
    borderLeft: '2px solid #B9B9B9',
    padding: '1em 1.125em',
  },
  menuList: {
    width: '100%',
    padding: '0',
    display: 'flex',
    flexDirection: 'column' as 'column',
    gap: '.8em',
    listStyle: 'none'
  },
  menu: {
    padding: '2.5em 1.375em',
    display: 'flex',
    flexDirection: 'column' as 'column',
    gap: '.6em',
    background: "rgba(13, 13, 14, 0.9)",
    border: "1px solid rgba(255, 255, 255, 0.3)",
    alignItems: 'center',
    borderRadius: ".5em",
  },
};

export default className;

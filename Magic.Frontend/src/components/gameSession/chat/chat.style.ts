import bg from "../../../assets/pictures/chatFon.jpg";

export const className = {
  chatBox: {
    backgroundImage: `url(${bg})`,
    width: "100%",
  },
  chatContainer: {
    overflowY: 'scroll' as "scroll",
    fontFamily: "Geist",
    height: "calc(100% - 80px)",
    width: "100%",
    display: "flex",
    flexDirection: "column" as "column",
  },
  message: {
    margin: '1em',
  },
  avatar: {
    display: 'flex',
    background: '#515151',
    borderRadius: '.25em',
    justifyContent: 'center',
    alignItems: 'center',
    maxWidth: '48px',
    width: '100%',
    height: '48px',
    marginRight: '20px'
  },
  scroll: {
    background: 'rgba(255, 255, 255, 0.10)',
    borderRadius: '1em'
  },
  senderMessage: {
    marginBottom: '1em',
    display: 'flex',
    justifyContent: 'flex-start',
  },
  receiverMessage: {
    marginBottom: '1em',
    display: 'flex',
    justifyContent: 'flex-end',
  },
  serverMessage: {
    overflow: 'hidden',
    display: '-webkit-box',
    color: '#fff',
    width: '100%',
    padding: '1.25em',
    background: 'rgba(79, 79, 79, 0.25)',
    border: '1px solid rgba(255, 255, 255, 0.15)',
    borderRadius: '0px 10px 10px 10px',
  },
  userMessage: {
    overflow: 'hidden',
    minWidth: '265px',
    display: '-webkit-box',
    color: '#fff',
    padding: '1.25em',
    background: '#CA5C40',
    borderRadius: '0px 10px 10px 10px'
  },
  chat: {
    height: "100%",
    marginBlock: '1em'
  },
  chatIcon: {
    color: '#fff',
  },
  chatController: {
    height: "5em",
    display: "flex",
    borderTop: "1px solid rgba(255, 255, 255, 0.15)",
  },
  input: {
    fontSize: '1em',
    color: "#fff",
    fontFamily: 'inherit',
    caretColor: "#fff",
    paddingLeft: "1.5em",
    background: "transparent",
    outline: "none",
    width: "100%",
    border: "none",
  },
};

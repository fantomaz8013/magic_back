import bg from "../../assets/pictures/fonGameSession.jpg";

const className = {
  container: {
    height: "calc(100svh - 81px)",
    backgroundSize: "cover",
    backgroundRepeat: "no-repeat",
    backgroundPosition: "center",
    backgroundImage: `url(${bg})`,
  },
  block: {
    display: "flex",
  },
  chat: {
    display: 'flex',
    height: 'calc(100svh - 81px)',
    maxWidth: '380px',
    width: '100%',
    flexDirection: ' row-reverse',
  },
};

export default className;

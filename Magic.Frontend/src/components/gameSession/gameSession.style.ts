import bg from "../../assets/pictures/fonGameSession.jpg";

const className = {
  container: {
    height: "calc(100svh - 81px)",
    backgroundSize: "cover",
    backgroundRepeat: "no-repeat",
    backgroundPosition: "center",
    backgroundImage: `url(${bg})`,
  },
  page: {
    display: "flex",
    height:'100%'
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

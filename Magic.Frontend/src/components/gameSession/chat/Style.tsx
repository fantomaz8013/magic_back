export const Style = () => {
  return (
    <style>
      {` 
        ::placeholder { 
            color: #9B9B9B;
            font-weight: 500;
            font-size: 1em;
            font-family: "Geist";
        }
        .rcs-inner-handle {
          border-radius: 1em;
          background-color: rgba(255, 255, 255, 0.10);
        }
        .chat ::-webkit-scrollbar {
          width: 8px;
        }
        .chat ::-webkit-scrollbar-track {
          background: transition;
        }

        .chat ::-webkit-scrollbar-thumb {
          background: rgba(255, 255, 255, .1);
          border-radius: 4px;
        }

        .chat ::-webkit-scrollbar-thumb:hover {
          background: rgba(255, 255, 255, .2);
        }
      `}
    </style>
  );
};
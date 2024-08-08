import React, {useEffect, useState} from "react";
import {useSelector} from "react-redux";
import {RootState} from "../../../redux/redux";
import {Snackbar} from "@mui/material";
import {RequestedSaveThrowPassed} from "../../../redux/toolkit/slices/gameSessionSlice";

export function EventSnackbar() {
    const [snackBarMessage, setSnackBarMessage] = useState<string | null>(null);
    const requestedSaveThrowPassed = useSelector((state: RootState) => state.gameSession.requestedSaveThrowPassed);

    useEffect(() => {
        if (!!requestedSaveThrowPassed) {
            setSnackBarMessage(buildSnackBarMessage(requestedSaveThrowPassed));
        }
    }, [requestedSaveThrowPassed]);

    return (
        <Snackbar
            open={!!snackBarMessage}
            autoHideDuration={6000}
            onClick={closeSnackBar}
            message={snackBarMessage}
        />
    );

    function closeSnackBar() {
        setSnackBarMessage(null);
    }

    function buildSnackBarMessage(requestPassed: RequestedSaveThrowPassed) {
        const hasPassed = requestPassed.resultRollValue >= requestPassed.value;
        return `Проверка ` + (hasPassed ? 'пройдена' : 'провалена');
    }
}

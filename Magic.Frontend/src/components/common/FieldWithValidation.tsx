import * as React from "react";
import TextField from "@mui/material/TextField";
import {useState} from "react";
import {ValidationResult} from "../validation";
import {BaseTextFieldProps} from "@mui/material/TextField/TextField";

export interface ProfileFieldProps {
    name: string;
    type: BaseTextFieldProps['type']
    runValidation?: boolean;
    validateFunction?: (value: string) => ValidationResult | null;
    defaultValue?: string | null;
}

export default function ProfileField(props: ProfileFieldProps) {
    const {name, type, runValidation, validateFunction, defaultValue = null} = props;
    const [value, setValue] = useState<string | null>(defaultValue);
    const [error, setError] = useState<ValidationResult | null>(null);

    return (
        <TextField
            type={type}
            margin="normal"
            required
            fullWidth
            id={name}
            label={name}
            name={name}
            autoComplete={name}
            autoFocus
            value={value}
            onChange={onChange}
            {...error}
        />
    );

    function onChange(e: React.ChangeEvent<HTMLTextAreaElement>) {
        const value = e.target.value;
        setValue(value);
        if (runValidation && validateFunction)
            setError(validateFunction(value));
    }
}

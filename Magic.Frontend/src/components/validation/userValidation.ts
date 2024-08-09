import {okResult, validateStringByConfig, ValidationConfig, ValidationResult} from "./index";

const config: Record<string, ValidationConfig> = {
    login: {
        fieldName: "Login",
        minLength: 4,
        regex: {
            explain: "Should contain only letters",
            expression: /^[a-zA-Z]+$/
        }
    },
    password: {
        fieldName: "Password",
        minLength: 6,
    },
    phoneNumber:{
        fieldName: "PhoneNumber",
        regex: {
            explain: "Should contain only numbers",
            expression: /^[0-9]+$/
        }
    }
}

export function validateLogin(login: string | null): ValidationResult {
    return validateStringByConfig(login, config.login) || okResult;
}

export function validatePassword(password: string | null): ValidationResult {
    return validateStringByConfig(password, config.password) || okResult;
}

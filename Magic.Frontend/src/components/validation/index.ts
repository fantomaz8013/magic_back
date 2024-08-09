export interface ValidationResult {
    error: boolean;
    helperText?: string;
}

export interface ValidationConfig {
    fieldName: string;
    minLength?: number;
    maxLength?: number;
    regex?: {
        explain: string;
        expression: RegExp;
    };
}

export const okResult: ValidationResult = {error: false};
export const errorResult = (helperText?: string): ValidationResult => ({error: true, helperText});

export const validateStringByConfig = (value: string | undefined | null, {
    minLength,
    maxLength,
    regex,
    fieldName,
}: ValidationConfig) => {
    if (value === undefined || value === null || value.length === 0) {
        return errorResult(`${fieldName} should not be empty`);
    }

    if (regex && !regex.expression.test(value)) {
        return errorResult(regex.explain);
    }

    const length = value.length;
    if (maxLength && length > maxLength) {
        return errorResult(`${fieldName} too long`);
    }

    if (minLength && length < minLength) {
        return errorResult(`${fieldName} too short`);
    }

    return null;
}

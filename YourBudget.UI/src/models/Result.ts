import { ValidationError } from './ValidationError';

export interface Result {
    isSuccess: boolean,
    message: string,
    validationErrors: ValidationError[]
}

export interface DataResult<T> extends Result {
    data: T
}
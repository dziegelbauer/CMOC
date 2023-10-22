export enum ServiceResult {
    Success,
    Failure,
    InUse,
    NotFound
}

export type ApiResponse<T> = {
    result: ServiceResult;
    payload: T | null; 
    message: string;
}
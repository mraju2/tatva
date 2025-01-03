export interface ApiResponse {
  text: string;
}

export interface ApiError {
  message: string;
  code: string;
}

export type ApiResult<T> = {
  data?: T;
  error?: ApiError;
};
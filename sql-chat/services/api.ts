import { Message } from "../types/chat"
import { ApiResponse, ApiResult } from "../types/api"

export class ApiError extends Error {
  constructor(
    message: string,
    public code: string = 'UNKNOWN_ERROR'
  ) {
    super(message);
    this.name = 'ApiError';
  }
}

export async function sendMessage(content: string): Promise<Message> {
  try {
    // Replace this with your actual API implementation
    const response = await new Promise<ApiResult<ApiResponse>>((resolve) => {
      setTimeout(() => {
        resolve({ 
          data: { text: `Response to: ${content}` }
        });
      }, 1000);
    });

    if (response.error) {
      throw new ApiError(response.error.message, response.error.code);
    }

    if (!response.data) {
      throw new ApiError('No response data received');
    }

    return {
      id: Date.now().toString(),
      content: response.data.text,
      role: 'assistant',
      timestamp: Date.now(),
    };
  } catch (error) {
    if (error instanceof ApiError) {
      throw error;
    }
    throw new ApiError(
      error instanceof Error ? error.message : 'Unknown error occurred'
    );
  }
}
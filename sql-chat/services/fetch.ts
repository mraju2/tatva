type FetchServiceOptions = {
  method: 'GET' | 'POST' | 'PUT' | 'DELETE' | 'PATCH';
  endpoint: string; // Dynamic endpoint
  body?: Record<string, unknown> | FormData; // For JSON or FormData
  contentType?: string; // e.g., 'application/json', 'multipart/form-data'
  headers?: Record<string, string>;
  timeout?: number; // Optional timeout in ms, defaults to 3 minutes
};

type FetchServiceResponse<T> = T; // Generic response type for flexibility

const API_BASE_URL = process.env.REACT_APP_API_BASE_URL || 'http://localhost:8081/api';

const fetchService = async <T>({
  method,
  endpoint,
  body,
  contentType = 'application/json',
  headers = {},
  timeout = 360000, // Default timeout to 3 minutes
}: FetchServiceOptions): Promise<FetchServiceResponse<T>> => {
  // Construct full URL by attaching endpoint to base URL
  const url = `${API_BASE_URL}${endpoint.startsWith('/') ? '' : '/'}${endpoint}`;

  // Add content type to headers if not FormData
  const requestHeaders: Record<string, string> = {
    ...headers,
  };

  if (contentType && !(body instanceof FormData)) {
    requestHeaders['Content-Type'] = contentType;
  }

  // Prepare the fetch options
  const options: RequestInit = {
    method,
    headers: requestHeaders,
    body: body && contentType === 'application/json' && !(body instanceof FormData)
      ? JSON.stringify(body)
      : (body as BodyInit), // FormData or raw body
  };

  // Timeout implementation
  const controller = new AbortController();
  const timeoutId = setTimeout(() => controller.abort(), timeout);

  try {
    // Perform the fetch
    const response = await fetch(url, { ...options, signal: controller.signal });

    // Clear timeout if successful
    clearTimeout(timeoutId);

    // Check if response is ok
    if (!response.ok) {
      const errorResponse = await response.text();
      throw new Error(`HTTP error! status: ${response.status} - ${errorResponse}`);
    }

    // Parse JSON response (if applicable)
    const responseContentType = response.headers.get('Content-Type');
    if (responseContentType && responseContentType.includes('application/json')) {
      return (await response.json()) as T;
    }

    // Return raw text for non-JSON responses
    return (await response.text()) as unknown as T;
  } catch (error) {
    clearTimeout(timeoutId);

    if (error instanceof DOMException && error.name === 'AbortError') {
      throw new Error('Request timed out');
    }

    if (error instanceof Error) {
      throw error;
    }

    throw new Error('An unknown error occurred');
  }
};

export default fetchService;

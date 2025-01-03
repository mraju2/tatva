import fetchService from './fetch'; // Assume this is the fetch service from earlier
import { LLMMessageResponse,OllamaAPIResponse,transformAPIResponse } from '@/types/chat'; 

type SendMessageRequest = {
  model: string;
  userMessage: string;
};

const sendMessage = async (
  url: string,
  payload: SendMessageRequest
): Promise<LLMMessageResponse | undefined> => {
  try {
    const response = await fetchService<OllamaAPIResponse>({
      method: 'POST',
      endpoint: url,
      body: payload,
      contentType: 'application/json',
      headers: {
        'User-Agent': 'HTTPie', // Optional if required
      },
    });

    console.log('API Response:', response);

    const transformedResponse = transformAPIResponse(response.data);

    console.log('Transformed Response:', transformedResponse);


    return transformedResponse;
  } catch (error) {
    console.error('Error sending message:', error);
    return undefined;
  }
};

export default sendMessage;

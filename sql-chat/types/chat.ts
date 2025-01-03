export interface Message {
    id: string;
    content: string;
    role: 'user' | 'assistant';
    timestamp: number;
    sqlQuery?:string;
  }
  
  export interface ChatState {
    messages: Message[];
    isLoading: boolean;
    addMessage: (message: Omit<Message, 'id' | 'timestamp'>) => void;
    setLoading: (loading: boolean) => void;
  }


  // API Response Type (matches API structure)
export type LLMAPIResponse = {
  model: string;
  created_at: string;
  message: {
    role: string;
    content: string;
  };
  done_reason: string;
  done: boolean;
  total_duration: number;
  load_duration: number;
  prompt_eval_count: number;
  prompt_eval_duration: number;
  eval_count: number;
  eval_duration: number;
  query?:string;
};

export type OllamaAPIResponse = {
  success: boolean;
  data: LLMAPIResponse;
};

// Application Type (cleaned structure without underscores)
export type LLMMessageResponse = {
  model: string;
  createdAt: string;
  message: {
    role: string;
    content: string;
  };
  doneReason: string;
  done: boolean;
  totalDuration: number;
  loadDuration: number;
  promptEvalCount: number;
  promptEvalDuration: number;
  evalCount: number;
  evalDuration: number;
  sqlQuery? :string
};


export const transformAPIResponse = (apiResponse: LLMAPIResponse): LLMMessageResponse => {
  return {
    model: apiResponse.model,
    createdAt: apiResponse.created_at,
    message: {
      role: apiResponse.message.role,
      content: apiResponse.message.content,
    },
    doneReason: apiResponse.done_reason,
    done: apiResponse.done,
    totalDuration: apiResponse.total_duration,
    loadDuration: apiResponse.load_duration,
    promptEvalCount: apiResponse.prompt_eval_count,
    promptEvalDuration: apiResponse.prompt_eval_duration,
    evalCount: apiResponse.eval_count,
    evalDuration: apiResponse.eval_duration,
    sqlQuery: apiResponse.query
  };
};

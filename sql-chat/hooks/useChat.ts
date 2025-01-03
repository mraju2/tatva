import { useState, useCallback } from 'react';
import { Message } from '@/types/chat';
import { sendMessage } from "../services/api";

export function useChat() {
  const [messages, setMessages] = useState<Message[]>([]);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const addMessage = useCallback((message: Omit<Message, 'id' | 'timestamp'>) => {
    const newMessage: Message = {
      ...message,
      id: Date.now().toString(),
      timestamp: Date.now(),
    };
    setMessages((prev) => [...prev, newMessage]);
  }, []);

  const handleSendMessage = useCallback(async (content: string) => {
    if (!content.trim()) return;

    // Add user message
    addMessage({ content, role: 'user' });
    
    // Send to API
    setIsLoading(true);
    try {
      const response = await sendMessage(content);
      addMessage(response);
    } catch (error) {
      console.error('Error:', error);
      // Handle error appropriately
    } finally {
      setIsLoading(false);
    }
  }, [addMessage]);

  return {
    messages,
    isLoading,
    sendMessage: handleSendMessage,
  };
}
import { Message } from "@/types/chat";
import { ChatInput } from "./chat-input";
import { MessageList } from "./message-list";
import { useEffect, useState } from "react";
import sendMessageAPI from "@/services/chat-service"; // Import the API function

export const ChatContainer = ({ resetKey }: { resetKey: number }) => {
  const [messages, setMessages] = useState<Message[]>([]);

  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    setMessages([]); // Clear messages when resetKey changes
  }, [resetKey]);

  const sendMessage = async (content: string) => {
    setIsLoading(true);
    const userMessage: Message = {
      id: `user-${Date.now()}`,
      content,
      role: "user",
      timestamp: Date.now(),
    };
    setMessages((prev) => [...prev, userMessage]);

    try {
      const response = await sendMessageAPI("/Chat/send", {
        model: "phi3",
        userMessage: content,
      });

      console.log("Response:", response);

      const assistantMessage: Message = {
        id: `assistant-${Date.now()}`,
        content: response?.message.content || "An empty response received.",
        role: "assistant",
        timestamp: Date.now(),
        sqlQuery: response?.sqlQuery || undefined,
      };

      setMessages((prev) => [...prev, assistantMessage]);
    } catch (error) {
      console.error("Error sending message:", error);
      const errorMessage: Message = {
        id: `error-${Date.now()}`,
        content:
          "An error occurred while sending your message. Please try again.",
        role: "assistant",
        timestamp: Date.now(),
      };

      setMessages((prev) => [...prev, errorMessage]);
    } finally {
      setIsLoading(false);
    }
  };

  const handleSampleClick = (sample: string) => {
    sendMessage(sample);
  };

  return (
    <div className="flex flex-col h-full bg-slate-900 text-white">
      {messages.length === 0 && (
        <div className="flex-1 flex items-center justify-center">
          <div className="p-6 rounded-lg shadow-lg">
            <h1 className="text-2xl font-bold mb-4 text-center">
              text-sql-data(experimental)
            </h1>
            <div className="flex flex-wrap justify-center gap-4">
              {[
                "Actor with most films?",
                "Cumulative revenue of all stores?",
                "What is the average running time of films by category?",
              ].map((sample, index) => (
                <button
                  key={index}
                  className="px-4 py-2 bg-slate-700 text-slate-300 rounded-lg hover:bg-slate-600 focus:ring-2 focus:ring-blue-500 transition"
                  onClick={() => handleSampleClick(sample)}
                >
                  {sample}
                </button>
              ))}
            </div>
          </div>
        </div>
      )}
      <div className="flex-1 overflow-y-auto p-6 bg-slate-900 text-white">
        <MessageList messages={messages} loading={isLoading} />{" "}
        {/* Pass loading */}
      </div>
      <div className="p-4 border-t border-slate-700 bg-slate-800">
        <ChatInput onSendMessage={sendMessage} isLoading={isLoading} />
      </div>
    </div>
  );
};

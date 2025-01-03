import React, { useEffect, useRef } from "react";

interface MessageAreaProps {
  messages: { role: "user" | "assistant"; content: string }[];
}

const MessageArea: React.FC<MessageAreaProps> = ({ messages }) => {
  const messageEndRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    messageEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  return (
    <div className="flex-1 overflow-y-auto bg-slate-900 p-6 rounded-lg border border-slate-700 shadow-inner">
      {messages.map((message, index) => (
        <div
          key={index}
          className={`flex mb-4 ${
            message.role === "user" ? "justify-end" : "justify-start"
          }`}
        >
          <div
            className={`px-4 py-2 rounded-lg text-sm ${
              message.role === "user"
                ? "bg-emerald-600 text-white"
                : "bg-slate-800 text-slate-300"
            } max-w-[75%]`}
          >
            {message.content}
          </div>
        </div>
      ))}
      <div ref={messageEndRef} />
    </div>
  );
};

export default MessageArea;

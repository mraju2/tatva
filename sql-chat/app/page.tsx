"use client";
import { ChatContainer } from "@/components/chat-container";
import { Bot } from "lucide-react";
import { useState } from "react";

export default function Chat() {
  const [chatKey, setChatKey] = useState(0); // State to force re-mount the ChatContainer

  const handleNewChat = () => {
    setChatKey((prevKey) => prevKey + 1); // Update the key to trigger re-mount
  };

  return (
    <div className="flex flex-col h-screen bg-slate-950">
      <header className="flex items-center justify-between p-4 border-b border-slate-700 bg-slate-900 shadow-sm">
        <div className="flex items-center gap-2">
          <Bot className="w-6 h-6 text-emerald-400" />
          <h1 className="text-xl font-semibold text-slate-400">
            Sql Chat Assistant
          </h1>
        </div>
        <button
          className="px-4 py-2 text-sm font-medium text-slate-400 bg-slate-800 rounded hover:bg-slate-700"
          onClick={handleNewChat}
        >
          New Chat
        </button>
      </header>
      <main className="flex-1 overflow-hidden">
        <ChatContainer resetKey={chatKey} />
      </main>
    </div>
  );
}

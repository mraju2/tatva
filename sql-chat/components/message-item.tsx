import { Message } from '@/types/chat';
import { Bot, User } from 'lucide-react';
import { formatTimestamp } from '@/lib/utils';

interface MessageItemProps {
  message: Message;
}

export function MessageItem({ message }: MessageItemProps) {
  const isBot = message.role === 'assistant';

  return (
    <div className={`flex gap-3 ${isBot ? 'bg-gray-50' : ''} p-4 rounded-lg`}>
      <div
        className={`w-8 h-8 rounded-full flex items-center justify-center ${
          isBot ? 'bg-green-100 text-green-600' : 'bg-blue-100 text-blue-600'
        }`}
      >
        {isBot ? <Bot size={20} /> : <User size={20} />}
      </div>
      <div className="flex-1">
        <div className="flex justify-between items-center mb-1">
          <span className="font-medium">{isBot ? 'Assistant' : 'You'}</span>
          <span className="text-sm text-gray-500">
            {formatTimestamp(message.timestamp)}
          </span>
        </div>
        <div className="text-gray-700 whitespace-pre-wrap">{message.content}</div>
      </div>
    </div>
  );
}
import { cn } from '@/lib/utils';
import { ButtonHTMLAttributes, forwardRef } from 'react';

export interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: 'primary' | 'secondary';
}

export const Button = forwardRef<HTMLButtonElement, ButtonProps>(
  ({ className, variant = 'primary', ...props }, ref) => {
    return (
      <button
        ref={ref}
        className={cn(
          'px-4 py-2 rounded-lg disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-2',
          variant === 'primary' && 'bg-blue-500 text-white hover:bg-blue-600',
          variant === 'secondary' && 'bg-gray-100 text-gray-700 hover:bg-gray-200',
          className
        )}
        {...props}
      />
    );
  }
);

Button.displayName = 'Button';
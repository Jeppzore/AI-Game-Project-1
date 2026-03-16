import React from 'react';

export interface BadgeProps extends React.HTMLAttributes<HTMLSpanElement> {
  variant?: 'default' | 'success' | 'warning' | 'error' | 'info';
}

export const Badge: React.FC<BadgeProps> = ({
  variant = 'default',
  children,
  className = '',
  ...props
}) => {
  return (
    <span className={`badge ${variant !== 'default' ? `badge-${variant}` : ''} ${className}`.trim()} {...props}>
      {children}
    </span>
  );
};

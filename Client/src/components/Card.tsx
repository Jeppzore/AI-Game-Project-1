import React from 'react';

export interface CardProps extends React.HTMLAttributes<HTMLDivElement> {
  header?: React.ReactNode;
  footer?: React.ReactNode;
}

export const Card: React.FC<CardProps> = ({
  header,
  footer,
  children,
  className = '',
  ...props
}) => {
  return (
    <div className={`card ${className}`.trim()} {...props}>
      {header && (
        <div className="card-header">
          {typeof header === 'string' ? <h3 className="card-title">{header}</h3> : header}
        </div>
      )}
      <div className="card-body">{children}</div>
      {footer && <div className="card-footer">{footer}</div>}
    </div>
  );
};

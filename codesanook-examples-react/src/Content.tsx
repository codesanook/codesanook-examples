import React from 'react';
import './App.css';

interface IContentProps {
  containerWidth: number;
}

const Content: React.FC<IContentProps> = (props: IContentProps) => {
  const { containerWidth } = props;
  return (
    <>
      <div style={{width:containerWidth}} className='container'>
        <strong>p's rect</strong>
        <p className='parent'>
          <span>Paragraph that spans multiple lines</span>
        </p>
      </div>
    </>
  );
}

export default Content;

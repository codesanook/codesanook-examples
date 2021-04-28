import React, { ReactNode } from 'react';

type Props = { children: ReactNode };
export default function Layout({ children }: Props) {

  return (
    <>
      <h1>wrapper</h1>
      { children}
    </>
  )
}

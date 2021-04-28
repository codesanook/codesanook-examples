import React, { ReactNode } from 'react'
import { createTeleporter } from 'react-teleporter'

const Title = createTeleporter()

export function TitleTarget() {
  return <Title.Target />
}

type Props = { children: ReactNode };

export function TitleSource({ children }: Props) {
  return <Title.Source>{children}</Title.Source>
}

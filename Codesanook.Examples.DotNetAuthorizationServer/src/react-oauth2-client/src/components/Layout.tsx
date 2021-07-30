import { Container } from 'reactstrap';
import { ReactNode } from 'react'
import NavMenu from './NavMenu';

// https://blog.echobind.com/react-with-typescript-components-as-function-declarations-vs-function-expressions-e433ac8d6938
type Props = {
  children: ReactNode
};

export default function Layout({ children }: Props) {
  return (
    <>
      <NavMenu />
      <Container>
        {children}
      </Container>
    </>
  );
}

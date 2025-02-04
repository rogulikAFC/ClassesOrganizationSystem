import { FC } from "react";
import { Button, Container, Form, FormControl, Nav, Navbar, NavDropdown } from "react-bootstrap";
import { Outlet } from "react-router";

const MainLayout: FC = () => (
  <>
    <Navbar expand="lg" className="bg-body-tertiary">
      <Container>
        <Navbar.Toggle aria-controls="basic-navbar-nav" />
        <Navbar.Collapse id="basic-navbar-nav">
          <Nav className="justify-content-between w-100">
            
              <div className="d-flex">
                <Nav.Link href="#home" className="me-a">На главную</Nav.Link>
                <Nav.Link href="#">Личный кабинет</Nav.Link>
                <Nav.Link href="#">Регистрация</Nav.Link>
              </div>

            <Form.Group className="d-flex">
              <FormControl type="search" placeholder="Поиск" className="me-1" /> 
              <Button variant="outline-primary">Поиск</Button>
            </Form.Group>
          </Nav>
        </Navbar.Collapse>
      </Container>
    </Navbar>

    <Outlet />
  </>
);

export default MainLayout;
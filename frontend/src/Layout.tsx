import {Outlet} from "react-router-dom";
import {Container, Nav, Navbar} from "react-bootstrap";
import {Link} from 'react-router-dom';

function Layout() {
    return (
        <main>
            <Navbar bg="dark" variant="dark" expand="sm">
                <Container>
                    <Navbar.Brand>
                        <Link to="/">
                            Currency API
                        </Link>
                    </Navbar.Brand>
                    <Navbar.Toggle aria-controls="navbar"/>
                    <Navbar.Collapse id="navbar">
                        <Nav>
                        <Nav.Link>
                            <Link to="/">
                                Average
                            </Link>
                        </Nav.Link>
                        <Nav.Link>
                            <Link to="/extremes">
                                Extremes
                            </Link>
                        </Nav.Link>
                        <Nav.Link>
                            <Link to="/differences">
                                Differences
                            </Link>
                        </Nav.Link>
                        <Nav.Link>
                            <Link to="/codes">
                                Codes
                            </Link>
                        </Nav.Link>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
            <Container>
                <Outlet/>
            </Container>
        </main>
    )
}

export {Layout};
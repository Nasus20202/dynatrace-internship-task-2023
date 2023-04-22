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
                            <Link className="nav-link" to="/">
                                Average
                            </Link>
                            <Link className="nav-link" to="/extremes">
                                Extremes
                            </Link>
                            <Link className="nav-link" to="/differences">
                                Differences
                            </Link>
                            <Link className="nav-link" to="/codes">
                                Codes
                            </Link>
                        </Nav>
                    </Navbar.Collapse>
                </Container>
            </Navbar>
            <Container className="mt-2">
                <Outlet/>
            </Container>
        </main>
    )
}

export {Layout};
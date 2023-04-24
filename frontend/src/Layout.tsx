import {Outlet} from "react-router-dom";
import {Container, Nav, Navbar} from "react-bootstrap";
import {Link} from 'react-router-dom';
import apiPath from "./api/ApiPath";

function Layout() {
    return (
        <main>
            <Navbar bg="dark" variant="dark" expand="sm">
                <Container>
                    <Navbar.Brand>
                        <Link to={`${apiPath}/swagger`}>
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
                            <Link className="nav-link me-1" to="/differences">
                                Differences
                            </Link>
                            <Link className="nav-link fw-semibold" to={`${apiPath}/swagger`}>
                                Swagger UI
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
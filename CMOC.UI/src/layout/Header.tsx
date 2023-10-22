import {NavLink} from "react-router-dom";

const Header = () => {
    return (
        <header>
            <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
                <div className="container">
                    <a className="navbar-brand" href="/">CMOC</a>
                    <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse" aria-controls="navbarSupportedContent"
                            aria-expanded="false" aria-label="Toggle navigation">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                        <ul className="navbar-nav flex-grow-1">
                            <li className="nav-item">
                                <NavLink className="nav-link text-dark" to="/">Home</NavLink>
                            </li>
                            <li className="nav-item dropdown">
                                <a className="nav-link text-dark dropdown-toggle" role="button" data-bs-toggle="dropdown">View Status</a>
                                <ul className="dropdown-menu">
                                    <li>
                                        <NavLink className="dropdown-item" to="/status/capabilities" >Capabilities</NavLink>
                                    </li>
                                    <li>
                                        <NavLink className="dropdown-item" to="/status/services">Services</NavLink>
                                    </li>
                                    <li>
                                        <NavLink className="dropdown-item" to="/status/equipment">Equipment</NavLink>
                                    </li>
                                </ul>
                            </li>
                            <li className="nav-item dropdown">
                                <a className="nav-link text-dark dropdown-toggle" role="button" data-bs-toggle="dropdown">Administration</a>
                                <ul className="dropdown-menu">
                                    <li>
                                        <NavLink className="dropdown-item" to="/admin/capabilities">Capabilities</NavLink>
                                    </li>
                                    <li>
                                        <NavLink className="dropdown-item" to="/admin/services">Services</NavLink>
                                    </li>
                                    <li>
                                        <NavLink className="dropdown-item" to="/admin/equipment">Equipment</NavLink>
                                    </li>
                                    <li>
                                        <NavLink className="dropdown-item" to="/admin/components">Components</NavLink>
                                    </li>
                                    <li>
                                        <NavLink className="dropdown-item" to="/admin/locations">Locations</NavLink>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </div>
                </div>
            </nav>
        </header>
    )
}

export default Header;
import React from "react";
import { NavItem, NavLink } from "reactstrap";
import { Link } from "react-router-dom";
import { ApplicationPaths } from "./constant";

interface PropType {}

const LoginMenu = ({ ...props }: PropType) => {
  return (
    <>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to={ApplicationPaths.Register}>
          Register
        </NavLink>
      </NavItem>
      <NavItem>
        <NavLink tag={Link} className="text-dark" to={ApplicationPaths.Login}>
          Login
        </NavLink>
      </NavItem>
    </>
  );
};

export default LoginMenu;

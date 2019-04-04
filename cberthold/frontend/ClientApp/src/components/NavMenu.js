import React from 'react';
import { Link } from 'react-router-dom';
import { Glyphicon, Nav, Navbar, NavItem } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import './NavMenu.css';

export default props => (
  <Navbar inverse fixedTop fluid collapseOnSelect>
    <Navbar.Header>
      <Navbar.Brand>
        <Link to={'/'}>frontend</Link>
      </Navbar.Brand>
      <Navbar.Toggle />
    </Navbar.Header>
    <Navbar.Collapse>
      <Nav>
        <LinkContainer to={'/'} exact>
          <NavItem>
            <Glyphicon glyph='home' /> Home
          </NavItem>
        </LinkContainer>
        <LinkContainer to={'/transactions'}>
          <NavItem>
            <Glyphicon glyph='th-list' /> Transactions
          </NavItem>
        </LinkContainer>
        <LinkContainer to={'/deposit'}>
          <NavItem>
            <Glyphicon glyph='education' /> Deposit
          </NavItem>
        </LinkContainer>
        <LinkContainer to={'/withdrawal'}>
          <NavItem>
            <Glyphicon glyph='education' /> Withdrawal
          </NavItem>
        </LinkContainer>
      </Nav>
    </Navbar.Collapse>
  </Navbar>
);

// src/components/Navbar.jsx
import React from "react";
import { NavLink } from "react-router-dom";
import "./Navbar.css";

function Navbar({ user, onLogout }) {
  return (
    <nav className="navbar">
      <div className="navbar-container">
        <NavLink to="/" className="navbar-title">Активность Парламента</NavLink>
        <div className="navbar-links">
          <NavLink to="/">Главная</NavLink>
          <NavLink to="/events">Мероприятия</NavLink>
          <NavLink to="/users">Участники</NavLink>
          <NavLink to="/ratings">Рейтинг</NavLink>
          <NavLink to={`/profile/${user?.Id || user?.id}`}>Профиль</NavLink>
          <button onClick={onLogout} className="logout-btn">Выйти</button>
        </div>
      </div>
    </nav>
  );
}

export default Navbar;
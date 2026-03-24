// src/pages/LoginPage.jsx
import React, { useState } from "react";
import { api } from "../api";

function LoginPage({ onLogin }) {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError("");
const response = await api.post("/auth/login", { email, password });
const user = response.data;
console.log("USER DATA:", user); 
    try {
      const response = await api.post("/auth/login", { email, password });
      const user = response.data;
      
      localStorage.setItem("user", JSON.stringify(user));
      onLogin(user);
    } catch (err) {
      setError(err.response?.data?.message || "Неверный email или пароль");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="login-page">
      <div className="login-container">
        <div className="login-card">
          <div className="login-header">
            <h1>Платформа активности</h1>
            <p>Войдите в систему, чтобы продолжить</p>
          </div>
          
          <form onSubmit={handleSubmit}>
            <div className="form-group">
              <label>Email</label>
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                placeholder="example@test.com"
                required
                autoFocus
              />
            </div>
            
            <div className="form-group">
              <label>Пароль</label>
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                placeholder="••••••••"
                required
              />
            </div>
            
            {error && <div className="error-message">{error}</div>}
            
            <button type="submit" className="login-btn" disabled={loading}>
              {loading ? "Вход..." : "Войти"}
            </button>
          </form>
          
          <div className="test-accounts">
            <p>Тестовые аккаунты:</p>
            <div className="accounts-list">
              <div className="account-item">
                <span className="account-role">Администратор</span>
                <code>admin@test.com / admin123</code>
              </div>
              <div className="account-item">
                <span className="account-role">Организатор</span>
                <code>organizer1@test.com / org123</code>
              </div>
              <div className="account-item">
                <span className="account-role">Участник</span>
                <code>participant1@test.com / user123</code>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default LoginPage;
// src/pages/UsersPage.jsx
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { api } from "../api";
import UserCard from "../components/UserCard";

export default function UsersPage() {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [showAdminPanel, setShowAdminPanel] = useState(false);
  const navigate = useNavigate();
  const user = JSON.parse(localStorage.getItem("user"));
  const isAdmin = user?.role === "Admin";

  useEffect(() => {
    api.get("/user")
      .then(res => {
        setUsers(res.data);
        setLoading(false);
      })
      .catch(err => {
        console.error(err);
        setLoading(false);
      });
  }, []);

  const changeRole = async (userId, newRole) => {
    try {
      await api.put(`/admin/users/${userId}/role`, { role: newRole });
      setUsers(users.map(u => u.id === userId ? { ...u, role: newRole } : u));
    } catch (err) {
      console.error(err);
    }
  };

  if (loading) return <div>Загрузка участников...</div>;

  return (
    <div className="fade-in">
      <div className="page-header">
        <h1>Участники</h1>
        {isAdmin && (
          <button className="btn-secondary" onClick={() => setShowAdminPanel(!showAdminPanel)}>
            {showAdminPanel ? "Скрыть панель" : "Админ панель"}
          </button>
        )}
      </div>

      {isAdmin && showAdminPanel && (
        <div className="admin-panel-compact">
          <h3>Управление ролями</h3>
          <div className="table-wrapper">
            <table>
              <thead>
                <tr>
                  <th>ID</th>
                  <th>Имя</th>
                  <th>Роль</th>
                  <th>Действие</th>
                </tr>
              </thead>
              <tbody>
                {users.map(u => (
                  <tr key={u.id}>
                    <td>{u.id}</td>
                    <td>{u.name}</td>
                    <td>{u.role}</td>
                    <td>
                      <select 
                        value={u.role} 
                        onChange={(e) => changeRole(u.id, e.target.value)}
                        style={{ padding: "4px 8px", borderRadius: "6px" }}
                      >
                        <option value="Participant">Участник</option>
                        <option value="Organizer">Организатор</option>
                        <option value="Admin">Админ</option>
                      </select>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {users.length === 0 ? (
        <div className="empty-state">Пока нет участников</div>
      ) : (
        <div className="grid-4">
          {users.map(user => (
            <div key={user.id} onClick={() => navigate(`/profile/${user.id}`)}>
              <UserCard
                user={{
                  full_name: user.name,
                  role: user.role,
                  city: user.city ?? "—",
                  age: user.age ?? "—",
                  total_points: user.total_points ?? 0
                }}
              />
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
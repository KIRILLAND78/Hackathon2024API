import React, { useState } from 'react';

function AdminPanel({ onLogout }) {
  const [users, setUsers] = useState([
    { id: 1, email: 'user1@example.com', role: 'user' },
    { id: 2, email: 'user2@example.com', role: 'user' },
    { id: 3, email: 'admin@example.com', role: 'admin' },
  ]);

  const handleRoleChange = (id) => {
    const newRole = prompt("Enter new role (user/admin)");
    setUsers(users.map(user => user.id === id ? { ...user, role: newRole } : user));
  };

  return (
    <div className="admin-panel-container">
      <header>
        <h1>Admin Panel</h1>
        <button onClick={onLogout}>Выйти</button>
      </header>
      <h2>Список пользователей</h2>
      <ul>
        {users.map(user => (
          <li key={user.id}>
            <span>{user.email}</span>
            <span>{user.role}</span>
            <button onClick={() => handleRoleChange(user.id)}>Изменить роль</button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default AdminPanel;
